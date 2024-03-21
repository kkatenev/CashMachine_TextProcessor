using CashMachine.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CashMachine
{
    public class CashMachineViewModel : INotifyPropertyChanged
    {
        private CashMachineModel CashMachine;

        public ObservableCollection<Banknote> AvailableBanknotes => CashMachine.AvailableBanknotes;


        public ObservableCollection<IGrouping<int, Banknote>> AvailableBanknotesGrouped
        {
            get { return availableBanknotesGrouped; }
            set
            {
                if (availableBanknotesGrouped != value)
                {
                    availableBanknotesGrouped = value;
                    OnPropertyChanged(nameof(AvailableBanknotesGrouped));
                }
            }
        }
        private ObservableCollection<IGrouping<int, Banknote>> availableBanknotesGrouped;

        public int LastDenomination
        {
            get { return _lastDenomination; }
            set
            {
                if (_lastDenomination != value)
                {
                    _lastDenomination = value;
                    OnPropertyChanged(nameof(LastDenomination));
                }
            }
        }
        private int _lastDenomination;

        public Banknote SelectedBanknote
        {
            get { return selectedBanknote; }
            set
            {
                if (selectedBanknote != value && value != null)
                {
                    if (value != null)
                    {
                        LastDenomination = value.Denomination;
                        OnPropertyChanged(nameof(LastDenomination));

                    }
                    selectedBanknote = value;
                    OnPropertyChanged(nameof(SelectedBanknote));
                }
            }
        }
        private Banknote selectedBanknote;        
        
        public int RemainingAmount
        {
            get { return _remainingAmount; }
            set
            {
                if (_remainingAmount != value)
                {
                    _remainingAmount = value;
                    OnPropertyChanged(nameof(RemainingAmount));
                }
            }
        }
        private int _remainingAmount;

        public CashMachineViewModel(CashMachineModel cashMachineModel)
        {
            CashMachine = cashMachineModel;
            UpdateAvailableBanknotesGrouped();
        }
        public CashMachineViewModel()
        {
            CashMachine = new CashMachineModel();
            UpdateAvailableBanknotesGrouped();
        }
        public ICommand DepositCommand
        {
            get
            {
                return depositCommand ?? (depositCommand = new RelayCommand(
                    param => DepositBanknote(this.SelectedBanknote.Denomination),
                    param => true
                ));
            }
        }
        private ICommand depositCommand;

        public ICommand WithdrawCommand
        {
            get
            {
                return withdrawCommand ?? (withdrawCommand = new RelayCommand(
                    param => WithdrawBanknotes(this.RemainingAmount, this.LastDenomination),
                    param => true
                ));
            }
        }
        private ICommand withdrawCommand;

        private void DepositBanknote(int denomination)
        {
            if (SelectedBanknote != null) // Проверяем, не равен ли SelectedBanknote null
            {
                CashMachine.DepositBanknote(new Banknote { Denomination = denomination });
                var sortedBanknotes = new ObservableCollection<Banknote>(AvailableBanknotes.OrderBy(x => x.Denomination));
                CashMachine.AvailableBanknotes = sortedBanknotes;
                OnPropertyChanged(nameof(AvailableBanknotes));
                UpdateAvailableBanknotesGrouped();
            }
        }

        private void WithdrawBanknotes(int RemainingAmount, int denomination)
        {
            var remainingAmount = RemainingAmount;
            var banknotesToWithdraw = new List<Banknote>();
            var remainingBanknotes = new List<Banknote>();

            var banknoteGroup = AvailableBanknotesGrouped.FirstOrDefault(group => group.Key == denomination);
            if (banknoteGroup != null)
            {
                var banknoteDenomination = banknoteGroup.Key;
                var banknotesAvailable = banknoteGroup.Count();

                while (remainingAmount >= banknoteDenomination && banknotesAvailable > 0)
                {
                    banknotesToWithdraw.Add(new Banknote { Denomination = banknoteDenomination });

                    remainingAmount -= banknoteDenomination;
                    banknotesAvailable--;
                }

                foreach (var banknote in banknoteGroup.Skip(banknoteGroup.Count() - banknotesAvailable))
                {
                    remainingBanknotes.Add(new Banknote { Denomination = banknote.Denomination });
                }

                for (int i = AvailableBanknotes.Count - 1; i >= 0; i--)
                {
                    if (remainingAmount - AvailableBanknotes[i].Denomination >= 0)
                    {
                        banknotesToWithdraw.Add(new Banknote { Denomination = AvailableBanknotes[i].Denomination });
                        remainingAmount -= AvailableBanknotes[i].Denomination;
                        AvailableBanknotes.RemoveAt(i);
                    }
                }

                foreach (var banknote in banknotesToWithdraw)
                {
                    for (int i = AvailableBanknotes.Count - 1; i >= 0; i--)
                    {
                        if (AvailableBanknotes[i].Denomination == banknote.Denomination)
                        {
                            AvailableBanknotes.RemoveAt(i);
                            break; 
                        }
                    }
                }

                StringBuilder changeMessage = new StringBuilder();
                for (int i = 0; i < banknotesToWithdraw.Count; i++)
                {
                    changeMessage.Append(banknotesToWithdraw[i].Denomination + " ");
                }
                if (banknotesToWithdraw.Count == 0)
                {
                    changeMessage = new StringBuilder("0");
                }
                MessageBox.Show($"Вам выдано: {changeMessage}\nВаша сдача:{remainingAmount}");
                UpdateAvailableBanknotesGrouped();

            }
        }


        private void UpdateAvailableBanknotesGrouped()
        {
            AvailableBanknotesGrouped = new ObservableCollection<IGrouping<int, Banknote>>(
                CashMachine.AvailableBanknotes.GroupBy(x => x.Denomination));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

