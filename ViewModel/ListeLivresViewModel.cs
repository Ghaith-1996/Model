using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Bibliotheque.Model;
using Bibliotheque.Services;

namespace Bibliotheque.ViewModels
{
    public class ListeLivresViewModel : BaseViewModel
    {
        private readonly XmlBibliothequeService _xmlService;

        public ObservableCollection<Livre> Livres { get; } = new();

        private Livre? _livreSelectionne;
        public Livre? LivreSelectionne
        {
            get => _livreSelectionne;
            set => SetProperty(ref _livreSelectionne, value);
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public ICommand ChargerLivresCommand { get; }
        public ICommand AllerAuxFavorisCommand { get; }

        public ListeLivresViewModel(XmlBibliothequeService xmlService)
        {
            _xmlService = xmlService;

            // Commande pour recharger la liste
            ChargerLivresCommand = new Command(() =>
            {
                IsRefreshing = true;
                ChargerLivres();
                IsRefreshing = false;
            });

            // Commande pour aller vers les favoris
            AllerAuxFavorisCommand = new Command(async () =>
            {
                await Shell.Current.GoToAsync("FavorisPage");
            });

            ChargerLivres();
        }

        public void ChargerLivres()
        {
            Livres.Clear();
            var livres = _xmlService.ChargerLivres();
            foreach (var l in livres)
                Livres.Add(l);
        }

        public void SelectionnerLivrePourNavigation(Livre livre)
        {
            LivreSelectionne = livre;
            Session.LivreSelectionne = livre;
        }
    }
}