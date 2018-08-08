using System;
using System.Collections.Generic;
using System.Text;
using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using Moneybox.App.Features;
using Moq;
using Xunit;

namespace Moneybox.App.Tests.Features
{
    public class WithDrawMoneyTests
    {
        [Fact]
        public void WhenBalanceSufficientCallsUpdate() {
            var accountId = Guid.NewGuid();
            
            var accountRepository = new Mock<IAccountRepository>();
            var notificationServiceWrapper = new Mock<INotificationServiceWrapper>();
            var fromAcccount = new Account {Balance = 500m};
       
            accountRepository.Setup(x => x.GetAccountById(It.Is<Guid>(g => g == accountId))).Returns(fromAcccount);
            accountRepository.Setup(x => x.Update(fromAcccount));

            var sut = new WithdrawMoney(accountRepository.Object, notificationServiceWrapper.Object);

            sut.Execute(accountId, 100m);

            accountRepository.Verify(x => x.Update(fromAcccount));            
        }

        [Fact]
        public void WhenBalacneInsufficient_ThrowsException_DoesNotCallUpdate()
        {
            var accountId = Guid.NewGuid();

            var accountRepository = new Mock<IAccountRepository>();
            var notificationServiceWrapper = new Mock<INotificationServiceWrapper>();
            var fromAcccount = new Account { Balance = 0m };

            accountRepository.Setup(x => x.GetAccountById(It.Is<Guid>(g => g == accountId))).Returns(fromAcccount);
            accountRepository.Setup(x => x.Update(fromAcccount));

            var sut = new WithdrawMoney(accountRepository.Object, notificationServiceWrapper.Object);
            Assert.Throws<InvalidOperationException>(()=> sut.Execute(accountId, 100m));

            accountRepository.Verify(x => x.Update(fromAcccount), Times.Never);
        }
    }
}
