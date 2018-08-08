using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using System;

namespace Moneybox.App.Features
{
    public class TransferMoney
    {
        private IAccountRepository accountRepository;        
        private readonly INotificationServiceWrapper _notificationServiceWrapper;

        public TransferMoney(IAccountRepository accountRepository, INotificationServiceWrapper notificationServiceWrapper)
        {
            this.accountRepository = accountRepository;            
            this._notificationServiceWrapper = notificationServiceWrapper;
        }

        public void Execute(Guid fromAccountId, Guid toAccountId, decimal amount)
        {
            var from = this.accountRepository.GetAccountById(fromAccountId);
            var to = this.accountRepository.GetAccountById(toAccountId);            

            from.WithdrawMoney(amount);
            to.ReceiveMoney(amount);

            _notificationServiceWrapper.SendNotifications(from);
            _notificationServiceWrapper.SendNotifications(to);
            
            this.accountRepository.Update(from);
            this.accountRepository.Update(to);
        }
    }
}
