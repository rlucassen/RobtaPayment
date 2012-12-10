namespace RobtaPayment.Model.Entities
{
    using System;
    using Castle.ActiveRecord;

    public abstract class ModelBase<T> : ActiveRecordValidationBase<T>, IEquatable<T> where T : ModelBase<T>
    {
        private readonly Guid guid = Guid.NewGuid();
#pragma warning disable 649
        private readonly int id;
#pragma warning restore 649
        private bool isActive = true;

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [PrimaryKey(
            Generator = PrimaryKeyType.Identity,
            UnsavedValue = "0",
            Access = PropertyAccess.NosetterCamelcase)]
        public virtual int Id
        {
            get { return id; }
        }

        /// <summary>
        /// Gets or sets the GUID.
        /// </summary>
        /// <value>The GUID.</value>
        [Property(
            NotNull = true,
            Update = false,
            Access = PropertyAccess.NosetterCamelcase)]
        public virtual Guid Guid
        {
            get { return guid; }
        }

        public virtual bool Equals(T other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Guid.Equals(Guid) && other.Id == Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (ModelBase<T>)) return false;
            return Equals((T) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Guid.GetHashCode()*397) ^ Id;
            }
        }

        public static bool operator ==(ModelBase<T> left, ModelBase<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ModelBase<T> left, ModelBase<T> right)
        {
            return !Equals(left, right);
        }
    }
}