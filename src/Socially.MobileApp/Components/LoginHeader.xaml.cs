namespace Socially.MobileApp.Components;

public partial class LoginHeader : AbsoluteLayout
{

	public string Text
	{
		get => lbl.Text; 
		set => lbl.Text = value;
	}

	public LoginHeader()
	{
		InitializeComponent();
	}
}