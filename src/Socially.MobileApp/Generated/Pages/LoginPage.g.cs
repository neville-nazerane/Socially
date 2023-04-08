

using Socially.Mobile.Logic.ViewModels;

namespace Socially.MobileApp.Pages 
{

    public partial class LoginPage
    {

        public LoginViewModel ViewModel { get; }
                    
        public LoginPage(LoginViewModel viewModel)
	    {
		    InitializeComponent();
            BindingContext = viewModel;
            ViewModel = viewModel;
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            await ViewModel.OnNavigatedAsync();
            base.OnNavigatedTo(args);
        }

        protected override async void OnNavigatedFrom(NavigatedFromEventArgs args)
        {
            await ViewModel.OnNavigatedFromAsync();
            base.OnNavigatedFrom(args);
        }

    }


}


