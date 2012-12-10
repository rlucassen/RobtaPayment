using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;

namespace RobtaPayment.Model.Entities.Fields
{
    [ActiveRecord]
    class IntegerField : Field<IntegerField>
    {
        [Property]
        public virtual int MaxValue { get; set; }

        [Property]
        public virtual int MinValue { get; set; }

        public override string FieldHtml
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                return sb.ToString();
            }
        }

        public override string AutoInfo
        {
            get
            {
                if(MaxValue != 0 && MinValue != 0)
                    return string.Format("De waarde moet tussen {0} en {1} vallen.", MinValue, MaxValue);
                if(MaxValue == 0 && MinValue != 0)
                    return string.Format("De waarde moet boven {0} vallen.", MinValue);
                if(MaxValue != 0 && MinValue == 0)
                    return string.Format("De waarde moet onder {0} vallen.", MaxValue);
                return string.Empty;
            }
        }
    }
}
