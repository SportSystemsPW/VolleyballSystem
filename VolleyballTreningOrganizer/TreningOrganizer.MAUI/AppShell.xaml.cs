using TreningOrganizer.MAUI.Views;

namespace TreningOrganizer.MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("login", typeof(LoginPage));
            Routing.RegisterRoute("messageTemplates", typeof(MessageTemplatesPage));
            Routing.RegisterRoute("home", typeof(LoadingPage));
            Routing.RegisterRoute("groups", typeof(TrainingGroupsPage));
            Routing.RegisterRoute("trainings", typeof(TrainingsPage));
            Routing.RegisterRoute("payment", typeof(PaymentPage));
            Routing.RegisterRoute("templateForm", typeof(MessageTemplateFormPage));
            Routing.RegisterRoute("groupForm", typeof(TrainingGroupFormPage));
            Routing.RegisterRoute("contacts", typeof(ContactsPage));
        }
    }
}