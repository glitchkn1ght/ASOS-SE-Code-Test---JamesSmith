namespace AppUnitTestProject.Validation
{
    using App.BusinessLogic;
    using App.Interfaces;
    using App.Validation;
    using App.Models;
    using App.Service;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    public class CustomerServiceTests
    {
        private Mock<ICustomerValidator> CustomerValidatorMock;

        private Mock<ICompanyRepository> CompanyRepositoryMock;

        private Mock<ICustomerCreditOrchestrator> CustomerCreditOrchestratorMock;

        private Mock<ICustomerRepositoryWrapper> CustomerRepositoryWrapperMock;

        private CustomerService CustomerService;

        private CustomerValidator CustomerValidator;

        private CustomerCreditOrchestrator CustomerCreditOrchestrator;

        private Mock<ICustomerCreditService> CustomerCreditServiceMock;

        private Company Company;

        private string Firstname;
        private string Lastname;
        private string EmailAddress;
        private DateTime DateOfBirth;

        private int CompanyId;

        [SetUp]
        public void Setup()
        {
            this.CustomerValidatorMock = new Mock<ICustomerValidator>();
            this.CompanyRepositoryMock = new Mock<ICompanyRepository>();
            this.CustomerCreditOrchestratorMock = new Mock<ICustomerCreditOrchestrator>();
            this.CustomerRepositoryWrapperMock = new Mock<ICustomerRepositoryWrapper>();
            this.CustomerCreditServiceMock = new Mock<ICustomerCreditService>();

            this.Firstname = "Jon";
            this.Lastname = "Snow";
            this.EmailAddress = "jon.snow@thenightswatch.com";
            this.DateOfBirth = DateTime.Parse("01/01/1990");
            this.CompanyId = 10001;

            this.Company = new Company
            {
                Id = 1001,
                Classification = App.Enums.Classification.Gold
            };
        }

        [Test]
        public void WhenCustomerNotValidated_ThenAddCustomerReturnsFalse()
        {
            this.CustomerService = new CustomerService(this.CustomerValidatorMock.Object, this.CompanyRepositoryMock.Object, this.CustomerCreditOrchestratorMock.Object, this.CustomerRepositoryWrapperMock.Object);

            this.CustomerValidatorMock.Setup(x => x.ValidateCustomerDetails(It.IsAny<Customer>())).Returns(false);
            this.CompanyRepositoryMock.Setup(x => x.GetCompanyById(this.CompanyId)).Returns(new Company());
            
            this.CustomerCreditOrchestratorMock.Setup(x => x.SetCustomerCreditDetails(It.IsAny<Customer>())).Returns(Task.FromResult(new Customer()));

            Assert.IsFalse(this.CustomerService.AddCustomer(this.Firstname, this.Lastname, this.EmailAddress, this.DateOfBirth, this.CompanyId));
        }

        [Test]
        public void WhenCannotRetrieveCompanyFromDatabase_ThenAddCustomerReturnFalse()
        {
            Company nullComp = null;

            this.CustomerService = new CustomerService(this.CustomerValidatorMock.Object, this.CompanyRepositoryMock.Object, this.CustomerCreditOrchestratorMock.Object, this.CustomerRepositoryWrapperMock.Object);

            this.CustomerValidatorMock.Setup(x => x.ValidateCustomerDetails(It.IsAny<Customer>())).Returns(true);
            this.CompanyRepositoryMock.Setup(x => x.GetCompanyById(this.CompanyId)).Returns(nullComp);

            Assert.IsFalse(this.CustomerService.AddCustomer(this.Firstname, this.Lastname, this.EmailAddress, this.DateOfBirth, this.CompanyId));
        }

        [Test]
        public void WhenValidateCustomerCreditReturnsFalse_ThenAddCustomerReturnsFalse()
        {
            this.CustomerService = new CustomerService(this.CustomerValidatorMock.Object, this.CompanyRepositoryMock.Object, this.CustomerCreditOrchestratorMock.Object, this.CustomerRepositoryWrapperMock.Object);

            this.CustomerValidatorMock.Setup(x => x.ValidateCustomerDetails(It.IsAny<Customer>())).Returns(true);
            this.CompanyRepositoryMock.Setup(x => x.GetCompanyById(this.CompanyId)).Returns(new Company());

            this.CustomerCreditOrchestratorMock.Setup(x => x.SetCustomerCreditDetails(It.IsAny<Customer>())).Returns(Task.FromResult(new Customer()));
            this.CustomerValidatorMock.Setup(x => x.ValidateCustomerCreditDetails(It.IsAny<Customer>())).Returns(false);

            Assert.IsFalse(this.CustomerService.AddCustomer(this.Firstname, this.Lastname, this.EmailAddress, this.DateOfBirth, this.CompanyId));
        }

        [Test]
        public void WhenCustomerValid_ThenReturnAddCustomerReturnsTrue()
        {
     
            this.CustomerService = new CustomerService(this.CustomerValidatorMock.Object, this.CompanyRepositoryMock.Object, this.CustomerCreditOrchestratorMock.Object, this.CustomerRepositoryWrapperMock.Object);

            this.CustomerValidatorMock.Setup(x => x.ValidateCustomerDetails(It.IsAny<Customer>())).Returns(true);
            this.CompanyRepositoryMock.Setup(x => x.GetCompanyById(this.CompanyId)).Returns(new Company());

            this.CustomerCreditOrchestratorMock.Setup(x => x.SetCustomerCreditDetails(It.IsAny<Customer>())).Returns(Task.FromResult(new Customer()));
            this.CustomerValidatorMock.Setup(x => x.ValidateCustomerCreditDetails(It.IsAny<Customer>())).Returns(true);

            Assert.IsTrue(this.CustomerService.AddCustomer(this.Firstname, this.Lastname, this.EmailAddress, this.DateOfBirth, this.CompanyId));
        }

  
        [TestCase("VeryImportantClient")]
        [TestCase("ImportantClient")]
        public void WhenCustomerValid_ThenAddCustomerReturnTrue_OnlyDatabaseMethodsMocked(string companyName)
        {
            this.Company.Name = companyName;
            this.CustomerValidator = new CustomerValidator();
            this.CustomerCreditOrchestrator = new CustomerCreditOrchestrator(this.CustomerCreditServiceMock.Object);

            this.CustomerCreditServiceMock.Setup(x => x.GetCreditLimit(this.Firstname, this.Lastname, this.DateOfBirth)).Returns(Task.FromResult(1000));

            this.CustomerService = new CustomerService(this.CustomerValidator, this.CompanyRepositoryMock.Object, this.CustomerCreditOrchestrator, this.CustomerRepositoryWrapperMock.Object);

            this.CompanyRepositoryMock.Setup(x => x.GetCompanyById(this.CompanyId)).Returns(this.Company);
            this.CustomerValidatorMock.Setup(x => x.ValidateCustomerCreditDetails(It.IsAny<Customer>())).Returns(true);

            Assert.IsTrue(this.CustomerService.AddCustomer(this.Firstname, this.Lastname, this.EmailAddress, this.DateOfBirth, this.CompanyId));
        }
    }
}