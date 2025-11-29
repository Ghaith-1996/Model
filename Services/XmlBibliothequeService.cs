using Bibliotheque.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;

namespace Bibliotheque.Services
{
    /// <summary>
    /// Service responsable de la lecture / écriture du fichier bibliotheque.xml.
    /// </summary>
    public class XmlBibliothequeService
    {
        private readonly string _cheminFichier;

        public XmlBibliothequeService(string cheminFichier)
        {
            _cheminFichier = cheminFichier;

            // S'assurer que le dossier existe
            string? dossier = Path.GetDirectoryName(_cheminFichier);
            if (!string.IsNullOrEmpty(dossier) && !Directory.Exists(dossier))
            {
                Directory.CreateDirectory(dossier);
            }

            // Forcer la création / vérification du XML dès le départ
            ChargerDocument();
        }

        // ---------- PUBLIC : LIVRES ----------

        public List<Livre> ChargerLivres()
        {
            XmlDocument doc = ChargerDocument();
            List<Livre> livres = new();

            XmlElement? racine = doc.DocumentElement;
            if (racine == null)
                return livres;

            XmlElement? noeudLivres = racine["Livres"];
            if (noeudLivres == null)
                return livres;

            XmlNodeList listeLivres = noeudLivres.GetElementsByTagName("Livre");
            foreach (XmlElement elementLivre in listeLivres)
            {
                Livre livre = new Livre
                {
                    Titre = elementLivre["Titre"]?.InnerText ?? string.Empty,
                    Auteur = elementLivre["Auteur"]?.InnerText ?? string.Empty,
                    ISBN = elementLivre["ISBN"]?.InnerText ?? string.Empty,
                    MaisonEdition = elementLivre["MaisonEdition"]?.InnerText ?? string.Empty,
                    DatePublication = elementLivre["DatePublication"]?.InnerText ?? string.Empty,
                    Description = elementLivre["Description"]?.InnerText ?? string.Empty
                };

                if (double.TryParse(
                        elementLivre["MoyenneEvaluation"]?.InnerText,
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        out double moyenne))
                {
                    livre.MoyenneEvaluation = moyenne;
                }

                if (int.TryParse(
                        elementLivre["NombreEvaluations"]?.InnerText,
                        out int nbEval))
                {
                    livre.NombreEvaluations = nbEval;
                }

                livres.Add(livre);
            }

            System.Diagnostics.Debug.WriteLine($"[DEBUG] ChargerLivres() -> {livres.Count} livres");
            foreach (var l in livres)
                System.Diagnostics.Debug.WriteLine($"[DEBUG] Livre: {l.Titre} | ISBN={l.ISBN}");

            return livres;
        }

        public void SauvegarderLivres(List<Livre> livres)
        {
            XmlDocument doc = ChargerDocument();
            XmlElement? racine = doc.DocumentElement;
            if (racine == null)
                return;

            XmlElement? noeudLivres = racine["Livres"];
            if (noeudLivres == null)
            {
                noeudLivres = doc.CreateElement("Livres");
                racine.AppendChild(noeudLivres);
            }
            else
            {
                noeudLivres.RemoveAll();
            }

            foreach (Livre livre in livres)
            {
                XmlElement livreElement = doc.CreateElement("Livre");

                AppendElement(doc, livreElement, "Titre", livre.Titre);
                AppendElement(doc, livreElement, "Auteur", livre.Auteur);
                AppendElement(doc, livreElement, "ISBN", livre.ISBN);
                AppendElement(doc, livreElement, "MaisonEdition", livre.MaisonEdition);
                AppendElement(doc, livreElement, "DatePublication", livre.DatePublication);
                AppendElement(doc, livreElement, "Description", livre.Description);
                AppendElement(doc, livreElement, "MoyenneEvaluation",
                    livre.MoyenneEvaluation.ToString(CultureInfo.InvariantCulture));
                AppendElement(doc, livreElement, "NombreEvaluations",
                    livre.NombreEvaluations.ToString(CultureInfo.InvariantCulture));

                noeudLivres.AppendChild(livreElement);
            }

            doc.Save(_cheminFichier);
        }

        public void AjouterLivre(Livre nouveauLivre)
        {
            var livres = ChargerLivres();

            bool existeDeja = livres.Any(l => l.ISBN == nouveauLivre.ISBN);
            if (existeDeja)
                throw new InvalidOperationException("Un livre avec cet ISBN existe déjà.");

            livres.Add(nouveauLivre);
            SauvegarderLivres(livres);
        }

        public void SupprimerLivreParIsbn(string isbn)
        {
            var livres = ChargerLivres();

            var restants = livres
                .Where(l => !l.ISBN.Equals(isbn, StringComparison.OrdinalIgnoreCase))
                .ToList();

            SauvegarderLivres(restants);
        }

        // ---------- PUBLIC : COMPTES ----------

