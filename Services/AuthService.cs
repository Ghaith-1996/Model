using System;
using System.Linq;
using Bibliotheque.Model;
using Bibliotheque.Services;

namespace Bibliotheque.Services
{
    public class AuthService
    {
        private readonly XmlBibliothequeService _xmlService;

        public AuthService(XmlBibliothequeService xmlService)
        {
            _xmlService = xmlService;
        }

        public Compte? Authentifier(string email, string motDePasse)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            email = email.Trim();

            // Cas spécial admin
            if (email.Equals("admin@exemple.com", StringComparison.OrdinalIgnoreCase)
                && motDePasse == "420-3GP")
            {
                return new Compte("admin@exemple.com", "Administrateur", "Principal", estAdministrateur: true)
                {
                    MotDePasse = "420-3GP"
                };
            }

            var comptes = _xmlService.ChargerComptes();

            var compte = comptes.FirstOrDefault(c =>
                c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            if (compte == null)
                return null;

            // Compte sans mot de passe dans le XML → connexion juste avec le courriel
            if (string.IsNullOrWhiteSpace(compte.MotDePasse))
                return compte;

            // Sinon, vérifier le mot de passe
            return compte.MotDePasse == motDePasse ? compte : null;
        }
    }
}
