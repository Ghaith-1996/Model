using System.Collections.ObjectModel;
using Bibliotheque.Model;
using Bibliotheque.Services;

namespace Bibliotheque.ViewModels
{
    //voir les utilisateurs dans admin
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
                Comptes.Add(compte);
            }
        }
    }
}
