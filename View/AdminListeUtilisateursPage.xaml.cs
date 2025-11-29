using Bibliotheque.ViewModels;

namespace View;

public partial class AdminListeUtilisateursPage : ContentPage
{
    public AdminListeUtilisateursPage(AdminListeUtilisateursViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
