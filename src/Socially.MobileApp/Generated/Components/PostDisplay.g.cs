
using Socially.Mobile.Logic.ComponentModels;
using Socially.MobileApp.Utils;

namespace Socially.MobileApp.Components;

public partial class PostDisplay 
{

    private PostDisplayComponentModel _componentModel;
    public PostDisplayComponentModel ComponentModel => _componentModel ??= (PostDisplayComponentModel)(BindingContext = ServicesUtil.Get<PostDisplayComponentModel>());

}

