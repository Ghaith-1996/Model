using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Bibliotheque.ViewModels
{
    /// <summary>
    /// ViewModel de base qui gère INotifyPropertyChanged et SetProperty.
    /// Tous tes ViewModels peuvent hériter de cette classe.
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Méthode utilitaire pour mettre à jour un champ + déclencher PropertyChanged.
        /// </summary>
        protected bool SetProperty<T>(
            ref T backingStore,
            T value,
            [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
