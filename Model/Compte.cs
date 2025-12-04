namespace Bibliotheque.Model
{ 
    public class Compte
    {
        public string Email { get; set; } = string.Empty;
        public string MotDePasse { get; set; } = string.Empty; // vide pour certains clients
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;

      
        // Indique si ce compte est un administrateur.
      
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
