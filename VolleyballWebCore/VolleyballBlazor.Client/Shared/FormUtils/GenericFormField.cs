using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Volleyball.DTO;
using VolleyballBlazor.Client.Models;
using VolleyballBlazor.Client.Shared.Components;

namespace VolleyballBlazor.Client.Shared.FormUtils
{
    public sealed class GenericFormField<TModel>
    {
        private static readonly MethodInfo s_eventCallbackFactoryCreate = GetEventCallbackFactoryCreate();

        private readonly GenericForm<TModel> _form;
        private RenderFragment? _editorTemplate;
        private RenderFragment? _fieldValidationTemplate;

        public event EventHandler? ValueChanged;

        private GenericFormField(GenericForm<TModel> form, PropertyInfo propertyInfo)
        {
            _form = form;
            Property = propertyInfo;
        }

        internal static List<GenericFormField<TModel>> Create(GenericForm<TModel> form)
        {
            var result = new List<GenericFormField<TModel>>();
            var properties = typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            foreach (var prop in properties)
            {
                // Skip readonly properties
                if (prop.SetMethod == null)
                    continue;

                if (prop.GetCustomAttribute<EditableAttribute>() is { } editor && !editor.AllowEdit)
                    continue;

                if (prop.GetCustomAttribute<ExcludeFromFormAttribute>() != null)
                    continue;

                var field = new GenericFormField<TModel>(form, prop);
                result.Add(field);
            }

            return result;
        }

        public PropertyInfo Property { get; }
        public string EditorId => _form.BaseEditorId + '_' + Property.Name;
        public TModel Owner => _form.Model!;

        public string DisplayName
        {
            get
            {
                var displayAttribute = Property.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute != null)
                {
                    var displayName = displayAttribute.GetName();
                    if (!string.IsNullOrEmpty(displayName))
                        return displayName;
                }

                var displayNameAttribute = Property.GetCustomAttribute<DisplayNameAttribute>();
                if (displayNameAttribute != null)
                {
                    var displayName = displayNameAttribute.DisplayName;
                    if (!string.IsNullOrEmpty(displayName))
                        return displayName;
                }

                return Property.Name;
            }
        }

