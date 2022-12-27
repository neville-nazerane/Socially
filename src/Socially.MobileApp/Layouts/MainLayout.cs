using CommunityToolkit.Maui.Converters;
using Socially.MobileApp.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.MobileApp.Layouts
{
    public class MainLayout : AbsoluteLayout
    {
        const int menuHeight = 120;

        protected override void OnSizeAllocated(double width, double height)
        {

            foreach (var child in Children)
            {
                if (child is TabMenu menu)
                {
                    AbsoluteLayout.SetLayoutBounds(menu, new()
                    {
                        X = 0,
                        Y = height - menuHeight,
                        Width = width,
                        Height = menuHeight
                    });

                    break;
                }
            }

            base.OnSizeAllocated(width, height);
        }

    }
}
