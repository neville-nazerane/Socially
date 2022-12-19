

using Socially.Mobile.Logic.ViewModels;

namespace Socially.MobileApp.Pages 
{

    public partial class LoginPage
    {
                    
        public LoginPage(LoginViewModel viewModel)
	    {
		    InitializeComponent();
            BindingContext = viewModel;
        }

    }

}


