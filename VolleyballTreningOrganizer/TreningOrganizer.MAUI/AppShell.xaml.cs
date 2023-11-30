using TreningOrganizer.MAUI.Views;

namespace TreningOrganizer.MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("templateTab/templateForm", typeof(MessageTemplateFormPage));
            Routing.RegisterRoute("groupsTab/groupForm", typeof(TrainingGroupFormPage));
            Routing.RegisterRoute("groupsTab/groupForm/contacts", typeof(ContactsPage));
        }
    }
}