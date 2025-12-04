using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Bibliotheque.Model
{
    
    public class Livre : INotifyPropertyChanged
    {
      
        private double _moyenneEvaluation;
        private int _nombreEvaluations;

        // Propriétés standard (ne changent pas souvent)
        public string Titre { get; set; } = string.Empty;
        public string Auteur { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string MaisonEdition { get; set; } = string.Empty;
        public string DatePublication { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Propriété avec notification : MoyenneEvaluation
        public double MoyenneEvaluation
        {
            get => _moyenneEvaluation;
            set
            {
                if (_moyenneEvaluation != value)
                {
                    _moyenneEvaluation = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(EstFavori)); // status favori 
                }
            }
        }

      
        public int NombreEvaluations
        {
            get => _nombreEvaluations;
            set
            {
                if (_nombreEvaluations != value)
                {
                    _nombreEvaluations = value;
                    OnPropertyChanged();
                }
            }
        }

        public Livre()
       {
        }

        public Livre(string titre, string auteur, string isbn)
        {
            Titre = titre;
            Auteur = auteur;
            ISBN = isbn;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        //déclencher l'événement
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

      
        
        public void AjouterEvaluation(double nouvelleNote)
        {
            if (nouvelleNote < 0 || nouvelleNote > 5)
                throw new ArgumentOutOfRangeException(nameof(nouvelleNote));

            double somme = (MoyenneEvaluation * NombreEvaluations) + nouvelleNote;

            // On met à jour via les propriétés 
            NombreEvaluations++;
            MoyenneEvaluation = somme / NombreEvaluations;
        }
        
        public void ModifierEvaluation(double ancienneNote, double nouvelleNote)
        {
            if (NombreEvaluations <= 0)
            {
                AjouterEvaluation(nouvelleNote);
                return;
            }

            if (nouvelleNote < 0 || nouvelleNote > 5)
                throw new ArgumentOutOfRangeException(nameof(nouvelleNote));

            double somme = (MoyenneEvaluation * NombreEvaluations) - ancienneNote + nouvelleNote;

            MoyenneEvaluation = somme / NombreEvaluations;
        }

        // Retourne vrai si la note est >= 4.0
        // Cette méthode est maintenant liée dynamiquement via OnPropertyChanged dans le setter de MoyenneEvaluation
        public bool EstFavori() => MoyenneEvaluation >= 4.0;
    }
}