        public List<Compte> ChargerComptes()
        {
            XmlDocument doc = ChargerDocument();
            List<Compte> comptes = new();

            XmlElement? racine = doc.DocumentElement;
            if (racine == null)
                return comptes;

            XmlElement? noeudComptes = racine["Comptes"];
            if (noeudComptes == null)
                return comptes;

            XmlNodeList listeComptes = noeudComptes.GetElementsByTagName("Compte");
            foreach (XmlElement elementCompte in listeComptes)
            {
                string email = elementCompte["Email"]?.InnerText ?? string.Empty;
                string motDePasse = elementCompte["MotDePasse"]?.InnerText ?? string.Empty;
                string nom = elementCompte["Nom"]?.InnerText ?? string.Empty;
                string prenom = elementCompte["Prenom"]?.InnerText ?? string.Empty;

                bool estAdmin = email.Equals("admin@exemple.com", StringComparison.OrdinalIgnoreCase);

                Compte compte = new Compte(email, nom, prenom, estAdmin)
                {
                    MotDePasse = motDePasse
                };

                comptes.Add(compte);
            }

            return comptes;
        }

        public void SauvegarderComptes(List<Compte> comptes)
        {
            XmlDocument doc = ChargerDocument();
            XmlElement? racine = doc.DocumentElement;
            if (racine == null)
                return;

            XmlElement? noeudComptes = racine["Comptes"];
            if (noeudComptes == null)
            {
                noeudComptes = doc.CreateElement("Comptes");
                racine.AppendChild(noeudComptes);
            }
            else
            {
                noeudComptes.RemoveAll();
            }

            foreach (Compte compte in comptes)
            {
                XmlElement compteElement = doc.CreateElement("Compte");

                AppendElement(doc, compteElement, "Email", compte.Email);
                AppendElement(doc, compteElement, "MotDePasse", compte.MotDePasse);
                AppendElement(doc, compteElement, "Nom", compte.Nom);
                AppendElement(doc, compteElement, "Prenom", compte.Prenom);

                noeudComptes.AppendChild(compteElement);
            }

            doc.Save(_cheminFichier);
        }

        // ---------- PUBLIC : EVALUATIONS (je garde ton code) ----------

        public List<Evaluation> ChargerEvaluations()
        {
            XmlDocument doc = ChargerDocument();
            List<Evaluation> evaluations = new();

            XmlElement? racine = doc.DocumentElement;
            if (racine == null)
                return evaluations;

            XmlElement? noeudEvaluations = racine["Evaluations"];
            if (noeudEvaluations == null)
                return evaluations;

            XmlNodeList listeEval = noeudEvaluations.GetElementsByTagName("Evaluation");
            foreach (XmlElement elementEval in listeEval)
            {
                string email = elementEval["EmailClient"]?.InnerText ?? string.Empty;
                string isbn = elementEval["IsbnLivre"]?.InnerText ?? string.Empty;
                double note = 0;

                double.TryParse(
                    elementEval["Note"]?.InnerText,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out note);

                evaluations.Add(new Evaluation(email, isbn, note));
            }

            return evaluations;
        }

        public void SauvegarderEvaluations(List<Evaluation> evaluations)
        {
            XmlDocument doc = ChargerDocument();
            XmlElement? racine = doc.DocumentElement;
            if (racine == null)
                return;

            XmlElement? noeudEvaluations = racine["Evaluations"];
            if (noeudEvaluations == null)
            {
                noeudEvaluations = doc.CreateElement("Evaluations");
                racine.AppendChild(noeudEvaluations);
            }
            else
            {
                noeudEvaluations.RemoveAll();
            }

            foreach (var eval in evaluations)
            {
                XmlElement evalElement = doc.CreateElement("Evaluation");

                AppendElement(doc, evalElement, "EmailClient", eval.EmailClient);
                AppendElement(doc, evalElement, "IsbnLivre", eval.IsbnLivre);
                AppendElement(doc, evalElement, "Note",
                    eval.Note.ToString(CultureInfo.InvariantCulture));

                noeudEvaluations.AppendChild(evalElement);
            }

            doc.Save(_cheminFichier);
        }

        public double? ObtenirNoteUtilisateurPourLivre(string emailClient, string isbnLivre)
        {
            var evaluations = ChargerEvaluations();

            var eval = evaluations
                .FirstOrDefault(e =>
                    e.EmailClient.Equals(emailClient, StringComparison.OrdinalIgnoreCase) &&
                    e.IsbnLivre.Equals(isbnLivre, StringComparison.OrdinalIgnoreCase));

            return eval == null ? (double?)null : eval.Note;
        }

        public void EnregistrerEvaluationPourLivre(string emailClient, string isbnLivre, double nouvelleNote)
        {
            var evaluations = ChargerEvaluations();

            var evalExistante = evaluations
                .FirstOrDefault(e =>
                    e.EmailClient.Equals(emailClient, StringComparison.OrdinalIgnoreCase) &&
                    e.IsbnLivre.Equals(isbnLivre, StringComparison.OrdinalIgnoreCase));

            var livres = ChargerLivres();
            var livre = livres.FirstOrDefault(l => l.ISBN == isbnLivre);

            if (livre == null)
                throw new InvalidOperationException("Livre introuvable pour l’ISBN donné.");

            if (evalExistante == null)
            {
                livre.AjouterEvaluation(nouvelleNote);
                evaluations.Add(new Evaluation(emailClient, isbnLivre, nouvelleNote));
            }
            else
            {
                double ancienneNote = evalExistante.Note;
                livre.ModifierEvaluation(ancienneNote, nouvelleNote);
                evalExistante.Note = nouvelleNote;
            }

            SauvegarderLivres(livres);
            SauvegarderEvaluations(evaluations);
        }