        public string? Description
        {
            get
            {
                var displayAttribute = Property.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute != null)
                {
                    var description = displayAttribute.GetDescription();
                    if (!string.IsNullOrEmpty(description))
                        return description;
                }

                var descriptionAttribute = Property.GetCustomAttribute<DescriptionAttribute>();
                if (descriptionAttribute != null)
                {
                    var description = descriptionAttribute.Description;
                    if (!string.IsNullOrEmpty(description))
                        return description;
                }

                return null;
            }
        }

        public Type PropertyType => Property.PropertyType;

        public object? Value
        {
            get => Property.GetValue(Owner);
            set
            {
                if (Property.SetMethod != null && !Equals(Value, value))
                {
                    Property.SetValue(Owner, value);
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public RenderFragment EditorTemplate
        {
            get
            {
                if (_editorTemplate != null)
                    return _editorTemplate;
                var propertyType = Property.PropertyType;
                var propertyName = Property.Name;


                // () => Owner.Property
                var access = Expression.Property(Expression.Constant(Owner, typeof(TModel)), Property);
                var lambda = Expression.Lambda(typeof(Func<>).MakeGenericType(PropertyType), access);

                // Create(object receiver, Action<object> callback
                var method = s_eventCallbackFactoryCreate.MakeGenericMethod(PropertyType);

                // value => Field.Value = value;
                var changeHandlerParameter = Expression.Parameter(PropertyType);
                var body = Expression.Assign(Expression.Property(Expression.Constant(this), nameof(Value)), Expression.Convert(changeHandlerParameter, typeof(object)));
                var changeHandlerLambda = Expression.Lambda(typeof(Action<>).MakeGenericType(PropertyType), body, changeHandlerParameter);
                var changeHandler = method.Invoke(EventCallback.Factory, new object[] { this, changeHandlerLambda.Compile() });

                if (propertyType == typeof(byte[]))
                {
                    return builder =>
                    {
                        builder.OpenComponent<ImageUpload>(0);
                        builder.AddAttribute(1, "ImageChange", EventCallback.Factory.Create(this, (InputFileChangeEventArgs e) => HandleFileChange(e)));
                        builder.AddAttribute(2, "Image", Value);
                        builder.CloseComponent();
                    };
                }
                else if (propertyName.EndsWith("Id"))
                {
                    // Handle Id field with associated list
                    var listPropertyName = $"{propertyName.Substring(0, propertyName.Length - 2)}List";
                    var listProperty = typeof(TModel).GetProperty(listPropertyName);

                    if (listProperty != null)
                    {
                        var options = (IEnumerable<ISelectValue>?)listProperty.GetValue(Owner);

                        if (options == null)
                        {
                            options = new List<ISelectValue>();
                            listProperty.SetValue(Owner, options);
                        }

                        return builder =>
                        {
                            builder.OpenElement(0, "select");
                            builder.AddAttribute(1, "value", BindConverter.FormatValue(Value));
                            builder.AddAttribute(2, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(this, async value =>
                            {
                                Value = int.Parse(value.Value?.ToString());
                                await Task.CompletedTask; // Add this line to ensure a Task is returned
                            }));
                            builder.AddAttribute(3, "class", _form.EditorClass);

                            foreach (var item in options)
                            {
                                builder.OpenElement(4, "option");
                                builder.AddAttribute(5, "value", item.Id);
                                builder.AddContent(6, item.Name);
                                builder.CloseElement();
                            }
                            builder.CloseComponent();
                        };
                    }
                }


                return _editorTemplate ??= builder =>
                {
                    var (componentType, additonalAttributes) = GetEditorType(Property);
                    builder.OpenComponent(0, componentType);
                    builder.AddAttribute(1, "Value", Value);
                    builder.AddAttribute(2, "ValueChanged", changeHandler);
                    builder.AddAttribute(3, "ValueExpression", lambda);
                    builder.AddAttribute(4, "id", EditorId);
                    builder.AddAttribute(5, "class", _form.EditorClass);
                    builder.AddMultipleAttributes(6, additonalAttributes);
                    builder.CloseComponent();
                };
            }
        }

        private void HandleImageCleared()
        {
            Value = null;
        }

        private async Task HandleFileChange(InputFileChangeEventArgs? e)
        {
            var file = e?.File;

            if (file != null)
            {
                var buffer = new byte[file.Size];
                await file.OpenReadStream(maxAllowedSize: 6000000).ReadAsync(buffer);

                Value = buffer;
            }
            else
            {
                Value = null;
            }
        }

        public static byte[] GetBytes(Stream stream)
        {
            var bytes = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.ReadAsync(bytes, 0, bytes.Length);
            stream.Dispose();
            return bytes;
        }

        public RenderFragment? FieldValidationTemplate
        {
            get
            {
                if (!_form.EnableFieldValidation)
                    return null;

                return _fieldValidationTemplate ??= builder =>
                {
                    // () => Owner.Property
                    var access = Expression.Property(Expression.Constant(Owner, typeof(TModel)), Property);
                    var lambda = Expression.Lambda(typeof(Func<>).MakeGenericType(PropertyType), access);

                    builder.OpenComponent(0, typeof(ValidationMessage<>).MakeGenericType(PropertyType));
                    builder.AddAttribute(1, "For", lambda);
                    builder.CloseComponent();
                };
            }
        }

        private static (Type ComponentType, IEnumerable<KeyValuePair<string, object>>? AdditonalAttributes) GetEditorType(PropertyInfo property)
        {
            var editorAttributes = property.GetCustomAttributes<EditorAttribute>();
            foreach (var editorAttribute in editorAttributes)
            {
                if (editorAttribute.EditorBaseTypeName == typeof(InputBase<>).AssemblyQualifiedName)
                    return (Type.GetType(editorAttribute.EditorTypeName, throwOnError: true)!, null);
            }

            if (property.PropertyType == typeof(bool))
                return (typeof(InputCheckbox), null);

            if (property.PropertyType == typeof(string))
            {
                var dataType = property.GetCustomAttribute<DataTypeAttribute>();
                if (dataType != null)
                {
                    if (dataType.DataType == DataType.Date)
                        return (typeof(InputText), new[] { KeyValuePair.Create<string, object>("type", "date") });

                    if (dataType.DataType == DataType.DateTime)
                        return (typeof(InputText), new[] { KeyValuePair.Create<string, object>("type", "datetime-local") });

                    if (dataType.DataType == DataType.EmailAddress)
                        return (typeof(InputText), new[] { KeyValuePair.Create<string, object>("type", "email") });

                    if (dataType.DataType == DataType.MultilineText)
                        return (typeof(InputTextArea), null);

                    if (dataType.DataType == DataType.Password)
                        return (typeof(InputText), new[] { KeyValuePair.Create<string, object>("type", "password") });

                    if (dataType.DataType == DataType.PhoneNumber)
                        return (typeof(InputText), new[] { KeyValuePair.Create<string, object>("type", "tel") });

                    if (dataType.DataType == DataType.Time)
                        return (typeof(InputText), new[] { KeyValuePair.Create<string, object>("type", "time") });

                    if (dataType.DataType == DataType.Url)
                        return (typeof(InputText), new[] { KeyValuePair.Create<string, object>("type", "url") });

                    if (dataType.DataType == DataType.Upload)
                    {
                        return (typeof(InputFile), new[] { KeyValuePair.Create<string, object>("type", "file") }); ;
                    }
                }

                return (typeof(InputText), null);
            }
            if (property.PropertyType == typeof(short))
                return (typeof(InputNumber<short>), null);

            if (property.PropertyType == typeof(int))
                return (typeof(InputNumber<int>), null);

            if (property.PropertyType == typeof(long))
                return (typeof(InputNumber<long>), null);

            if (property.PropertyType == typeof(float))
                return (typeof(InputNumber<float>), null);

            if (property.PropertyType == typeof(double))
                return (typeof(InputNumber<double>), null);

            if (property.PropertyType == typeof(decimal))
                return (typeof(InputNumber<decimal>), null);

            Console.WriteLine("OK");
            if (property.PropertyType == typeof(DateTime))
            {
                var dataType = property.GetCustomAttribute<DataTypeAttribute>();
                if (dataType != null && dataType.DataType == DataType.Date)
                    return (typeof(InputDate<DateTime>), null);

                return (typeof(InputDateTime<DateTime>), null);
            }

            if (property.PropertyType == typeof(DateTimeOffset))
            {
                var dataType = property.GetCustomAttribute<DataTypeAttribute>();
                if (dataType != null && dataType.DataType == DataType.Date)
                    return (typeof(InputDate<DateTimeOffset>), null);

                return (typeof(InputDateTime<DateTimeOffset>), null);
            }

            if (property.PropertyType == typeof(byte[]))
            {
                return (typeof(InputFile), new[] { KeyValuePair.Create<string, object>("type", "file") }); ;
            }

            return (typeof(InputText), null);
        }

        private static MethodInfo GetEventCallbackFactoryCreate()
        {
            return typeof(EventCallbackFactory).GetMethods()
                .Single(m =>
                {
                    if (m.Name != "Create" || !m.IsPublic || m.IsStatic || !m.IsGenericMethod)
                        return false;

                    var generic = m.GetGenericArguments();
                    if (generic.Length != 1)
                        return false;

                    var args = m.GetParameters();
                    return args.Length == 2 && args[0].ParameterType == typeof(object) && args[1].ParameterType.IsGenericType && args[1].ParameterType.GetGenericTypeDefinition() == typeof(Action<>);
                });
        }
    }
}
