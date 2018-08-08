using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Moneybox.App.Tests.Domain
{
    public class AccountTests
    {
        [Fact]
        public void WithdrawMoney_ThrowsException_WhenBalanceInsufficient()
        {
            var sut = new Account {Balance = 10m};
            Assert.Throws<InvalidOperationException>(() => sut.WithdrawMoney(100m));
        }

        [Fact]
        public void WithdrawMoney_UpdatesBalancesCorrect()
        {
            var sut = new Account { Balance = 10m };
            sut.WithdrawMoney(5m);
            Assert.Equal(5m, sut.Balance);
        }

        [Fact]
        public void WithdrawMoney_UpdatesWithDrawnCorrect()
        {
            var sut = new Account { Balance = 10m, Withdrawn = -5m};
            sut.WithdrawMoney(5m);
            Assert.Equal(-10m, sut.Withdrawn);
        }
        [Fact]
        public void WithdrawMoney_UpdatesBalancesCorrectFor0Balance()
        {
            var sut = new Account { Balance = 5m };
            sut.WithdrawMoney(5m);
            Assert.Equal(0, sut.Balance);
        }

        [Fact]
        public void ReceiveMoney_UpdatesBalanceCorrect()
        {
            var sut = new Account {Balance = 100m, PaidIn = 100m};
            sut.ReceiveMoney(50);
            Assert.Equal(150, sut.Balance);
        }

        [Fact]
        public void ReceiveMoney_UpdatesPaidInCorrect()
        {
            var sut = new Account { Balance = 100m, PaidIn = 100m };
            sut.ReceiveMoney(50);
            Assert.Equal(150, sut.PaidIn);
        }

        [Fact]
        public void ReceiveMoney_ThrowExceptionWhenPaidInLimitReached()
        {
            var sut = new Account {PaidIn = Account.PayInLimit};
            Assert.Throws<InvalidOperationException>(() => sut.ReceiveMoney(1m));
        }

        [Fact]
        public void FundsLowWarning_ShouldBeTrue_AfterWithDrawnUnderLimit()
        {
            var sut = new Account {Balance = Account.NotificationLimit};
            sut.WithdrawMoney(1m);
            Assert.True(sut.FundsLowWarning);
        }

        [Fact]
        public void PayInLimitWarning_ShouldBeTrue_AfterReceive()
        {
            var sut = new Account { PaidIn = Account.PayInLimit - Account.NotificationLimit};
            sut.ReceiveMoney(1m);
            Assert.True(sut.PayInLimitWarning);
        }


    }
}
