namespace RobtaPayment.Model.Entities
{
    using Castle.ActiveRecord;
    using Castle.Components.Validator;

    [ActiveRecord]
    public class Education : ModelBase<Education>
    {
        private string name = string.Empty;

        [Property, ValidateNonEmpty]
        public virtual string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}