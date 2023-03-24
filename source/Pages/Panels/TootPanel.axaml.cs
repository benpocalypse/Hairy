using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Hairy.Pages.Panels
{
    public partial class TootPanel : UserControl
    {
        public TootPanel()
        {
            this.InitializeComponent();
            //var tooter = this.FindControl<TextBlock>("TooterContext");
            //tooter.TextWrapping = Avalonia.Media.TextWrapping.Wrap;
        }

        public TootPanel(string tootAuthor, string tootContent, string avatarImageUrl)
        {
            this.InitializeComponent();

            this.DataContext = new TootPanelViewModel(tootAuthor, tootContent, avatarImageUrl);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}