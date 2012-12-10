using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;

namespace RobtaPayment.Model.Entities.Fields
{
    [ActiveRecord]
    public abstract class Field : ModelBase<Field>
    {
        [Property]
        public virtual string Name { get; set; }

        [BelongsTo]
        public virtual Project Project { get; set; }

        public abstract string StringValue { get; }

        public abstract string FieldHtml { get; }

        public abstract bool IsValid();
    }
}
