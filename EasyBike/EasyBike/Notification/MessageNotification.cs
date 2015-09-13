using System;

namespace EasyBike.Notification
{
    public class RefreshFailureNotification : Notification
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string ContractName { get; set; }
        public Exception Exception { get; set; }
    }
}
