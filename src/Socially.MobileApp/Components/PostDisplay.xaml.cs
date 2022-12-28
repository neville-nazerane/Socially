using LiteDB;

namespace Socially.MobileApp.Components;

public partial class PostDisplay : Grid
{
	public PostDisplay()
	{
		InitializeComponent();
        using var db = new LiteDatabase(@"C:\Temp\MyData.db");
    }
}