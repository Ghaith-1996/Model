using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Bibliotheque.Services
{
    public class FavorisService
    {
        private readonly string _cheminFichier;

        public FavorisService(string cheminFichier)
        {
            _cheminFichier = cheminFichier;
        }

        private XDocument ChargerOuCreerDocument()
        {
            if (File.Exists(_cheminFichier))
                return XDocument.Load(_cheminFichier);

            var doc = new XDocument(new XElement("Favoris"));
            doc.Save(_cheminFichier);
            return doc;
        }

        /// <summary>
        /// Ajoute ou supprime un favori pour un client.
        /// estFavori = true → ajouter ; false → retirer.
        /// </summary>
        public void AjouterOuMettreAJourFavori(string emailClient, string isbnLivre, bool estFavori)
        {
            var doc = ChargerOuCreerDocument();
            var racine = doc.Root!;

            var existant = racine.Elements("Favori")
                .FirstOrDefault(e =>
                    string.Equals((string?)e.Element("EmailClient"), emailClient,
                        StringComparison.OrdinalIgnoreCase) &&
                    string.Equals((string?)e.Element("IsbnLivre"), isbnLivre,
                        StringComparison.OrdinalIgnoreCase));

            if (estFavori)
            {
                if (existant == null)
                {
                    racine.Add(new XElement("Favori",
                        new XElement("EmailClient", emailClient),
                        new XElement("IsbnLivre", isbnLivre)));
                }
            }
            else
            {
                existant?.Remove();
            }

            doc.Save(_cheminFichier);
        }

        /// <summary>
        /// Retourne les ISBN favoris d'un client.
        /// </summary>
        public List<string> ChargerIsbnsFavorisPourClient(string emailClient)
        {
            var doc = ChargerOuCreerDocument();
            var racine = doc.Root!;

            return racine.Elements("Favori")
                .Where(e =>
                    string.Equals((string?)e.Element("EmailClient"), emailClient,
                        StringComparison.OrdinalIgnoreCase))
                .Select(e => (string?)e.Element("IsbnLivre") ?? string.Empty)
                .Where(isbn => !string.IsNullOrWhiteSpace(isbn))
                .ToList();
        }
    }
}
