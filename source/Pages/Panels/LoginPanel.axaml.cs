using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Hairy.Pages.Panels
{
    public partial class LoginPanel : UserControl
    {
        public LoginPanel()
        {
            this.InitializeComponent();

            this.DataContext = new LoginPanelViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}