using Microsoft.VisualStudio.TestTools.UnitTesting;﻿
namespace UnitTestGasAccountLogic
{
    [TestClass]
    public sealed class Test1
    {
        [TestInitialize]

        public void Setup()
        {
            GasAccount.GasAccount.UnitCost = 0.2; //Result to default
        }
        [TestMethod]
        public void UnitCost_StaticSetter_ShouldAffectBalanceCalculation()
        {

            //Arrange
            GasAccount.GasAccount.UnitCost = 0.5; // change from default 0.2
            var account = new GasAccount.GasAccount(400, "StaticTest", "Test St", 10);

            //Act
            String result = account.RecordUnits(2); //should use new UnitCost

            //Assert 

            Assert.AreEqual("Transaction Successful", result);
            Assert.AreEqual(10 + 2, account.Units);
            Assert.AreEqual((10 * 0.5) + (2 * 0.5), account.Balance);
        }
        [TestMethod]
        public void DefaultConstructor_ShouldInitalizeWithDefaultValues()
        {
            //Arrange and act
            var account = new GasAccount.GasAccount();

            //Assert
            Assert.AreEqual(-999, account.AccRefNo);
            Assert.AreEqual("No Name", account.Name);
            Assert.AreEqual("No Address", account.Address);
            Assert.AreEqual(-9.99, account.Units);
            Assert.AreEqual(-9.99, account.Balance);
        }
        [TestMethod]
        public void ThreeArgConstuctor_ShouldInitalizeWithValues()
        {
            //Arrange and act
            var account = new GasAccount.GasAccount(54321, "Test Name", "Test Address");

            //Assert
            Assert.AreEqual(54321, account.AccRefNo);
            Assert.AreEqual("Test Name", account.Name);
            Assert.AreEqual("Test Address", account.Address);
            Assert.AreEqual(0.0, account.Units);
            Assert.AreEqual(0.0, account.Balance);

        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Consturctor_ShouldThrowException_WhenUnitsNegative()
        {
            // Arrange & Act
            var account = new GasAccount.GasAccount(54321, "jim", "123 mainfeild rd", -5);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Deposit_NegativeAmount_ShouldThrowException()
        {
            var account = new GasAccount.GasAccount();
            account.Deposit(-50);
        }

       
        //Boundary analysis 
        [DataTestMethod]
        [DataRow(1,false)] // valid bva 
        [DataRow(-1, true )] // invalid bva
        [DataRow(50, false)]  //valid deposit 
        [DataRow(0, true)] //Zero deposit - invalid 
        [DataRow(-10, true)] //negtaive deposit - invalid
        public void Deposit_ShouldValidateAmount(double amount, bool expectException)
        {
            var account = new GasAccount.GasAccount(200, "Deposit", "Test st", 10);
            if (expectException)
            {
                Assert.ThrowsException<ArgumentException>(() => account.Deposit(amount));
            }
            else
            {
                double intialBalance = account.Balance;
                account.Deposit(amount);
                Assert.AreEqual(intialBalance - amount, account.Balance);
            }
        }

        //Setter Tests 
        [TestMethod]
        public void Address_Setter_ShouldUpdateValue()
        {
            var account = new GasAccount.GasAccount();
            account.Address = "123 main street";
            Assert.AreEqual("123 main street", account.Address);
        }
        [TestMethod]
        public void AccountRefNo_Setter_ShouldUpdateValue()
        {
            var account = new GasAccount.GasAccount();
            account.AccRefNo = 12345;
            Assert.AreEqual(12345, account.AccRefNo);
        }
        [TestMethod]
        public void Name_Setter_ShouldUpdateValue()
        {
            var account = new GasAccount.GasAccount();
            account.Name = "Luke";
            Assert.AreEqual("Luke", account.Name);
        }
        [TestMethod]
        public void Units_Setter_ShouldUpdateValue()
        {
            var account = new GasAccount.GasAccount();
            account.Units = 2;
            Assert.AreEqual(2, account.Units);
        }
        [TestMethod]
        public void Balance_Setter_ShouldUpdateValue()
        {
            var account = new GasAccount.GasAccount();
            account.Balance = 3;
            Assert.AreEqual(3, account.Balance);
        }




         //Boundary Test 
        [DataTestMethod]
        [DataRow(9_999, false)]   // Below limit → should succeed
        [DataRow(10_000, false)]  // At limit → should succeed
        [DataRow(10_001, true)]   // Above limit → should throw
        public void RecordUnits_UpperBoundaryValues_ShouldBehaveAsExpected(double unitsUsed, bool expectException)
        {
            var account = new GasAccount.GasAccount(300, "Boundary", "Test St", 0);

            if (expectException)
            {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => account.RecordUnits(unitsUsed));
            }
            else
            {
                string result = account.RecordUnits(unitsUsed);

                if (unitsUsed > 0 && unitsUsed <= 10_000)
                {
                    Assert.AreEqual("Transaction Successful", result);
                    Assert.AreEqual(unitsUsed, account.Units);
                    Assert.AreEqual(unitsUsed * GasAccount.GasAccount.UnitCost, account.Balance);
                }
                else
                {
                    Assert.AreEqual("Transaction Unsuccessful", result);
                    Assert.AreEqual(0, account.Units);
                    Assert.AreEqual(0, account.Balance);
                }
            }
        }
        [TestMethod]
        public void Deposit_PositiveAmount_ShouldReduceBalance()
        {
            var account = new GasAccount.GasAccount(100, "Test", "Test", 20);
            double initialBalance = account.Balance;

            account.Deposit(5);

            Assert.AreEqual(initialBalance - 5, account.Balance);
        }
        [TestMethod]
        public void PropertyChanged_ShouldAlert_accrefNo()
        {
            var account = new GasAccount.GasAccount();
            string? changedProp = null;
            account.PropertyChanged += (s, e) => changedProp = e.PropertyName;

            account.AccRefNo = 222;

            Assert.AreEqual("AccRefNo", changedProp);
        }

        [TestMethod]
        public void PropertyChanged_ShouldAlert_name()
        {
            var account = new GasAccount.GasAccount();
            string? changedProp = null;
            account.PropertyChanged += (s, e) => changedProp = e.PropertyName;

            account.Name = "New Name";

            Assert.AreEqual("Name", changedProp);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RecordUnits_ZeroUnits_ShouldThrow()
        {
            var account = new GasAccount.GasAccount();
            account.RecordUnits(0);
        }

        [TestMethod]
        public void Units_Setter_ShouldAlertpropertyChange()
        {
            var account = new GasAccount.GasAccount();
            string? prop = null;
            account.PropertyChanged += (s, e) => prop = e.PropertyName;

            account.Units = 99;

            Assert.AreEqual("Units", prop);
        }
        [TestMethod]
        public void Address_Setter_ShouldAlertpropertyChange()
        {
            var account = new GasAccount.GasAccount();
            string? prop = null;
            account.PropertyChanged += (s, e) => prop = e.PropertyName;

            account.Address = "123 main lane";

            Assert.AreEqual("Address", prop);
        }
        [TestMethod]
        public void Balance_Setter_ShouldAlertpropertyChange()
        {
            var account = new GasAccount.GasAccount();
            string? prop = null;
            account.PropertyChanged += (s, e) => prop = e.PropertyName;

            account.Balance = 5;

            Assert.AreEqual("Balance", prop);
        }









    }
}
