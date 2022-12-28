
using Socially.Mobile.Logic.ComponentModels;
using Socially.MobileApp.Utils;

namespace Socially.MobileApp.Components;

public partial class PostDisplay 
{

    private PostDisplayComponentModel _componentModel;
    public PostDisplayComponentModel ComponentModel
    {
        get
        {
            if (_componentModel is null)
            {
                _componentModel = ServicesUtil.Get<PostDisplayComponentModel>();
                BindingContext = _componentModel;
            }
            return _componentModel;
        }
    }
        
        
}

