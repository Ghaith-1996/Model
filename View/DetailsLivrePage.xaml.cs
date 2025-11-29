using Bibliotheque.ViewModels;
using Bibliotheque.Model;   // si tu utilises Livre directement
// using ... où se trouve ta classe Session

namespace View;

public partial class DetailsLivrePage : ContentPage
{
    private readonly DetailsLivreViewModel _vm;

    public DetailsLivrePage(DetailsLivreViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = _vm;

        Appearing += (s, e) =>
        {
            if (Session.LivreSelectionne != null)
            {
                _vm.Initialiser(Session.LivreSelectionne);
            }
        };
    }
}
