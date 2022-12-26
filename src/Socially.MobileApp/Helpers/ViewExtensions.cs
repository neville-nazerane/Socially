using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.MobileApp.Helpers
{
    public static class ViewExtensions
    {

        public static void SetStyleClass(this NavigableElement view, string className)
            => view.StyleClass = new List<string> { className };


    }
}
