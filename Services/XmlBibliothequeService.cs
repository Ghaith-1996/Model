using Bibliotheque.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;

namespace Bibliotheque.Services
{
    public class XmlBibliothequeService
    {
        private readonly string _cheminFichier;

        public XmlBibliothequeService(string cheminFichier)
        {
            _cheminFichier = cheminFichier;
            string? dossier = Path.GetDirectoryName(_cheminFichier);
            if (!string.IsNullOrEmpty(dossier) && !Directory.Exists(dossier))
            {
                Directory.CreateDirectory(dossier);
            }
            ChargerDocument(); 
        }
        // gérer les données dans le XML.
        public List<Livre> ChargerLivres()
        {
            var livres = new List<Livre>();
            foreach (XmlElement el in GetElementsDeSection("Livres", "Livre"))
            {
                livres.Add(new Livre
                {
                    Titre = GetTexte(el, "Titre"),
                    Auteur = GetTexte(el, "Auteur"),
                    ISBN = GetTexte(el, "ISBN"),
                    MaisonEdition = GetTexte(el, "MaisonEdition"),
                    DatePublication = GetTexte(el, "DatePublication"),
                    Description = GetTexte(el, "Description"),
                    MoyenneEvaluation = GetDouble(el, "MoyenneEvaluation"),
                    NombreEvaluations = GetInt(el, "NombreEvaluations")
                });
            }
            return livres;
        }
        //Sauvegarde la liste des livres dans le XML.
        public void SauvegarderLivres(List<Livre> livres)
        {
            MettreAJourSection("Livres", (doc, parent) =>
            {
                foreach (var livre in livres)
                {
                    XmlElement el = doc.CreateElement("Livre");
                    AppendElement(doc, el, "Titre", livre.Titre);
                    AppendElement(doc, el, "Auteur", livre.Auteur);
                    AppendElement(doc, el, "ISBN", livre.ISBN);
                    AppendElement(doc, el, "MaisonEdition", livre.MaisonEdition);
                    AppendElement(doc, el, "DatePublication", livre.DatePublication);
                    AppendElement(doc, el, "Description", livre.Description);
                    AppendElement(doc, el, "MoyenneEvaluation", livre.MoyenneEvaluation.ToString(CultureInfo.InvariantCulture));
                    AppendElement(doc, el, "NombreEvaluations", livre.NombreEvaluations.ToString(CultureInfo.InvariantCulture));
                    parent.AppendChild(el);
                }
            });
        }

        public void AjouterLivre(Livre nouveauLivre)
        {
            var livres = ChargerLivres();
            // Vérifier isbn unique
            if (livres.Any(l => l.ISBN == nouveauLivre.ISBN))
                throw new InvalidOperationException("Un livre avec cet ISBN existe déjà.");

            livres.Add(nouveauLivre);
            SauvegarderLivres(livres);
        }

        public void SupprimerLivreParIsbn(string isbn)
        {
            var livres = ChargerLivres();
            SauvegarderLivres(livres.Where(l => !l.ISBN.Equals(isbn, StringComparison.OrdinalIgnoreCase)).ToList());
        }

        //charger la liste
        public List<Compte> ChargerComptes()
        {
            var comptes = new List<Compte>();
            foreach (XmlElement el in GetElementsDeSection("Comptes", "Compte"))
            {
                string email = GetTexte(el, "Email");
                bool estAdmin = email.Equals("admin@exemple.com");

                comptes.Add(new Compte(email, GetTexte(el, "Nom"), GetTexte(el, "Prenom"), estAdmin)
                {
                    MotDePasse = GetTexte(el, "MotDePasse")
                });
            }
            return comptes;
        }

     //Sauvegarde la liste des évaluations dans le XML
        public void SauvegarderComptes(List<Compte> comptes)
        {
            MettreAJourSection("Comptes", (doc, parent) =>
            {
                foreach (var compte in comptes)
                {
                    XmlElement el = doc.CreateElement("Compte");
                    AppendElement(doc, el, "Email", compte.Email);
                    AppendElement(doc, el, "MotDePasse", compte.MotDePasse);
                    AppendElement(doc, el, "Nom", compte.Nom);
                    AppendElement(doc, el, "Prenom", compte.Prenom);
                    parent.AppendChild(el);
                }
            });
        }

