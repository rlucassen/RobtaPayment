using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;

namespace RobtaPayment.Model.Entities.Fields
{
    [ActiveRecord]
    public class StringField : Field
    {
        [Property]
        public virtual string Value { get; set; }

        public override string StringValue { get { return Value; } }

        public override string FieldHtml
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                return sb.ToString();
            }
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
