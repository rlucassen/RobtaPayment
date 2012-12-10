using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobtaPayment.Model.Entities.Fields;

namespace RobtaPayment.Model.Entities.Answers
{
    public abstract class Answer<T> : ModelBase<T> where T : Answer<T>
    {  
        public abstract string StringValue { get; }

        public abstract bool Valid();
    }
}
