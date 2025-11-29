using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Bibliotheque.Model;
using Bibliotheque.Services;

namespace Bibliotheque.ViewModels
{
    public class AdminSuppressionLivreViewModel : BaseViewModel
    {
        private readonly XmlBibliothequeService _xmlService;

        private string _isbnRecherche = string.Empty;
        private string _message = string.Empty;
        private Livre? _livreSelectionne;

        public string IsbnRecherche
        {
            get => _isbnRecherche;
            set => SetProperty(ref _isbnRecherche, value);
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public ObservableCollection<Livre> ResultatsRecherche { get; } = new();

        public Livre? LivreSelectionne
        {
            get => _livreSelectionne;
            set => SetProperty(ref _livreSelectionne, value);
        }

        public ICommand RechercherCommand { get; }
        public ICommand SupprimerSelectionCommand { get; }

        public AdminSuppressionLivreViewModel(XmlBibliothequeService xmlService)
        {
            _xmlService = xmlService;

            RechercherCommand = new Command(RechercherLivre);
            SupprimerSelectionCommand = new Command(SupprimerLivreSelectionne);
        }

        private void RechercherLivre()
        {
            Message = string.Empty;
            ResultatsRecherche.Clear();
            LivreSelectionne = null;

            string filtre = IsbnRecherche?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(filtre))
            {
                Message = "Veuillez entrer une partie de l'ISBN.";
                return;
            }

            string filtreNettoye = filtre.Replace("-", "").Replace(" ", "");

            var tousLesLivres = _xmlService.ChargerLivres();

            var trouves = tousLesLivres
                .Where(l =>
                {
                    string isbnLivre = (l.ISBN ?? string.Empty)
                        .Replace("-", "")
                        .Replace(" ", "");
                    return isbnLivre.Contains(filtreNettoye, StringComparison.OrdinalIgnoreCase);
                })
                .ToList();

            if (!trouves.Any())
            {
                Message = "Aucun livre trouvé pour cet ISBN.";
                return;
            }

            foreach (var livre in trouves)
                ResultatsRecherche.Add(livre);

            LivreSelectionne = ResultatsRecherche.FirstOrDefault();
            Message = $"{ResultatsRecherche.Count} livre(s) trouvé(s).";
        }

        private void SupprimerLivreSelectionne()
        {
            Message = string.Empty;

            if (LivreSelectionne == null)
            {
                Message = "Veuillez sélectionner un livre à supprimer.";
                return;
            }

            _xmlService.SupprimerLivreParIsbn(LivreSelectionne.ISBN);

            ResultatsRecherche.Remove(LivreSelectionne);
            LivreSelectionne = null;

            Message = "Livre supprimé avec succès.";
        }
    }
}
