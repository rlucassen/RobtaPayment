namespace RobtaPayment.Model.Entities
{
    #region

    using Castle.ActiveRecord;
    using RobtaPayment.Model.Interfaces;

    #endregion

    [ActiveRecord("[Transaction]")]
    public class Transaction : ModelBase<Transaction>, ITransaction
    {
        [Property]
        public virtual bool Payed { get; set; }

        [Property]
        public virtual string TransactionId { get; set; }

        [Property]
        public virtual string BankId { get; set; }

        [Property]
        public virtual decimal Amount { get; set; }
    }
}