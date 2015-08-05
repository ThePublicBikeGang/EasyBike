using System;

namespace PublicBikes.Notification
{
    public interface INotificationService
    {
        void Notify(Notification notification);
        event EventHandler<Notification> OnNotify;
    }
}
