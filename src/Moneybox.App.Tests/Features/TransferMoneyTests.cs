using System;
using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using Moneybox.App.Features;
using Moq;
using Xunit;

namespace Moneybox.App.Tests.Features
{
    public class TransferMoneyTests
    {
        [Fact]
        public void TransferMoneyCallsUpdate()
        {
            var fromId = Guid.NewGuid();
            var toId = Guid.NewGuid();
            var accountRepository = new Mock<IAccountRepository>();
            var notificationServiceWrapper = new Mock<INotificationServiceWrapper>();
            var fromAcccount = new Account {Balance = 500m};
            var toAccount = new Account();


            accountRepository.Setup(x => x.GetAccountById(It.Is<Guid>(g => g == fromId))).Returns(fromAcccount);
            accountRepository.Setup(x => x.GetAccountById(It.Is<Guid>(g => g == toId))).Returns(toAccount);

            accountRepository.Setup(x => x.Update(fromAcccount));
            accountRepository.Setup(x => x.Update(toAccount));


            var sut = new TransferMoney(accountRepository.Object, notificationServiceWrapper.Object);

            sut.Execute(fromId, toId, 100m );

            accountRepository.Verify(x => x.Update(fromAcccount));
            accountRepository.Verify(x => x.Update(toAccount));

        }

        [Fact]
        public void TransferMoney_WhenBalanceInsufficient_throwsException_DoesNotCallUpdate()
        {
            var fromId = Guid.NewGuid();
            var toId = Guid.NewGuid();
            var accountRepository = new Mock<IAccountRepository>();
            var notificationServiceWrapper = new Mock<INotificationServiceWrapper>();
            var fromAcccount = new Account { Balance = 0 };
            var toAccount = new Account();


            accountRepository.Setup(x => x.GetAccountById(It.Is<Guid>(g => g == fromId))).Returns(fromAcccount);
            accountRepository.Setup(x => x.GetAccountById(It.Is<Guid>(g => g == toId))).Returns(toAccount);

            accountRepository.Setup(x => x.Update(fromAcccount));
            accountRepository.Setup(x => x.Update(toAccount));


            var sut = new TransferMoney(accountRepository.Object, notificationServiceWrapper.Object);

            Assert.Throws<InvalidOperationException>(()=> sut.Execute(fromId, toId, 100m));

            accountRepository.Verify(x => x.Update(fromAcccount),Times.Never);
            accountRepository.Verify(x => x.Update(toAccount), Times.Never);

        }

        [Fact]
        public void TransferMoney_WhenPaidInLimitReached_throwsException_DoesNotCallUpdate()
        {
            var fromId = Guid.NewGuid();
            var toId = Guid.NewGuid();
            var accountRepository = new Mock<IAccountRepository>();
            var notificationServiceWrapper = new Mock<INotificationServiceWrapper>();
            var fromAcccount = new Account { Balance = 500 };
            var toAccount = new Account{PaidIn = Account.PayInLimit};


            accountRepository.Setup(x => x.GetAccountById(It.Is<Guid>(g => g == fromId))).Returns(fromAcccount);
            accountRepository.Setup(x => x.GetAccountById(It.Is<Guid>(g => g == toId))).Returns(toAccount);

            accountRepository.Setup(x => x.Update(fromAcccount));
            accountRepository.Setup(x => x.Update(toAccount));


            var sut = new TransferMoney(accountRepository.Object, notificationServiceWrapper.Object);

            Assert.Throws<InvalidOperationException>(() => sut.Execute(fromId, toId, 100m));

            accountRepository.Verify(x => x.Update(fromAcccount), Times.Never);
            accountRepository.Verify(x => x.Update(toAccount), Times.Never);

        }

    }
}
