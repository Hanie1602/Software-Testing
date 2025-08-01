﻿namespace UniteTestingTutorial
{
    public class BankAccount
    {
        private double balance;

        public BankAccount()
        {

        }
        public BankAccount(double balance)
        {
            this.balance = balance;
        }

        public double Balance { get { return balance; } }

        public void Add(double amount)
        {
            if(amount < 0)
            {
                throw new ArgumentException();
            }
            balance += amount;
        }

        public void Withdraw(double amount)
        {
            if (amount > balance)
            {
                throw new ArgumentException();
            }

            if(amount < 0)
            {
                throw new ArgumentException();
            }

            balance -= amount;
        }

        public void TransferFundsTo(BankAccount otherAccount, double amount)
        {
            if (otherAccount is null)
            {
                throw new ArgumentNullException(nameof(otherAccount));
            }

            Withdraw(amount);
            otherAccount.Add(balance);
        }
    }
}
