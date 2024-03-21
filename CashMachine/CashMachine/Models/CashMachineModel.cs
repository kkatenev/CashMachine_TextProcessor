using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CashMachine.Models
{
    public class CashMachineModel
    {
        public ObservableCollection<Banknote> AvailableBanknotes { get; set; } = new ObservableCollection<Banknote>();

        public int MaxBanknotesCount { get; } = 100;

        readonly List<int> BanknoteNaminal = new List<int> { 50, 100, 500, 1000, 5000 };

        public CashMachineModel()
        {
            ResetCashMachine();
        }

        public void DepositBanknote(Banknote banknote)
        {
            if (AvailableBanknotes.Count < MaxBanknotesCount)
            {
                AvailableBanknotes.Add(banknote);
            }
        }

        public bool WithdrawBanknotes(int denomination, int quantity)
        {
            int count = 0;
            foreach (Banknote banknote in AvailableBanknotes)
            {
                if (banknote.Denomination == denomination)
                {
                    count++;
                    if (count <= quantity)
                    {
                        AvailableBanknotes.Remove(banknote);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return count == quantity;
        }

        private void ResetCashMachine()
        {
            AvailableBanknotes.Clear();
            foreach (var currentNaminal in BanknoteNaminal)
            {
                Thread.Sleep(1);
                var random = new Random().Next(2, 30);
                for (int i = 0; i < random; i++)
                {
                    AvailableBanknotes.Add(new Banknote
                    {
                        Denomination = currentNaminal
                    });
                }
            }
        }
    }

    public class Banknote : INotifyPropertyChanged
    {
        public int Denomination
        {
            get { return denomination; }
            set
            {
                if (denomination != value)
                {
                    denomination = value;
                }
                OnPropertyChanged(nameof(Denomination));
            }
        }
        private int denomination;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}