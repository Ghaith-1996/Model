using System.Collections.ObjectModel;
using System.Linq;
using Bibliotheque.Model;
using Bibliotheque.Services;

namespace Bibliotheque.ViewModels
{
    public class FavorisViewModel : BaseViewModel
    {
        private readonly XmlBibliothequeService _xmlService;
        private readonly FavorisService _favorisService;

        public ObservableCollection<Livre> Favoris { get; } = new();

        public FavorisViewModel(
            XmlBibliothequeService xmlService,
            FavorisService favorisService)
        {
            _xmlService = xmlService;
            _favorisService = favorisService;

            ChargerFavoris();
        }

        private void ChargerFavoris()
        {
            Favoris.Clear();

            var email = Session.CompteCourant?.Email;
            if (string.IsNullOrWhiteSpace(email))
                return;

            var isbnsFav = _favorisService.ChargerIsbnsFavorisPourClient(email);
            var tousLesLivres = _xmlService.ChargerLivres();

            foreach (var isbn in isbnsFav)
            {
                var livre = tousLesLivres.FirstOrDefault(l => l.ISBN == isbn);
                if (livre != null)
                    Favoris.Add(livre);
            }
        }
    }
}
