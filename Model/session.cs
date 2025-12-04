namespace Bibliotheque.Model
{

    // garder en memoire l'utilisateur connecté et le livre sélectionné.
  
    public static class Session
    {
        public static Compte? CompteCourant { get; set; }

        public static Livre? LivreSelectionne { get; set; }
    }
}
