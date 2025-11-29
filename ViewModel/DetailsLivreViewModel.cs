using System;
using System.Linq;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Bibliotheque.Model;
using Bibliotheque.Services;

namespace Bibliotheque.ViewModels
{
    public class DetailsLivreViewModel : BaseViewModel
    {
        private readonly XmlBibliothequeService _xmlService;
        private readonly FavorisService _favorisService;

        private Livre? _livre;
        private double _noteUtilisateur;
        private string _message = string.Empty;
        private int _nombreEvaluations;
        private double _moyenne;

        public Livre? Livre
        {
            get => _livre;
            set => SetProperty(ref _livre, value);
        }

        public double NoteUtilisateur
        {
            get => _noteUtilisateur;
            set => SetProperty(ref _noteUtilisateur, value);
        }

        public int NombreEvaluations
        {
            get => _nombreEvaluations;
            set => SetProperty(ref _nombreEvaluations, value);
        }

        public double Moyenne
        {
            get => _moyenne;
            set => SetProperty(ref _moyenne, value);
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public ICommand SauvegarderEvaluationCommand { get; }

        public DetailsLivreViewModel(
            XmlBibliothequeService xmlService,
            FavorisService favorisService)
        {
            _xmlService = xmlService;
            _favorisService = favorisService;

            SauvegarderEvaluationCommand = new Command(SauvegarderEvaluation);
        }

        /// <summary>
        /// Appelé par la page quand on arrive sur les détails.
        /// </summary>
        public void Initialiser(Livre livre)
        {
            Livre = livre;
            Moyenne = livre.MoyenneEvaluation;
            NombreEvaluations = livre.NombreEvaluations;

            var email = Session.CompteCourant?.Email;

            if (!string.IsNullOrWhiteSpace(email))
            {
                var noteExistante = _xmlService.ObtenirNoteUtilisateurPourLivre(email, livre.ISBN);
                NoteUtilisateur = noteExistante ?? 0;
            }
            else
            {
                NoteUtilisateur = 0;
            }

            Message = string.Empty;
        }

        private void SauvegarderEvaluation()
        {
            if (Livre == null)
                return;

            var email = Session.CompteCourant?.Email;
            if (string.IsNullOrWhiteSpace(email))
            {
                Message = "Vous devez être connecté pour évaluer.";
                return;
            }

            if (NoteUtilisateur < 0 || NoteUtilisateur > 5)
            {
                Message = "La note doit être entre 0 et 5.";
                return;
            }

            // 1. Enregistrer / modifier l'évaluation dans le XML principal
            _xmlService.EnregistrerEvaluationPourLivre(email, Livre.ISBN, NoteUtilisateur);

            // 2. Recharger le livre mis à jour pour récupérer Moyenne + N
            var livres = _xmlService.ChargerLivres();
            var livreMaj = livres.First(l => l.ISBN == Livre.ISBN);
            Livre = livreMaj;

            Moyenne = Livre.MoyenneEvaluation;
            NombreEvaluations = Livre.NombreEvaluations;

            // 3. Gérer les favoris automatiquement (moyenne >= 4)
            bool estFavori = Livre.MoyenneEvaluation >= 4.0;
            _favorisService.AjouterOuMettreAJourFavori(email, Livre.ISBN, estFavori);

            Message = "Évaluation enregistrée.";
        }
    }
}
