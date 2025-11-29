namespace Bibliotheque.Model
{
    /// <summary>
    /// Représente l’évaluation d’un livre par un utilisateur.
    /// </summary>
    public class Evaluation
    {
        /// <summary>
        /// Email du client qui a évalué le livre.
        /// </summary>
        public string EmailClient { get; set; } = string.Empty;

        /// <summary>
        /// ISBN du livre évalué.
        /// </summary>
        public string IsbnLivre { get; set; } = string.Empty;

        /// <summary>
        /// Note donnée par l’utilisateur (sur 5).
        /// </summary>
        public double Note { get; set; }

        public Evaluation()
        {
        }

        public Evaluation(string emailClient, string isbnLivre, double note)
        {
            EmailClient = emailClient;
            IsbnLivre = isbnLivre;
            Note = note;
        }
    }
}
