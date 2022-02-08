namespace AppUnitTestProject.Validation
{
    using App.Validation;
    using App.Models;
    using NUnit.Framework;
    using System;

    public class CustomerValidatorTests
    {
        private CustomerValidator CustomerValidator;
        private Customer Customer;

        [SetUp]
        public void Setup()
        {
            this.CustomerValidator = new CustomerValidator();
            this.Customer = new Customer()
            {
                Firstname = "Jon",
                Lastname = "Snow",
                EmailAddress = "jon.snow@thenightswatch.com",
                DateOfBirth = DateTime.Parse("01/01/1990")
            };

        }

        [TestCase(" ")]
        [TestCase("   ")]
        [TestCase("")]
        [TestCase(null)]
        public void WhenCustomerFirstNameNullOrWhitespace_ThenValidateCustomerDetailsReturnsFalse(string testFirstName)
        {
            this.Customer.Firstname = testFirstName;
            
            bool actual = this.CustomerValidator.ValidateCustomerDetails(this.Customer);

            Assert.AreEqual(false, actual);
        }

        [TestCase(" ")]
        [TestCase("   ")]
        [TestCase("")]
        [TestCase(null)]
        public void WhenCustomerLastnameNullOrWhitespace_ThenValidateCustomerDetailsReturnsFalse(string testLastname)
        {
            this.Customer.Firstname = testLastname;

            bool actual = this.CustomerValidator.ValidateCustomerDetails(this.Customer);

            Assert.AreEqual(false, actual);
        }

        [TestCase("jonsnowthenightswatch.com")]
        [TestCase("jonsnow@thenightswatchcom")]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void WhenCustomerEmailInvalid_ThenValidateCustomerDetailsReturnsFalse(string testEmail)
        {
            this.Customer.EmailAddress = testEmail;

            bool actual = this.CustomerValidator.ValidateCustomerDetails(this.Customer);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void WhenCustomerUnder21_ThenValidateCustomerDetailsReturnFalse()
        {
            this.Customer.DateOfBirth = DateTime.Now.AddYears(-10);

            bool actual = this.CustomerValidator.ValidateCustomerDetails(this.Customer);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void WhenCustomerDetailsAreAllValid_ThenValidateCustomerDetailsReturnsTrue()
        {
            bool actual = this.CustomerValidator.ValidateCustomerDetails(this.Customer);

            Assert.AreEqual(true, actual);
        }

        [Test]
        public void WhenCustomerHasNoCreditLimit_ThenValidateCustomerCreditDetailsReturnsTrue()
        {
            this.Customer.HasCreditLimit = false;

            bool actual = this.CustomerValidator.ValidateCustomerCreditDetails(this.Customer);

            Assert.AreEqual(true, actual);
        }

        public void WhenCustomerHasCreditLimitGreaterThan499_ThenValidateCustomerCreditDetailsReturnsTrue()
        {

            this.Customer.HasCreditLimit = true;
            this.Customer.CreditLimit = 500;
            
            bool actual = this.CustomerValidator.ValidateCustomerCreditDetails(this.Customer);

            Assert.AreEqual(true, actual);
        }

        public void WhenCustomerHasCreditLimitLessThan500_ThenValidateCustomerCreditDetailsReturnsFalse()
        {
            this.Customer.HasCreditLimit = true;
            this.Customer.CreditLimit = 499;

            bool actual = this.CustomerValidator.ValidateCustomerCreditDetails(this.Customer);

            Assert.AreEqual(false, actual);
        }

    }
}