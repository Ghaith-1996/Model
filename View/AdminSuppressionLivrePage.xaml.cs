using Bibliotheque.ViewModels;

namespace View;

public partial class AdminSuppressionLivrePage : ContentPage
{
    public AdminSuppressionLivrePage(AdminSuppressionLivreViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
