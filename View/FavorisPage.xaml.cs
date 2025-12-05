using Bibliotheque.ViewModels;

namespace View;

public partial class FavorisPage : ContentPage
{
    private readonly FavorisViewModel _vm;

    public FavorisPage(FavorisViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // On recharge la liste chaque fois qu'on affiche la page
        // au cas où une note a changé ailleurs.
        _vm.ChargerFavoris();
    }
}