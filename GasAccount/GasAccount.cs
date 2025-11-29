using System;
using System.ComponentModel;
using GasAccount;


namespace GasAccount
{
    public class GasAccount : INotifyPropertyChanged
    {
        // Static property
        public static double UnitCost { get; set; } = 0.2;


        // Instance fields
        private int accRefNo;
        private string name;
        private string address;
        private double balance;
        private double units;

        // Properties with change notification
        public int AccRefNo
        {
            get => accRefNo;
            set
            {
                if (accRefNo != value)
                {
                    accRefNo = value;
                    OnPropertyChanged(nameof(AccRefNo));
                }
            }
        }

        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Address
        {
            get => address;
            set
            {
                if (address != value)
                {
                    address = value;
                    OnPropertyChanged(nameof(Address));
                }
            }
        }

        public double Balance
        {
            get => balance;
            set
            {
                if (balance != value)
                {
                    balance = value;
                    OnPropertyChanged(nameof(Balance));
                }
            }
        }

        public double Units
        {
            get => units;
            set
            {
                if (units != value)
                {
                    units = value;
                    OnPropertyChanged(nameof(Units));
                }
            }
        }

        // Constructors
        public GasAccount()
        {
            AccRefNo = -999;
            Name = "No Name";
            Address = "No Address";
            Units = -9.99;
            Balance = -9.99;
        }

        public GasAccount(int accRefNo, string name, string address, double units)
        {

            if (units < 0)
            {
                throw new ArgumentException("Units cannot be negative.", nameof(units));
            }

            AccRefNo = accRefNo;
            Name = name;
            Address = address;
            Units = units;
            Balance = units * UnitCost;
        }

        public GasAccount(int accRefNo, string name, string address)
        {
            AccRefNo = accRefNo;
            Name = name;
            Address = address;
            Units = 0;
            Balance = 0;
        }

        // Business logic
        public void Deposit(double amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Deposit amount must be positive.", nameof(amount));
            }
            Balance -= amount;
        }


        // NOTE 1: Invalid input → throw exception
        // NOTE 2: The difference between soft failure (return message) vs hard failure (exception).
        public string RecordUnits(double unitsUsed)
        {
            if (unitsUsed < 1)
                throw new ArgumentOutOfRangeException(nameof(unitsUsed), "A minimum of 1 Unit is required.");

            if (unitsUsed > 10_000)
                throw new ArgumentOutOfRangeException(nameof(unitsUsed), "Units exceed allowed maximum (10,000).");

            double cost = unitsUsed * UnitCost;
            Balance += cost;
            Units += unitsUsed;
            return "Transaction Successful";
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
