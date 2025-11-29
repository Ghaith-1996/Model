namespace Bibliotheque.Model
{
    /// <summary>
    /// Représente un livre favori pour un utilisateur.
    /// </summary>
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
