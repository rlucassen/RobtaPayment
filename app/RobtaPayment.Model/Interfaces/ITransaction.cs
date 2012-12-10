using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobtaPayment.Model.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITransaction
    {
        bool Payed { get; set; }
        string TransactionId { get; set; }
        string BankId { get; set; }
        decimal Amount { get; set; }
    }
}
