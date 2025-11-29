using System.Windows.Input;
using Microsoft.Maui.Controls;
using Bibliotheque.Model;
using Bibliotheque.Services;

namespace Bibliotheque.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly AuthService _authService;

        private string _email = string.Empty;
        private string _motDePasse = string.Empty;
        private string _messageErreur = string.Empty;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string MotDePasse
        {
            get => _motDePasse;
            set => SetProperty(ref _motDePasse, value);
        }

        public string MessageErreur
        {
            get => _messageErreur;
            set => SetProperty(ref _messageErreur, value);
        }

        public ICommand SeConnecterCommand { get; }

        public LoginViewModel(AuthService authService)
        {
            _authService = authService;
            SeConnecterCommand = new Command(SeConnecter);
        }

        private async void SeConnecter()
        {
            MessageErreur = string.Empty;

            if (string.IsNullOrWhiteSpace(Email))
            {
                MessageErreur = "Veuillez entrer votre courriel.";
                return;
            }

            string email = Email.Trim();
            string motDePasse = MotDePasse ?? string.Empty;

            var compte = _authService.Authentifier(email, motDePasse);

            if (compte == null)
            {
                MessageErreur = "Courriel ou mot de passe invalide.";
                return;
            }

            Session.CompteCourant = compte;

            if (compte.EstAdministrateur)
            {
                await Shell.Current.GoToAsync("AdminDashboardPage");
            }
            else
            {
                await Shell.Current.GoToAsync("ListeLivresPage");
            }
        }



    }
}
