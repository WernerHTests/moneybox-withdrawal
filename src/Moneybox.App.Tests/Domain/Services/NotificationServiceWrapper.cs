using Moneybox.App.Domain.Services;
using Moq;
using Xunit;

namespace Moneybox.App.Tests.Domain.Services
{
    public class NotificationServiceWrapperTest
    {
        [Fact]
        public void SendNotification_WhenBalanceIsLow()
        {
            var email = "emailAdress";
            var notificationServiceMock = new Mock<INotificationService>();
            notificationServiceMock.Setup(x => x.NotifyFundsLow(It.Is<string>( s=> s ==  email)));
            var sut = new NotificationServiceServiceWrapper(notificationServiceMock.Object);
            var account = new Account{ Balance = Account.NotificationLimit - 1, User = new User { Email = email}};
            sut.SendNotifications(account);
            notificationServiceMock.Verify(x => x.NotifyFundsLow(It.Is<string>(s => s == email)));
        }

        [Fact]
        public void SendNotification_WhenPaidInLimit()
        {
            var email = "emailAdress";
            var notificationServiceMock = new Mock<INotificationService>();
            notificationServiceMock.Setup(x => x.NotifyFundsLow(It.Is<string>(s => s == email)));
            var sut = new NotificationServiceServiceWrapper(notificationServiceMock.Object);
            var account = new Account { PaidIn = Account.PayInLimit - Account.NotificationLimit + 1, User = new User { Email = email } };
            sut.SendNotifications(account);
            notificationServiceMock.Verify(x => x.NotifyFundsLow(It.Is<string>(s => s == email)));
        }
    }
}