        // ---------- PRIVÉ : CREATION / INIT ----------

        private XmlDocument ChargerDocument()
        {
            XmlDocument doc = new XmlDocument();

            if (File.Exists(_cheminFichier))
            {
                doc.Load(_cheminFichier);

                XmlElement? racine = doc.DocumentElement;
                if (racine == null || racine.Name != "Bibliotheque")
                {
                    return CreerDocumentInitial();
                }

                XmlElement? livres = racine["Livres"];
                if (livres == null)
                {
                    livres = doc.CreateElement("Livres");
                    racine.AppendChild(livres);
                }

                XmlElement? comptes = racine["Comptes"];
                if (comptes == null)
                {
                    comptes = doc.CreateElement("Comptes");
                    racine.AppendChild(comptes);
                }

                XmlElement? evaluations = racine["Evaluations"];
                if (evaluations == null)
                {
                    evaluations = doc.CreateElement("Evaluations");
                    racine.AppendChild(evaluations);
                }

                // Si aucun livre et aucun compte → on injecte les données initiales
                if (!livres.HasChildNodes && !comptes.HasChildNodes)
                {
                    RemplirDonneesInitiales(doc, livres, comptes);
                    doc.Save(_cheminFichier);
                }

                return doc;
            }

            // Fichier inexistant → on le crée
            return CreerDocumentInitial();
        }

        private XmlDocument CreerDocumentInitial()
        {
            XmlDocument doc = new XmlDocument();

            XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(declaration);

            XmlElement racine = doc.CreateElement("Bibliotheque");
            doc.AppendChild(racine);

            XmlElement livres = doc.CreateElement("Livres");
            XmlElement comptes = doc.CreateElement("Comptes");
            XmlElement evaluations = doc.CreateElement("Evaluations");

            racine.AppendChild(livres);
            racine.AppendChild(comptes);
            racine.AppendChild(evaluations);

            RemplirDonneesInitiales(doc, livres, comptes);

            doc.Save(_cheminFichier);
            return doc;
        }

        private void RemplirDonneesInitiales(
            XmlDocument doc,
            XmlElement livres,
            XmlElement comptes)
        {
            // LIVRES
            XmlElement livre1 = doc.CreateElement("Livre");
            AppendElement(doc, livre1, "Titre", "Les Fleurs du Mal");
            AppendElement(doc, livre1, "Auteur", "Charles Baudelaire");
            AppendElement(doc, livre1, "ISBN", "978-2-07-038255-2");
            AppendElement(doc, livre1, "MaisonEdition", "Gallimard");
            AppendElement(doc, livre1, "DatePublication", "1857-06-25");
            AppendElement(doc, livre1, "Description",
                "Recueil de poèmes majeur de la littérature française.");
            AppendElement(doc, livre1, "MoyenneEvaluation", "4.9");
            AppendElement(doc, livre1, "NombreEvaluations", "1");
            livres.AppendChild(livre1);

            XmlElement livre2 = doc.CreateElement("Livre");
            AppendElement(doc, livre2, "Titre", "Neuromancien");
            AppendElement(doc, livre2, "Auteur", "William Gibson");
            AppendElement(doc, livre2, "ISBN", "978-2-07-041573-0");
            AppendElement(doc, livre2, "MaisonEdition", "J'ai lu");
            AppendElement(doc, livre2, "DatePublication", "1984");
            AppendElement(doc, livre2, "Description",
                "Un roman cyberpunk qui a popularisé le terme \"matric.\"");
            AppendElement(doc, livre2, "MoyenneEvaluation", "4.3");
            AppendElement(doc, livre2, "NombreEvaluations", "1");
            livres.AppendChild(livre2);

            // COMPTES
            XmlElement compteAdmin = doc.CreateElement("Compte");
            AppendElement(doc, compteAdmin, "Email", "admin@exemple.com");
            AppendElement(doc, compteAdmin, "MotDePasse", "420-3GP");
            AppendElement(doc, compteAdmin, "Nom", "Administrateur");
            AppendElement(doc, compteAdmin, "Prenom", "Principal");
            comptes.AppendChild(compteAdmin);

            XmlElement compteBob = doc.CreateElement("Compte");
            AppendElement(doc, compteBob, "Email", "bob.martin@exemple.com");
            AppendElement(doc, compteBob, "Nom", "Martin");
            AppendElement(doc, compteBob, "Prenom", "Bob");
            comptes.AppendChild(compteBob);
        }

        private static void AppendElement(XmlDocument doc, XmlElement parent, string name, string value)
        {
            XmlElement child = doc.CreateElement(name);
            child.InnerText = value;
            parent.AppendChild(child);
        }
    }
}
