using Bibliotheque.Model;
using Bibliotheque.ViewModels;

namespace View;

public partial class ListeLivresPage : ContentPage
{
    private ListeLivresViewModel Vm => (ListeLivresViewModel)BindingContext;

    public ListeLivresPage(ListeLivresViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    // Bouton "Sélectionner" : affiche les détails du livre en bas
    private void OnSelectionnerClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is Livre livre)
        {
            Vm.SelectionnerLivre(livre);
        }
    }

    // Bouton "Évaluer" : sélectionne le livre + prépare la zone d’évaluation
    private void OnEvaluerClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is Livre livre)
        {
            Vm.SelectionnerLivre(livre);
        }
    }
}
