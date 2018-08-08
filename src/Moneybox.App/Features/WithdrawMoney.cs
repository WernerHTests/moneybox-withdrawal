using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using System;

namespace Moneybox.App.Features
{
    public class WithdrawMoney
    {
        private IAccountRepository accountRepository;
        private readonly INotificationServiceWrapper _notificationServiceWrapper;
        

        public WithdrawMoney(IAccountRepository accountRepository, INotificationServiceWrapper notificationServiceWrapper)
        {
            this.accountRepository = accountRepository;
            this._notificationServiceWrapper = notificationServiceWrapper;
            
        }

        public void Execute(Guid fromAccountId, decimal amount)
        {
            var from = this.accountRepository.GetAccountById(fromAccountId);
            from.WithdrawMoney(amount);
            _notificationServiceWrapper.SendNotifications(from);
            this.accountRepository.Update(from);
        }
    }
}
