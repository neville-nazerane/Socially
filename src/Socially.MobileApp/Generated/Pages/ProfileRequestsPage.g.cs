

using Socially.Mobile.Logic.ViewModels;

namespace Socially.MobileApp.Pages 
{

    public partial class ProfileRequestsPage
    {

        public ProfileRequestsViewModel ViewModel { get; }
                    
        public ProfileRequestsPage(ProfileRequestsViewModel viewModel)
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


