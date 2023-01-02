
using Socially.Mobile.Logic.ComponentModels;
using Socially.MobileApp.Utils;

namespace Socially.MobileApp.Components;

public partial class ProfileHeader 
{

    private ProfileHeaderComponentModel _componentModel;
    public ProfileHeaderComponentModel ComponentModel => _componentModel ??= (ProfileHeaderComponentModel)(BindingContext = ServicesUtil.Get<ProfileHeaderComponentModel>());

}

