using System;

namespace EasyBike.Notification
{
    public interface INotificationService
    {
        void Notify(Notification notification);
        event EventHandler<Notification> OnNotify;
    }
}
