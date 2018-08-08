using System;
using System.Collections.Generic;
using System.Text;

namespace Moneybox.App.Domain.Services
{
    public class NotificationServiceServiceWrapper : INotificationServiceWrapper
    {
        private readonly INotificationService _notificationService;

        public NotificationServiceServiceWrapper(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public void SendNotifications(Account account)
        {
            if (account.FundsLowWarning)
            {
                this._notificationService.NotifyFundsLow(account.User.Email);
            }
            if (account.PayInLimitWarning)
            {
                this._notificationService.NotifyApproachingPayInLimit(account.User.Email);
            }
        }

    }
}
