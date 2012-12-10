using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MonoRail.ActiveRecordSupport;
using Castle.MonoRail.Framework;
using RobtaPayment.Model.Entities.Fields;

namespace RobtaPayment.Web.controllers.admin
{
    [ControllerDetails(Area = "Admin")]
    public class StringFieldController : SecureController
    {
        public void New()
        {
            PropertyBag.Add("item", new StringField());
        }

        public void Preview([ARFetch("id")] StringField stringField)
        {
            RenderText(stringField.FieldHtml);
        }
    }
}