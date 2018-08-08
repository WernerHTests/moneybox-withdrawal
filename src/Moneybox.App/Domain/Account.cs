using System;

namespace Moneybox.App
{
    public class Account
    {
        public const decimal PayInLimit = 4000m;
        public const decimal NotificationLimit = 500m;

        public Guid Id { get; set; }

        public User User { get; set; }

        public decimal Balance { get; set; }

        public decimal Withdrawn { get; set; }

        public decimal PaidIn { get; set; }

        public void WithdrawMoney(decimal amount)
        {
            var afterWithdraw = Balance - amount;
            if (afterWithdraw < 0m)
            {
                throw new InvalidOperationException("Insufficient funds to make transfer");
            }
            Balance = Balance - amount;
            Withdrawn = Withdrawn - amount;       
        }

        public void ReceiveMoney(decimal amount)
        {
            var paidIn = PaidIn + amount;
            if (paidIn > Account.PayInLimit)
            {
                throw new InvalidOperationException("Account pay in limit reached");
            }
            Balance = Balance + amount;
            PaidIn = PaidIn + amount;
        }

        public bool FundsLowWarning => Balance < NotificationLimit;

        public bool PayInLimitWarning => Account.PayInLimit - PaidIn < NotificationLimit;

    }
}
