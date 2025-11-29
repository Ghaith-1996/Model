using System.Collections.ObjectModel;
using Bibliotheque.Model;
using Bibliotheque.Services;

namespace Bibliotheque.ViewModels
{
    public class AdminListeUtilisateursViewModel : BaseViewModel
    {
        private readonly XmlBibliothequeService _xmlService;

        public ObservableCollection<Compte> Comptes { get; } = new();

        public AdminListeUtilisateursViewModel(XmlBibliothequeService xmlService)
        {
            _xmlService = xmlService;
            ChargerComptes();
        }

        private void ChargerComptes()
        {
            Comptes.Clear();

            var comptes = _xmlService.ChargerComptes();
            foreach (var compte in comptes)
            {
                // On peut filtrer si on veut exclure l’admin
                Comptes.Add(compte);
            }
        }
    }
}
