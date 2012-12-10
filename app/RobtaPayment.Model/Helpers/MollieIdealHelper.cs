using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobtaPayment.Model.Helpers
{
    using Castle.ActiveRecord;
    using Entities;
    using Mollie.iDEAL;

    public static class MollieIdealHelper
    {
        public static IList<Bank> GetIdealBanks(string mollieClientNumber, bool testMode)
        {
            return new IdealBanks(mollieClientNumber, testMode).Banks;
        }

        public static string GetPaymentUrl(Transaction idealPayer, string mollieClientNumber, bool testMode, string paymentName, string reportUrl, string landUrl)
        {
            if(idealPayer.Payed)
                throw new Exception("Er is al betaald");

            IdealFetch idealFetch = new IdealFetch(mollieClientNumber, testMode, paymentName, reportUrl, landUrl, idealPayer.BankId, idealPayer.Amount);

            if (idealFetch.Error)
            {
                throw new Exception(idealFetch.ErrorMessage);
            }

            idealPayer.TransactionId = idealFetch.TransactionId;
            using( TransactionScope transactionScope = new TransactionScope(TransactionMode.Inherits, OnDispose.Rollback))
            {
                idealPayer.Save();
                transactionScope.VoteCommit();
            }

            return idealFetch.Url;
        }

        public static Transaction GetTransactionByTransactionId(string transactionId)
        {
            IList<Transaction> transactions = Transaction.FindAllByProperty("TransactionId", transactionId);
            if (transactions.Count == 0)
            {
                return null;
            }
            return transactions[0];
        }

        public static bool IsTransactionPayed(string transactionId, string mollieClientNumber, bool testMode)
        {
            IdealCheck idealCheck = new IdealCheck(mollieClientNumber, testMode, transactionId);
            return idealCheck.Payed;
        }
    }
}
