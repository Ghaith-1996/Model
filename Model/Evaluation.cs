namespace Bibliotheque.Model
{  

    public class Evaluation
    {
      
        // Email du client 
        public string EmailClient { get; set; } = string.Empty;

        // ISBN du livr
        public string IsbnLivre { get; set; } = string.Empty;

        // Note donnée par l’utilisateur.

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
