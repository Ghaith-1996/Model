namespace Bibliotheque.Model
{
    /// <summary>
    /// Représente un compte utilisateur (client ou administrateur).
    /// </summary>
    public class Compte
    {
        public string Email { get; set; } = string.Empty;
        public string MotDePasse { get; set; } = string.Empty; // vide pour certains clients
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;

        /// <summary>
        /// Indique si ce compte est un administrateur.
        /// Dans le XML, on pourra décider que l’admin a un mot de passe
        /// et/ou un email spécial (admin@exemple.com).
        /// </summary>
        public bool EstAdministrateur { get; set; }

        public Compte()
        {
        }

        public Compte(string email, string nom, string prenom, bool estAdministrateur = false)
        {
            Email = email;
            Nom = nom;
            Prenom = prenom;
            EstAdministrateur = estAdministrateur;
        }
    }
}
