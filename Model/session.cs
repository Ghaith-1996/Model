namespace Bibliotheque.Model
{
    /// <summary>
    /// Classe statique pour garder en mémoire l'utilisateur connecté
    /// (et éventuellement le livre sélectionné).
    /// </summary>
    public static class Session
    {
        public static Compte? CompteCourant { get; set; }

        // Utile si un jour tu veux mémoriser un livre choisi
        public static Livre? LivreSelectionne { get; set; }
    }
}
