using Avalonia.Controls;
using Mastonet;
using Neumorphism.Avalonia.Styles;

namespace Hairy.Pages.Panels
{
    public class LoginPanelViewModel : ViewModelBase
    {
        private AuthenticationClient? _authClient;
        public AuthenticationClient AuthenticationClient
        {
            get => _authClient;
        }

        private string _instanceName = "";
        public string InstanceName
        {
            get => _instanceName;
            set
            {
                _instanceName = value;
                OnPropertyChanged();
            }
        }

        private string _token = "";
        public string Token
        {
            get => _token;
            set
            {
                _token = value;
                OnPropertyChanged();
            }
        }

        private bool _isAuthenticated = false;
        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            set
            {
                _isAuthenticated = value;
                OnPropertyChanged();
            }
        }

        public void ButtonTokenClick(object sender)
        {
            if (sender is Button)
            {
                if (string.IsNullOrEmpty(InstanceName))
                {
                    SnackbarHost.Post("Please enter an Instance Name.");
                }
                else
                {
                    if (InstanceName.Contains("http"))
                    {
                        SnackbarHost.Post("Instance should not contain http/https.");
                    }
                    else
                    {
                        _authClient = new AuthenticationClient(InstanceName); 
                        var appRegistration = _authClient?.CreateApp("C# Mastodon - Hairy", Scope.Read | Scope.Write | Scope.Follow).GetAwaiter().GetResult();

                        var url = _authClient?.OAuthUrl();
                        System.Diagnostics.Process.Start("xdg-open", url);
                    }
                }
            }
        }

        public void ButtonValidateTokenClick(object sender)
        {
            if (sender is Button)
            {
                if (string.IsNullOrEmpty(Token) || !(_authClient?.ConnectWithCode(Token).IsCompletedSuccessfully ?? false ) )
                {
                    SnackbarHost.Post("Please enter a valid token.");
                }
                else
                {
                    IsAuthenticated = true;
                }
            }
        }
    }
}
