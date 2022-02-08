namespace AppUnitTestProject.Validation
{
    using App.BusinessLogic;
    using App.Interfaces;
    using App.Models;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    public class CustomerCreditOrchestratorTests
    {
        private Mock<ICustomerCreditService> CustomerCreditServiceMock;
        private CustomerCreditOrchestrator CustomerCreditOrchestrator;
        private Customer Customer;

        [SetUp]
        public void Setup()
        {
            this.CustomerCreditServiceMock = new Mock<ICustomerCreditService>();

            this.Customer = new Customer
            {
                Firstname = "Jon",
                Lastname = "Snow",
                DateOfBirth = DateTime.Parse("01/01/1990"),
                EmailAddress = "jonsnow@thenightswatch.com",
                Company = new Company()
            };
        }

        [Test]
        public void WhenCustomerVeryImportant_ThenReturnCustomerNoCreditLimit()
        {
            this.CustomerCreditOrchestrator = new CustomerCreditOrchestrator(this.CustomerCreditServiceMock.Object);

            this.Customer.Company.Name = "VeryImportantClient";

            Customer actual = this.CustomerCreditOrchestrator.SetCustomerCreditDetails(this.Customer).Result;

            Assert.IsFalse(actual.HasCreditLimit);
        }

        [Test]
        public void WhenCustomerImportant_ThenHasCreditLimitReturnsTrueAndDoubleCreditLimit()
        {
            this.CustomerCreditOrchestrator = new CustomerCreditOrchestrator(this.CustomerCreditServiceMock.Object);

            this.Customer.Company.Name = "ImportantClient";

            this.CustomerCreditServiceMock.Setup(x => x.GetCreditLimit(this.Customer.Firstname, this.Customer.Lastname, this.Customer.DateOfBirth)).Returns(Task.FromResult(1000));

            Customer actual = this.CustomerCreditOrchestrator.SetCustomerCreditDetails(this.Customer).Result;

            Assert.IsTrue(actual.HasCreditLimit);
            Assert.AreEqual(2000, actual.CreditLimit);
        }

        [Test]
        public void WhenCustomerStandard_ThenHasCreditLimitReturnsTrueAndNormalCreditLimit()
        {
            this.Customer.Company.Name = "StandardClient";

            this.CustomerCreditServiceMock.Setup(x => x.GetCreditLimit(this.Customer.Firstname, this.Customer.Lastname, this.Customer.DateOfBirth)).Returns(Task.FromResult(1000));

            Customer actual = this.CustomerCreditOrchestrator.SetCustomerCreditDetails(this.Customer).Result;

            Assert.IsTrue(actual.HasCreditLimit);
            Assert.AreEqual(1000, actual.CreditLimit);
        }
    }
}