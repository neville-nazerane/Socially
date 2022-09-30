using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace Socially.Server.Managers.Tests.Traits
{
    internal enum ActionEnums { Add, Update, Get, Delete }

    internal class ActionTraitAttribute : Attribute, ITraitAttribute
    {



    }
}
