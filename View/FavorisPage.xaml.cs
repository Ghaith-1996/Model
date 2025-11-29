using Bibliotheque.ViewModels;

namespace View;

public partial class FavorisPage : ContentPage
{
    public FavorisPage(FavorisViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
