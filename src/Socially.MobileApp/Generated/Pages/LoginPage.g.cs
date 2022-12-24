

using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Socially.Mobile.Logic.ViewModels;

namespace Socially.MobileApp.Pages 
{

    public partial class LoginPage
    {

        public LoginViewModel ViewModel { get; }
                    
        public LoginPage(LoginViewModel viewModel)
	    {
		    InitializeComponent();

            EntryHandler.Mapper.AppendToMapping("entityCustomization", (handler, view) =>
            {
                var border = new RoundRectangle()
                {
                    CornerRadius = 5,

                };
                handler.PlatformView.UpdateBorder(border);
                //handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
                //handler.PlatformView.GettingFocus += PlatformView_GettingFocus;// you can change Thickness in PlatformView_GettingFocus method
                //handler.PlatformView.LosingFocus += PlatformView_LosingFocus;
            });
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


