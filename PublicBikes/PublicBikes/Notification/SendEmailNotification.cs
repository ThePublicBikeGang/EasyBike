namespace PublicBikes.Notification
{
    public class SendEmailNotification : Notification
    {
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
