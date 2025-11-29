using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Bibliotheque.ViewModels
{
    /// <summary>
    /// ViewModel de base qui gère INotifyPropertyChanged.
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? nom = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nom));
        }

        protected bool SetProperty<T>(ref T champ, T valeur, [CallerMemberName] string? nom = null)
        {
            if (Equals(champ, valeur))
                return false;

            champ = valeur;
            OnPropertyChanged(nom);
            return true;
        }
    }
}
