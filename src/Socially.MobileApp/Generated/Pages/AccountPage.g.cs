

using Socially.Mobile.Logic.ViewModels;

namespace Socially.MobileApp.Pages 
{

    public partial class AccountPage
    {

        public AccountViewModel ViewModel { get; }
                    
        public AccountPage(AccountViewModel viewModel)
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

    }


}


