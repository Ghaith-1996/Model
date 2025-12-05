using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Bibliotheque.Model;
using Bibliotheque.Services;

namespace Bibliotheque.ViewModels
{
    public class FavorisViewModel : BaseViewModel
    {
        private readonly XmlBibliothequeService _xmlService;
        private readonly FavorisService _favorisService;

        public ObservableCollection<Livre> Favoris { get; } = new();

        public ICommand RafraichirCommand { get; }

        public FavorisViewModel(XmlBibliothequeService xmlService, FavorisService favorisService)
        {
            _xmlService = xmlService;
            _favorisService = favorisService;

            RafraichirCommand = new Command(ChargerFavoris);
        }

        public void ChargerFavoris()
        {
            Favoris.Clear();

            //Vérifier qu'un utilisateur est connecté
            var email = Session.CompteCourant?.Email;
            if (string.IsNullOrWhiteSpace(email))
                return;

            //Récupérer les ISBN
            var isbnsFavoris = _favorisService.ChargerIsbnsFavorisPourClient(email);

            
            if (!isbnsFavoris.Any())
                return;

            //Charger les détails
            var tousLesLivres = _xmlService.ChargerLivres();

            //On garde les livres dont l'ISBN est dans la liste des favoris
            foreach (var livre in tousLesLivres)
            {
                //re verifier si note >=4
                if (isbnsFavoris.Contains(livre.ISBN))
                {
                    Favoris.Add(livre);
                }
            }
        }
    }
}