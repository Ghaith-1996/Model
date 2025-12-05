using NUnit.Framework;
using Bibliotheque.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public class BibliothequeTests
    {
        private Livre _livre;
        // On simule une liste de livres pour tester l'ajout/suppression
        private List<Livre> _listeLivres;

        [SetUp]
        public void Setup()
        {
           
            _livre = new Livre("Titre Test", "Auteur Test", "123-456");

            
            _listeLivres = new List<Livre>();
        }


        //ajout et supprimer de livre
        [Test]
        public void AjouterLivre_AjouteBienALaListe()
        {
            var nouveauLivre = new Livre("L'Étranger", "Camus", "978-2070360024");

            
            _listeLivres.Add(nouveauLivre);

            
            Assert.That(_listeLivres.Count, Is.EqualTo(1), "La liste devrait contenir 1 livre.");
            Assert.That(_listeLivres.First(), Is.EqualTo(nouveauLivre), "Le livre ajouté devrait être celui qu'on retrouve.");
        }

        [Test]
        public void SupprimerLivre_RetireBienDeLaListe()
        {
           
            var livreASupprimer = new Livre("L'Étranger", "Camus", "978-2070360024");
            _listeLivres.Add(livreASupprimer);
            Assert.That(_listeLivres.Count, Is.EqualTo(1)); // Vérification préalable

            
            _listeLivres.Remove(livreASupprimer);

           
            Assert.That(_listeLivres.Count, Is.EqualTo(0), "La liste devrait être vide après suppression.");
        }

        
        [Test]
        public void RechercheLivre_ParISBN_TrouveLeBonLivre()
        {
          
            var livre1 = new Livre("A", "A", "111");
            var livre2 = new Livre("B", "B", "222");
            _listeLivres.Add(livre1);
            _listeLivres.Add(livre2);

            
            var livreTrouve = _listeLivres.FirstOrDefault(l => l.ISBN == "222");

            
            Assert.IsNotNull(livreTrouve, "Le livre devrait être trouvé.");
            Assert.That(livreTrouve!.Titre, Is.EqualTo("B"), "Le titre du livre trouvé est incorrect.");
        }

        //Evaluation

        [Test]
        public void AjouterEvaluation_PremiereNote_CalculeMoyenneCorrectement()
        {
            _livre.MoyenneEvaluation = 0;
            _livre.NombreEvaluations = 0;
            _livre.AjouterEvaluation(5.0);

            Assert.That(_livre.NombreEvaluations, Is.EqualTo(1));
            Assert.That(_livre.MoyenneEvaluation, Is.EqualTo(5.0));
        }

        [Test]
        public void AjouterEvaluation_PlusieursNotes_CalculeMoyennePonderee()
        {
            _livre.MoyenneEvaluation = 4.0;
            _livre.NombreEvaluations = 1;
            _livre.AjouterEvaluation(2.0);

            Assert.That(_livre.NombreEvaluations, Is.EqualTo(2));
            Assert.That(_livre.MoyenneEvaluation, Is.EqualTo(3.0));
        }

        [Test]
        public void ModifierEvaluation_ChangeMoyenne_SansChangerNombreEvaluations()
        {
            _livre.MoyenneEvaluation = 4.0;
            _livre.NombreEvaluations = 2;

            
            _livre.ModifierEvaluation(5.0, 3.0);

            Assert.That(_livre.NombreEvaluations, Is.EqualTo(2));
            Assert.That(_livre.MoyenneEvaluation, Is.EqualTo(3.0));
        }

        [Test]
        public void EstFavori_RetourneVrai_SiNoteSuperieureOuEgaleA_4()
        {
            _livre.MoyenneEvaluation = 4.0;
            Assert.IsTrue(_livre.EstFavori());

            _livre.MoyenneEvaluation = 3.9;
            Assert.IsFalse(_livre.EstFavori());
        }
    }
}