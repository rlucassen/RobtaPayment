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
        public Project()
        {
            Fields = new List<Field>();
            StaticPrice = true;
        }

        [Property]
        public virtual string Name { get; set; }

        [Property(SqlType = "ntext")]
        public virtual string Description { get; set; }

        [Property]
        public virtual bool StaticPrice { get; set; }

        [Property]
        public virtual decimal Price { get; set; }

        [HasMany]
        public virtual IList<Field> Fields { get; set; } 
    }
}