        //charger la liste des évaluations
        public List<Evaluation> ChargerEvaluations()
        {
            var evals = new List<Evaluation>();
            foreach (XmlElement el in GetElementsDeSection("Evaluations", "Evaluation"))
            {
                evals.Add(new Evaluation(
                    GetTexte(el, "EmailClient"),
                    GetTexte(el, "IsbnLivre"),
                    GetDouble(el, "Note")
                ));
            }
            return evals;
        }

        //Sauvegarde la liste des évaluations dans le XML
        public void SauvegarderEvaluations(List<Evaluation> evaluations)
        {
            MettreAJourSection("Evaluations", (doc, parent) =>
            {
                foreach (var eval in evaluations)
                {
                    XmlElement el = doc.CreateElement("Evaluation");
                    AppendElement(doc, el, "EmailClient", eval.EmailClient);
                    AppendElement(doc, el, "IsbnLivre", eval.IsbnLivre);
                    AppendElement(doc, el, "Note", eval.Note.ToString(CultureInfo.InvariantCulture));
                    parent.AppendChild(el);
                }
            });
        }

        // la note par email et isbn
        public double? ObtenirNoteUtilisateurPourLivre(string emailClient, string isbnLivre)
        {
            return ChargerEvaluations()
                .FirstOrDefault(e =>
                    e.EmailClient.Equals(emailClient, StringComparison.OrdinalIgnoreCase) &&
                    e.IsbnLivre.Equals(isbnLivre, StringComparison.OrdinalIgnoreCase))?.Note;
        }

        // Enregistre ou met à jour l'évaluation
        public void EnregistrerEvaluationPourLivre(string emailClient, string isbnLivre, double nouvelleNote)
        {
            var evaluations = ChargerEvaluations();
            var evalExistante = evaluations.FirstOrDefault(e =>
                e.EmailClient.Equals(emailClient, StringComparison.OrdinalIgnoreCase) &&
                e.IsbnLivre.Equals(isbnLivre, StringComparison.OrdinalIgnoreCase));

            var livres = ChargerLivres();
            var livre = livres.FirstOrDefault(l => l.ISBN == isbnLivre)
                        ?? throw new InvalidOperationException("Livre introuvable.");

            if (evalExistante == null)
            {
                livre.AjouterEvaluation(nouvelleNote);
                evaluations.Add(new Evaluation(emailClient, isbnLivre, nouvelleNote));
            }
            else
            {
                livre.ModifierEvaluation(evalExistante.Note, nouvelleNote);
                evalExistante.Note = nouvelleNote;
            }

            SauvegarderLivres(livres);
            SauvegarderEvaluations(evaluations);
        }

        /// Récupère la liste
        private IEnumerable<XmlElement> GetElementsDeSection(string nomSection, string nomElementEnfant)
        {
            XmlDocument doc = ChargerDocument();
            XmlElement? racine = doc.DocumentElement;
            if (racine == null) yield break;

            XmlElement? section = racine[nomSection];
            if (section == null) yield break;

            foreach (XmlNode node in section.GetElementsByTagName(nomElementEnfant))
            {
                if (node is XmlElement element)
                    yield return element;
            }
        }

        /// Met à jour 
        private void MettreAJourSection(string nomSection, Action<XmlDocument, XmlElement> actionRemplissage)
        {
            XmlDocument doc = ChargerDocument();
            XmlElement? racine = doc.DocumentElement;
            if (racine == null) return;

            XmlElement? section = racine[nomSection];
            if (section == null)
            {
                section = doc.CreateElement(nomSection);
                racine.AppendChild(section);
            }
            else
            {
                section.RemoveAll();
            }

            actionRemplissage(doc, section);
            doc.Save(_cheminFichier);
        }

        // Récupère le titre, auteur, etc.
        private string GetTexte(XmlElement el, string tag) => el[tag]?.InnerText ?? string.Empty;

