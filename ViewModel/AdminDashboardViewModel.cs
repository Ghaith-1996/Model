using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace Bibliotheque.ViewModels
{
    //ce que L'admin voit en se connectant
    public class AdminDashboardViewModel : BaseViewModel
    {
        public ICommand AllerAjoutLivreCommand { get; }
        public ICommand AllerSuppressionLivreCommand { get; }
        public ICommand AllerListeUtilisateursCommand { get; }

        
           public AdminDashboardViewModel()
        {
            AllerAjoutLivreCommand = new Command(async () =>
            {
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

