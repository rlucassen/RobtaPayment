namespace RobtaPayment.Model.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Castle.ActiveRecord;
    using Castle.Components.Validator;

    [ActiveRecord]
    public class LockerBlock : ModelBase<LockerBlock>
    {
        private string name;
        private bool horizontalCounting;

        private Building building;
        private IList<Locker> lockers = new List<Locker>();

        [Property, ValidateNonEmpty]
        public virtual string Name
        {
            get { return name; }
            set { name = value; }
        }

        [Property]
        public virtual bool HorizontalCounting
        {
            get { return horizontalCounting; }
            set { horizontalCounting = value; }
        }

        [BelongsTo, ValidateNonEmpty]
        public virtual Building Building
        {
            get { return building; }
            set { building = value; }
        }

        [HasMany(Lazy = true)]
        public virtual IList<Locker> Lockers
        {
            get { return lockers; }
            set { lockers = value; }
        }

        public virtual IList<Locker> LockerRow(int row)
        {
            if (horizontalCounting)
            {
                var offset = Lockers.OrderBy(l => l.Number).First().Number - 1;
                var lockersPerRow = Lockers.Count/5;
                return Lockers.Where(l => l.Number <= (row*lockersPerRow)+offset && l.Number > ((row-1)*lockersPerRow)+offset).ToList();
            }
            return Lockers.Where(l => l.Number%5 == row%5).ToList();
        }
    }
}