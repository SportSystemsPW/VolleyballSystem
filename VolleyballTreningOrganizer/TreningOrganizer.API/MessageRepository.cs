namespace TreningOrganizer.API
{
    public static class MessageRepository
    {
        public static string MessageTemplateTooLong = "Message template content can't be longer than 300 characters";
        public static string MessageTemplateNameTooLong = "Message template name can't be longer than 50 characters";
        public static string MessageTemplateEmpty = "Message template content can't be empty";
        public static string MessageTemplateNameEmpty = "Message template name can't be empty";
        public static string CannotRemoveMessageTemplate = "You can't remove this message template";
        public static string CannotEditMessageTemplate = "You can't edit this message template";

        public static string CannotRemoveTrainingParticipant = "";
        public static string CannotRemoveTrainingGroup = "";
        public static string CannotRemoveTraining = "";
    }
}
