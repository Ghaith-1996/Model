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

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Rafraîchir la liste quand on revient sur la page (pour voir les nouvelles moyennes)
        Vm.ChargerLivres();
    }

    // Nouvelle méthode unique pour gérer le clic
    private async void OnVoirDetailsClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is Livre livre)
        {
            // 1. On dit au ViewModel de mettre ce livre en Session
            Vm.SelectionnerLivrePourNavigation(livre);

            // 2. On navigue vers la page DetailsLivrePage
            // Assurez-vous que la route est bien enregistrée dans AppShell.xaml.cs
            await Shell.Current.GoToAsync("DetailsLivrePage");
        }
    }
}