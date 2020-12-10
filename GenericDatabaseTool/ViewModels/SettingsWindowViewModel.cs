using System.Security;
using System.Windows.Input;
using GenericDatabaseTool.Managers;
using Utility.MVVM;

namespace GenericDatabaseTool.ViewModels
{
    public class SettingsWindowViewModel : ObservableObject
    {
        /// <summary>
        /// Contains the hostname
        /// </summary>
        private string _hostname;

        /// <summary>
        /// Gets or sets the hostname
        /// </summary>
        public string Hostname
        {
            get => _hostname;
            set => SetField(ref _hostname, value);
        }

        /// <summary>
        /// Contains the Port
        /// </summary>
        private int _port;

        /// <summary>
        /// Gets or sets the Port
        /// </summary>
        public int Port
        {
            get => _port;
            set => SetField(ref _port, value);
        }

        /// <summary>
        /// Contains the user
        /// </summary>
        private string _user;

        /// <summary>
        /// Gets or sets the user
        /// </summary>
        public string User
        {
            get => _user;
            set => SetField(ref _user, value);
        }

        /// <summary>
        /// Contains the password
        /// </summary>
        private string _password;

        /// <summary>
        /// Gets or sets the password
        /// </summary>
        public string Password
        {
            get => _password;
            set => SetField(ref _password, value);
        }

        /// <summary>
        /// Contains the save command
        /// </summary>
        public ICommand SaveCommand => new DelegateCommand(Save);

        /// <summary>
        /// Creates a new instance of <see cref="SettingsWindowViewModel"/>
        /// </summary>
        public SettingsWindowViewModel()
        {
            Hostname = SettingsManager.Host;
            Port = SettingsManager.Port;
            User = SettingsManager.User;
            Password = SettingsManager.Password;
        }

        /// <summary>
        /// Saves the settings
        /// </summary>
        public void Save()
        {
            SettingsManager.Port = Port;
            SettingsManager.Host = Hostname;
            SettingsManager.Password = Password;
            SettingsManager.User = User;
            SettingsManager.Save();
        }
    }
}
