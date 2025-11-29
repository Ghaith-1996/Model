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

        private double _noteUtilisateur;
        public double NoteUtilisateur
        {
            get => _noteUtilisateur;
            set => SetProperty(ref _noteUtilisateur, value);
        }

        private string _message = string.Empty;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public ICommand SauvegarderEvaluationCommand { get; }

        public ListeLivresViewModel(XmlBibliothequeService xmlService)
        {
            _xmlService = xmlService;

            // Charger tous les livres au démarrage
            var livres = _xmlService.ChargerLivres();
            foreach (var l in livres)
                Livres.Add(l);

            SauvegarderEvaluationCommand = new Command(SauvegarderEvaluation);
        }

        /// <summary>
        /// Appelé quand l’utilisateur clique sur "Sélectionner" ou "Évaluer".
        /// </summary>
        public void SelectionnerLivre(Livre livre)
        {
            LivreSelectionne = livre;

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

        /// <summary>
        /// Enregistre ou modifie l’évaluation de l’utilisateur pour le livre sélectionné.
        /// </summary>
        private void SauvegarderEvaluation()
        {
            if (LivreSelectionne == null)
            {
                Message = "Veuillez d'abord sélectionner un livre.";
                return;
            }

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

            // 1. Enregistrer / modifier l’évaluation dans le XML
            _xmlService.EnregistrerEvaluationPourLivre(email, LivreSelectionne.ISBN, NoteUtilisateur);

            // 2. Recharger le livre depuis le XML pour mettre à jour la moyenne & le nombre d’évaluations
            var tousLesLivres = _xmlService.ChargerLivres();
            var livreMaj = tousLesLivres.FirstOrDefault(l => l.ISBN == LivreSelectionne.ISBN);
            if (livreMaj != null)
            {
                // Remplacer l’ancienne entrée dans la collection affichée
                int index = Livres.IndexOf(LivreSelectionne);
                if (index >= 0)
                    Livres[index] = livreMaj;

                LivreSelectionne = livreMaj;
            }

            Message = "Évaluation enregistrée.";
        }
    }
}
