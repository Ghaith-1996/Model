namespace Bibliotheque.Model
{
    public class Livre
    {
        public string Titre { get; set; } = string.Empty;
        public string Auteur { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string MaisonEdition { get; set; } = string.Empty;
        public string DatePublication { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // ⬇⬇⬇ setters publics (plus simples pour le XML)
        public double MoyenneEvaluation { get; set; }
        public int NombreEvaluations { get; set; }

        public Livre()
        {
        }

        public Livre(string titre, string auteur, string isbn)
        {
            Titre = titre;
            Auteur = auteur;
            ISBN = isbn;
        }

        public void AjouterEvaluation(double nouvelleNote)
        {
            if (nouvelleNote < 0 || nouvelleNote > 5)
                throw new ArgumentOutOfRangeException(nameof(nouvelleNote));

            double somme = MoyenneEvaluation * NombreEvaluations;
            somme += nouvelleNote;
            NombreEvaluations++;
            MoyenneEvaluation = somme / NombreEvaluations;
        }

        public void ModifierEvaluation(double ancienneNote, double nouvelleNote)
        {
            // S'il n'y a pas encore d'évaluations, on se comporte comme un ajout
            if (NombreEvaluations <= 0)
            {
                AjouterEvaluation(nouvelleNote);
                return;
            }

            if (nouvelleNote < 0 || nouvelleNote > 5)
                throw new ArgumentOutOfRangeException(nameof(nouvelleNote));

            double somme = MoyenneEvaluation * NombreEvaluations;
            somme = somme - ancienneNote + nouvelleNote;
            MoyenneEvaluation = somme / NombreEvaluations;
        }


        public bool EstFavori() => MoyenneEvaluation >= 4.5;
    }
}
