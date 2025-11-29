using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace Bibliotheque.ViewModels
{
    public class AdminDashboardViewModel : BaseViewModel
    {
        public ICommand AllerAjoutLivreCommand { get; }
        public ICommand AllerSuppressionLivreCommand { get; }
        public ICommand AllerListeUtilisateursCommand { get; }

        
           public AdminDashboardViewModel()
        {
            AllerAjoutLivreCommand = new Command(async () =>
            {
                // ⛔ AVANT : await Shell.Current.GoToAsync("//AdminAjoutLivrePage");
                await Shell.Current.GoToAsync("AdminAjoutLivrePage");
            });

            AllerSuppressionLivreCommand = new Command(async () =>
            {
                await Shell.Current.GoToAsync("AdminSuppressionLivrePage");
            });

            AllerListeUtilisateursCommand = new Command(async () =>
            {
                await Shell.Current.GoToAsync("AdminListeUtilisateursPage");
            });
        }

    }
}

