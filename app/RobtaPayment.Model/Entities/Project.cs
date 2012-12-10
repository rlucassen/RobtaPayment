using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using RobtaPayment.Model.Entities.Fields;

namespace RobtaPayment.Model.Entities
{
    [ActiveRecord]
    public class Project : ModelBase<Project>
    {
        [Property]
        public virtual string Name { get; set; }

        [HasMany]
        public virtual IList<Field> Fields { get; set; } 
    }
}
