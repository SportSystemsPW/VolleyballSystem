﻿namespace TreningOrganizer.API
{
    public static class MessageRepository
    {
        public static string EmptyGroup = "Group must have at least one member";

        public static string CannotEditObject(string objectType)
        {
            return $"You can't edit this {objectType}";
        }
        public static string CannotRemoveObject(string objectType)
        {
            return $"You can't remove this {objectType}";
        }
        public static string FieldTooLong(string fieldName, int maxLength)
        {
            return $"{fieldName} can't be longer than {maxLength} characters";
        }
        public static string FieldEmpty(string fieldName)
        {
            return $"{fieldName} can't be empty";
        }
    }
}
