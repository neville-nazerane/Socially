
using Socially.Mobile.Logic.ComponentModels;
using Socially.MobileApp.Utils;

namespace Socially.MobileApp.Components;

public partial class CommentDisplay 
{

    private CommentDisplayComponentModel _componentModel;
    public CommentDisplayComponentModel ComponentModel => _componentModel ??= (CommentDisplayComponentModel)(BindingContext = ServicesUtil.Get<CommentDisplayComponentModel>());

}

