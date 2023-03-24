using Avalonia.Controls;
using Avalonia.Threading;
using Mastonet;
using Neumorphism.Avalonia.Styles;

namespace Hairy.Pages.Panels
{
    public class LoginPanelViewModel : ViewModelBase
    {
        //private readonly string _instanceNamePreface = "https://";
        private string _instanceName = "";//"mastodon.social";
        public string InstanceName
        {
            get => _instanceName; //_instanceNamePreface + 
            set
            {
                _instanceName = value;//.Substring(8);
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
                    //Dispatcher.UIThread.Post(() => 
                    {
                        var authClient = new AuthenticationClient(InstanceName);
                        var appRegistration = authClient.CreateApp("C# Mastodon - Hairy", Scope.Read | Scope.Write | Scope.Follow).GetAwaiter().GetResult();

                        var url = authClient.OAuthUrl();
                        System.Diagnostics.Process.Start("xdg-open", url);
                        ///_ = System.Diagnostics.Process.Start("xdg-open", url);
                    }//, DispatcherPriority.Normal);
                }
            }
        }

        public void ButtonValidateTokenClick(object sender)
        {
            if (sender is Button)
            {
                // TODO - validation
                if (string.IsNullOrEmpty(Token))
                {
                    SnackbarHost.Post("Please enter a token.");
                }
            }
        }
    }
}
