

using Socially.Mobile.Logic.ViewModels;

namespace Socially.MobileApp.Pages 
{

    public partial class InitialPage
    {

        public InitialViewModel ViewModel { get; }
                    
        public InitialPage(InitialViewModel viewModel)
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


