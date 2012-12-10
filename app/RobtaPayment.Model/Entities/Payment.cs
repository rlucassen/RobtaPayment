using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;

namespace RobtaPayment.Model.Entities
{
    [ActiveRecord]
    public class Payment
    {
        [BelongsTo]
        public virtual Project Project { get; set; }

        [BelongsTo("[Transaction]", Cascade = CascadeEnum.Delete)]
        public virtual Transaction Transaction { get; set; }

        public virtual bool Paid
        {
            get
            {
                return Transaction != null && Transaction.Payed;
            }
        }

    }
}
