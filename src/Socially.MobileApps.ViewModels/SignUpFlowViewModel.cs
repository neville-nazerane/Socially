using Socially.MobileApps.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Text;

namespace Socially.MobileApps.ViewModels
{
    public class SignUpFlowViewModel
    {

        public ObservableCollection<SignUpInputContext> Inputs { get; set; }

        public SignUpFlowViewModel()
        {
            Inputs = new ObservableCollection<SignUpInputContext>();
        }



    }
}
