using System.Collections.Generic;

namespace Bibliotheque.Model
{
  
    
    
    public class Bibliotheque
    {
        public List<Livre> Livres { get; } = new();
        public List<Compte> Comptes { get; } = new();
        public List<Evaluation> Evaluations { get; } = new();

        public Bibliotheque()
        {
        }
    }
}
