using CashMachine.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
namespace CashMachine.Tests
{
    [TestClass]
    public class CashMachineTests
    {
        [TestMethod]
        public void DepositCommand_CanExecute_ReturnsTrueWhenSelectedBanknoteNotNull()
        {
            var viewModel = new CashMachineViewModel();
            viewModel.SelectedBanknote = new Banknote { Denomination = 100 }; 

            bool canExecute = viewModel.DepositCommand.CanExecute(null);

            Assert.IsTrue(canExecute);
        }

        [TestMethod]
        public void AvailableBanknotesGrouped_IsUpdated_AfterDeposit()
        {
            var mockCashMachineModel = new Mock<CashMachineModel>();
            var viewModel = new CashMachineViewModel(mockCashMachineModel.Object);

            viewModel.SelectedBanknote = new Banknote { Denomination = 100 };

            viewModel.DepositCommand.Execute(null);

            Assert.IsNotNull(viewModel.AvailableBanknotesGrouped);
        }

        [TestMethod]
        public void WithdrawCommand_CanExecute_ReturnsTrueWhenRemainingAmountGreaterThanZero()
        {
            var viewModel = new CashMachineViewModel();
            viewModel.RemainingAmount = 1000;

            bool canExecute = viewModel.WithdrawCommand.CanExecute(null);

            Assert.IsTrue(canExecute);
        }
    }
}
