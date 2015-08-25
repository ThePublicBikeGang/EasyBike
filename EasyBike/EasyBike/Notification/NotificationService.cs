using System;

namespace EasyBike.Notification
{
    public class NotificationService : INotificationService
    {
        public event EventHandler<Notification> OnNotify;
        public void Notify(Notification notification)
        {
            OnNotify?.Invoke(this, notification);
        }
    }

    public class Notification : EventArgs
    {
    }
}
