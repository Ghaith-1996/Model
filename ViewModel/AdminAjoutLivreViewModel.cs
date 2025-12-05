using System;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Bibliotheque.Model;
using Bibliotheque.Services;

namespace Bibliotheque.ViewModels
{
    public class AdminAjoutLivreViewModel : BaseViewModel
    {

        
        private readonly XmlBibliothequeService _xmlService;

        // Propriétés d'ajout de livre
        public string Titre { get => _titre; set => SetProperty(ref _titre, value); }
        public string Auteur { get => _auteur; set => SetProperty(ref _auteur, value); }
        public string ISBN { get => _isbn; set => SetProperty(ref _isbn, value); }
        public string MaisonEdition { get => _maisonEdition; set => SetProperty(ref _maisonEdition, value); }
        public string DatePublication { get => _datePublication; set => SetProperty(ref _datePublication, value); }
        public string Description { get => _description; set => SetProperty(ref _description, value); }

        private string _titre = string.Empty;
        private string _auteur = string.Empty;
        private string _isbn = string.Empty;
        private string _maisonEdition = string.Empty;
        private string _datePublication = string.Empty;
        private string _description = string.Empty;
        private string _message = string.Empty;

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }
        // Commande pour ajouter un livre
        public ICommand AjouterLivreCommand { get; }

        public AdminAjoutLivreViewModel(XmlBibliothequeService xmlService)
        {
            _xmlService = xmlService;
            AjouterLivreCommand = new Command(AjouterLivre);
        }
        // Méthode pour ajouter un livre
        private void AjouterLivre()
        {
            Message = string.Empty;

            if (string.IsNullOrWhiteSpace(Titre) ||
                string.IsNullOrWhiteSpace(Auteur) ||
                string.IsNullOrWhiteSpace(ISBN))
            {
                Message = "Titre, auteur et ISBN sont obligatoires.";
                return;
            }

            var livre = new Livre
            {
                Titre = Titre,
                Auteur = Auteur,
                ISBN = ISBN,
                MaisonEdition = MaisonEdition,
                DatePublication = DatePublication,
                Description = Description,
                MoyenneEvaluation = 0,
                NombreEvaluations = 0
            };

            try
            {
                _xmlService.AjouterLivre(livre);
                Message = "Livre ajouté avec succès.";

                // Réinitialiser le formulaire
                Titre = string.Empty;
                Auteur = string.Empty;
                ISBN = string.Empty;
                MaisonEdition = string.Empty;
                DatePublication = string.Empty;
                Description = string.Empty;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }
    }
}
