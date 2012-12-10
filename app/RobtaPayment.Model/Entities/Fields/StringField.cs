using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Castle.ActiveRecord;

namespace RobtaPayment.Model.Entities.Fields
{
    [ActiveRecord]
    public class StringField : Field<StringField>
    {
        [Property]
        public string Regex { get; set; }
        
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
                if(string.IsNullOrEmpty(Regex))
                {
                    return string.Empty;
                } else
                {
                    return string.Format("Moet voldoen aan de volgende regex: {0}", HttpUtility.HtmlEncode(Regex));
                }
            }
        }
    }
}
