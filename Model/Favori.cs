namespace Bibliotheque.Model
{
    // Représente un livre favori pour un utilisateur.
    public class Favori
    {
        public string EmailClient { get; set; } = string.Empty;
        public string IsbnLivre { get; set; } = string.Empty;

        public Favori()
        {
        }

        public Favori(string emailClient, string isbnLivre)
        {
            EmailClient = emailClient;
            IsbnLivre = isbnLivre;
        }
    }
}
