using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Bibliotheque.Services
{
    public class FavorisService
    {
        private readonly string _cheminFichier;

        public FavorisService(string cheminFichier)
        {
            _cheminFichier = cheminFichier;
            
            if (!File.Exists(_cheminFichier))
            {
                CreerFichierInitial();
            }
        }

       
        public List<string> ChargerIsbnsFavorisPourClient(string emailClient)
        {
            var isbns = new List<string>();
            var doc = ChargerDocument();

            if (doc.DocumentElement == null) return isbns;

            foreach (XmlElement noeud in doc.DocumentElement.GetElementsByTagName("Favori"))
            {
                string email = noeud["EmailClient"]?.InnerText ?? "";

                // On ne garde que les favoris de l'utilisateur connecté
                if (email.Equals(emailClient, StringComparison.OrdinalIgnoreCase))
                {
                    string isbn = noeud["IsbnLivre"]?.InnerText ?? "";
                    if (!string.IsNullOrWhiteSpace(isbn))
                    {
                        isbns.Add(isbn);
                    }
                }
            }
            return isbns;
        }

        //supprimer si note change
        public void AjouterOuMettreAJourFavori(string emailClient, string isbnLivre, bool estFavori)
        {
            var doc = ChargerDocument();
            var racine = doc.DocumentElement;
            if (racine == null) return;

            // Chercher si le favori existe déjà
            XmlElement? favoriTrouve = null;
            foreach (XmlElement noeud in racine.GetElementsByTagName("Favori"))
            {
                string email = noeud["EmailClient"]?.InnerText ?? "";
                string isbn = noeud["IsbnLivre"]?.InnerText ?? "";

                if (email.Equals(emailClient, StringComparison.OrdinalIgnoreCase) &&
                    isbn.Equals(isbnLivre, StringComparison.OrdinalIgnoreCase))
                {
                    favoriTrouve = noeud;
                    break;
                }
            }

          
            if (estFavori)
            {
                // on l'ajoute s'il n'existe pas déjà
                if (favoriTrouve == null)
                {
                    XmlElement nouveau = doc.CreateElement("Favori");

                    XmlElement elEmail = doc.CreateElement("EmailClient");
                    elEmail.InnerText = emailClient;
                    nouveau.AppendChild(elEmail);

                    XmlElement elIsbn = doc.CreateElement("IsbnLivre");
                    elIsbn.InnerText = isbnLivre;
                    nouveau.AppendChild(elIsbn);

                    racine.AppendChild(nouveau);
                }
            }
            else
            {
                //on le supprime si il existe
                if (favoriTrouve != null)
                {
                    racine.RemoveChild(favoriTrouve);
                }
            }

            doc.Save(_cheminFichier);
        }

        private XmlDocument ChargerDocument()
        {
            var doc = new XmlDocument();
            try
            {
                doc.Load(_cheminFichier);
                if (doc.DocumentElement?.Name != "Favoris")
                    return CreerFichierInitial();
            }
            catch
            {
                return CreerFichierInitial();
            }
            return doc;
        }

        private XmlDocument CreerFichierInitial()
        {
            var doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", null));
            doc.AppendChild(doc.CreateElement("Favoris"));
            doc.Save(_cheminFichier);
            return doc;
        }
    }
}