using System;
using System.Collections.Generic;
using System.IO;
using System.Linq; 
using System.Xml;   

namespace Bibliotheque.Services
{
    public class FavorisService
    {
        private readonly string _cheminFichier;

        public FavorisService(string cheminFichier)
        {
            _cheminFichier = cheminFichier;
        }

        
        private XmlDocument ChargerOuCreerDocument()
        {
            var doc = new XmlDocument();

            if (File.Exists(_cheminFichier))
            {
                try
                {
                    doc.Load(_cheminFichier);
                    // Vérifier la racine, sinon on recrée
                    if (doc.DocumentElement == null || doc.DocumentElement.Name != "Favori")
                    {
                        return CreerNouveauDocument();
                    }
                    return doc;
                }
                catch
                {
                    
                    return CreerNouveauDocument();
                }
            }

            return CreerNouveauDocument();
        }

        private XmlDocument CreerNouveauDocument()
        {
            var doc = new XmlDocument();
            
            XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(declaration);

          
            XmlElement racine = doc.CreateElement("Favoris");
            doc.AppendChild(racine);

            doc.Save(_cheminFichier);
            return doc;
        }

        public void AjouterOuMettreAJourFavori(string emailClient, string isbnLivre, bool estFavori)
        {
            var doc = ChargerOuCreerDocument();
            var racine = doc.DocumentElement;
            if (racine == null) return;

            // On cherche si le favori existe déjà
            // Avec XmlDocument, on doit itérer manuellement ou utiliser XPath
            XmlElement? favoriExistant = null;

            foreach (XmlElement noeud in racine.GetElementsByTagName("Favoris"))
            {
                string? email = noeud["EmailClient"]?.InnerText;
                string? isbn = noeud["IsbnLivre"]?.InnerText;

                if (string.Equals(email, emailClient, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(isbn, isbnLivre, StringComparison.OrdinalIgnoreCase))
                {
                    favoriExistant = noeud;
                    break;
                }
            }

            if (estFavori)
            {
                // On veut ajouter, seulement si ça n'existe pas déjà
                if (favoriExistant == null)
                {
                    XmlElement nouveauFavori = doc.CreateElement("Favori");

                    XmlElement elEmail = doc.CreateElement("EmailClient");
                    elEmail.InnerText = emailClient;
                    nouveauFavori.AppendChild(elEmail);

                    XmlElement elIsbn = doc.CreateElement("IsbnLivre");
                    elIsbn.InnerText = isbnLivre;
                    nouveauFavori.AppendChild(elIsbn);

                    racine.AppendChild(nouveauFavori);
                }
            }
            else
            {
                // On veut supprimer
                if (favoriExistant != null)
                {
                    racine.RemoveChild(favoriExistant);
                }
            }

            doc.Save(_cheminFichier);
        }

        //isbn du livre favori
        public List<string> ChargerIsbnsFavorisPourClient(string emailClient)
        {
            var resultats = new List<string>();
            var doc = ChargerOuCreerDocument();
            var racine = doc.DocumentElement;
            if (racine == null) return resultats;

            foreach (XmlElement noeud in racine.GetElementsByTagName("Favori"))
            {
                string? email = noeud["EmailClient"]?.InnerText;

                if (string.Equals(email, emailClient, StringComparison.OrdinalIgnoreCase))
                {
                    string? isbn = noeud["IsbnLivre"]?.InnerText;
                    if (!string.IsNullOrWhiteSpace(isbn))
                    {
                        resultats.Add(isbn);
                    }
                }
            }

            return resultats;
        }
    }
}