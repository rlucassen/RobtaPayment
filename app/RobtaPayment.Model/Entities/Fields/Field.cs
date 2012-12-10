using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;

namespace RobtaPayment.Model.Entities.Fields
{
    [ActiveRecord]
    public abstract class Field<T> : ModelBase<T> where T : Field<T>
    {
        public Field()
        {
            Page = 1;
            IncludeAutoInfo = true;
        }
            
        [Property]
        public virtual string Name { get; set; }

        [BelongsTo]
        public virtual Project Project { get; set; }

        [Property]
        public virtual int Page { get; set; }

        [Property]
        public virtual string Info { get; set; }

        [Property]
        public virtual bool IncludeAutoInfo { get; set; }

        [Property]
        public virtual int RankOrder { get; set; }

        public abstract string FieldHtml { get; }

        public abstract string AutoInfo { get; }
    }
}
