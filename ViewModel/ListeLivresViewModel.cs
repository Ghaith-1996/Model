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

        // On garde juste la référence, pas besoin de toute la logique de note ici désormais
        private Livre? _livreSelectionne;
        public Livre? LivreSelectionne
        {
            get => _livreSelectionne;
            set => SetProperty(ref _livreSelectionne, value);
        }

        public ListeLivresViewModel(XmlBibliothequeService xmlService)
        {
            _xmlService = xmlService;
            ChargerLivres();
        }

        public void ChargerLivres()
        {
            Livres.Clear();
            var livres = _xmlService.ChargerLivres();
            foreach (var l in livres)
                Livres.Add(l);
        }

        /// <summary>
        /// Appelé quand on clique sur un livre dans la liste.
        /// Met à jour la Session globale pour que la page suivante sache quel livre afficher.
        /// </summary>
        public void SelectionnerLivrePourNavigation(Livre livre)
        {
            LivreSelectionne = livre;

            // C'est ici le point clé : on stocke le livre dans la Session
            Session.LivreSelectionne = livre;
        }
    }
}