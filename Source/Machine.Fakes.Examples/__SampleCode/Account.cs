using System;

namespace Machine.Fakes.Examples.SampleCode
{
    public class Account
    {
        public Account(decimal balance)
        {
            Balance = balance;
        }

        public decimal Balance { get; private set; }

        public void Transfer(decimal amount, Account toAccount)
        {
            if (amount > Balance)
            {
                throw new ArgumentException(string.Format("Cannot transfer ${0}. The available balance is ${1}.", amount, Balance));
            }

            Balance -= amount;
            toAccount.Balance += amount;
        }
    }
}