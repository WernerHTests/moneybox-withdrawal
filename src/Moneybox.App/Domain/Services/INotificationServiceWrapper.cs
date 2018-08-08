namespace Moneybox.App.Domain.Services
{
    public interface INotificationServiceWrapper
    {
        void SendNotifications(Account account);
    }
}