namespace Lecture9
{
    public class Account
    {
        private decimal balance;
        public decimal Balance
        {
            get { return balance; }
            set
            {
                if (value > 0)
                {
                    balance = value;
                }
            }
        }

        public void TransferAmount(decimal amount, Account receiver)
        {
            if (this.Balance < amount)
                throw new InvalidOperationException("Not enough balance to perform the transfer.");

            this.Balance -= amount;
            receiver.Balance += amount;
        }

    }
}
