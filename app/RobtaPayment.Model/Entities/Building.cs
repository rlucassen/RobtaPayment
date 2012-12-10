namespace RobtaPayment.Model.Entities
{
    using System;
    using System.Collections.Generic;
    using Castle.ActiveRecord;
    using Castle.Components.Validator;

    [ActiveRecord]
    public class Building : ModelBase<Building>
    {
        private string name;
        private string slug;

        private IList<LockerBlock> lockerBlocks = new List<LockerBlock>();

        [Property, ValidateNonEmpty]
        public virtual string Name
        {
            get { return name; }
            set { name = value; }
        }

        [Property]
        public virtual string Slug
        {
            get { return slug; }
            set { slug = value; }
        }

        [HasMany(Lazy = true)]
        public virtual IList<LockerBlock> LockerBlocks
        {
            get { return lockerBlocks; }
            set { lockerBlocks = value; }
        }
    }
}