        // Récupère la moyenne de l'évaluation
        private double GetDouble(XmlElement el, string tag)
        {
            if (double.TryParse(el[tag]?.InnerText, NumberStyles.Any, CultureInfo.InvariantCulture, out double res))
                return res;
            return 0;
        }

        // Récupère le nombre d'évaluations
        private int GetInt(XmlElement el, string tag)
        {
            if (int.TryParse(el[tag]?.InnerText, out int res))
                return res;
            return 0;
        }
        private static void AppendElement(XmlDocument doc, XmlElement parent, string name, string value)
        {
            XmlElement child = doc.CreateElement(name);
            child.InnerText = value;
            parent.AppendChild(child);
        }

        // / Charge le document XML ou le crée
        private XmlDocument ChargerDocument()
        {
            XmlDocument doc = new XmlDocument();
            if (File.Exists(_cheminFichier))
            {
                try
                {
                    doc.Load(_cheminFichier);
                    if (doc.DocumentElement?.Name == "Bibliotheque")
                        return doc;
                }
                catch {
                    CreerDocumentInitial();
                }
            }
            return CreerDocumentInitial();
        }

        private XmlDocument CreerDocumentInitial()
        {
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", null));

            XmlElement racine = doc.CreateElement("Bibliotheque");
            doc.AppendChild(racine);

            // Création des sections 
            racine.AppendChild(doc.CreateElement("Livres"));
            racine.AppendChild(doc.CreateElement("Comptes"));
            racine.AppendChild(doc.CreateElement("Evaluations"));

            // Données par défaut
            RemplirDonneesInitiales(doc, racine["Livres"]!, racine["Comptes"]!);

            doc.Save(_cheminFichier);
            return doc;
        }

        private void RemplirDonneesInitiales(XmlDocument doc, XmlElement livres, XmlElement comptes)
        {
            // Ajout Livre 1
            XmlElement l1 = doc.CreateElement("Livre");
            AppendElement(doc, l1, "Titre", "Les Fleurs du Mal");
            AppendElement(doc, l1, "Auteur", "Charles Baudelaire");
            AppendElement(doc, l1, "ISBN", "978-2-07-038255-2");
            AppendElement(doc, l1, "MaisonEdition", "Gallimard");
            AppendElement(doc, l1, "DatePublication", "1857-06-25");
            AppendElement(doc, l1, "Description", "Recueil de poèmes majeur de la littérature française.");
            AppendElement(doc, l1, "MoyenneEvaluation", "4.9");
            AppendElement(doc, l1, "NombreEvaluations", "1");
            livres.AppendChild(l1);

            // Ajout Livre 2
            XmlElement l2 = doc.CreateElement("Livre");
            AppendElement(doc, l2, "Titre", "Neuromancien");
            AppendElement(doc, l2, "Auteur", "William Gibson");
            AppendElement(doc, l2, "ISBN", "978-2-07-041573-0");
            AppendElement(doc, l2, "MaisonEdition", "J'ai lu");
            AppendElement(doc, l2, "DatePublication", "1984");
            AppendElement(doc, l2, "Description", "Un roman cyberpunk qui a popularisé le terme \"matrice\".");
            AppendElement(doc, l2, "MoyenneEvaluation", "4.3");
            AppendElement(doc, l2, "NombreEvaluations", "1");
            livres.AppendChild(l2);

            // Ajout Admin
            XmlElement admin = doc.CreateElement("Compte");
            AppendElement(doc, admin, "Email", "admin@exemple.com");
            AppendElement(doc, admin, "MotDePasse", "420-3GP");
            AppendElement(doc, admin, "Nom", "Administrateur");
            AppendElement(doc, admin, "Prenom", "Principal");
            comptes.AppendChild(admin);

            // Ajout Bob
            XmlElement bob = doc.CreateElement("Compte");
            AppendElement(doc, bob, "Email", "bob.martin@exemple.com");
            AppendElement(doc, bob, "Nom", "Martin");
            AppendElement(doc, bob, "Prenom", "Bob");
            comptes.AppendChild(bob);
        }
    }
}