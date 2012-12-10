using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using RobtaPayment.Model.Entities.Fields;

namespace RobtaPayment.Model.Entities.Answers
{
    [ActiveRecord]
    public class StringAnswer : Answer<StringAnswer>
    {
        [BelongsTo]
        public virtual StringField Field { get; set; }

        [Property]
        public virtual string Value { get; set; }

        public override string StringValue
        {
            get { return Value; }
        }

        public override bool Valid()
        {
            return true;
        }
    }
}
