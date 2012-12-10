using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobtaPayment.Model.Interfaces
{
    using System.Security.Principal;

    public interface IUser : IPrincipal, IIdentity
    {
        string Salt { get; }
        string Password { get; }
        string Name { get; }
    }
}
