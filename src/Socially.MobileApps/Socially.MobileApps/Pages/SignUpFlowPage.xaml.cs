﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Socially.MobileApps.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpFlowPage : ContentPage
    {
        public SignUpFlowPage()
        {
            InitializeComponent();
        }

        private void CarouselView_ChildAdded(object sender, ElementEventArgs e)
        {

        }
    }
}