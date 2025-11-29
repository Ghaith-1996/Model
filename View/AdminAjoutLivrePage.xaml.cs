using Bibliotheque.ViewModels;

namespace View;

public partial class AdminAjoutLivrePage : ContentPage
{
    public AdminAjoutLivrePage(AdminAjoutLivreViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
