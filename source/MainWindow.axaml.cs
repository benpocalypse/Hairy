using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Hairy.Pages.Panels;
using Mastonet;
using Neumorphism.Avalonia.Styles;

namespace Hairy;

public partial class MainWindow : Window
{
    private MastodonClient? _client { get; set; } = null;

    private LoginPanel? loginPanel;
    private Button? validateToken;

    public MainWindow()
    {
        InitializeComponent();

        this.Opened += OnLoaded;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        loginPanel = this.FindControl<LoginPanel>("_loginPanel");
        validateToken = loginPanel.FindControl<Button>("_validateToken");
        validateToken.Click += ValidateTokenButtonClicked;
        //loginPanel = this.Get<LoginPanel>(nameof(LoginPanel));
    }

    private void ValidateTokenButtonClicked(object? sender, RoutedEventArgs e)
    {
        var instance = (loginPanel?.DataContext as LoginPanelViewModel)?.InstanceName;
        if (instance != null)
        {
            var authClient = new AuthenticationClient(instance);
            var appRegistration = authClient.CreateApp("Hairy", Scope.Read | Scope.Write | Scope.Follow).GetAwaiter().GetResult();
            
            var token = (loginPanel?.DataContext as LoginPanelViewModel)?.Token;

            _client = new MastodonClient(instance, token ?? string.Empty);
            authClient.ConnectWithCode(token ?? string.Empty);

            var test = _client.GetHomeTimeline().ConfigureAwait(false).GetAwaiter().GetResult();

            foreach (var thing in test)
            {
                var strippedText = StripHTML(thing.Content, true);

                var text = thing.Content == string.Empty ?
                    string.Empty :
                    "\x1b[1m" + "\tText: " + "\x1b[0m" + strippedText + System.Environment.NewLine;

                var media = thing.MediaAttachments.Count() != 0 ?
                            "\x1b[1m\tMedia:\x1b[0m <Undisplayable Media Attachment>" + System.Environment.NewLine:
                            string.Empty;

                var reblog = thing.Reblog != null ?
                    "\x1b[1m" + "\tReblogged from: " + "\x1b[0m" + thing.Reblog.Account.DisplayName + $" ({thing.Reblog.Account.AccountName})" + System.Environment.NewLine +
                    "\x1b[1m" + "\t\tText: " + "\x1b[0m" +  StripHTML(thing.Reblog.Content, true) :
                    string.Empty;

                Console.WriteLine("------------------------------------");
                Console.WriteLine("\x1b[1m" + thing.Account.DisplayName + $" ({thing.Account.AccountName}) " + "\x1b[0m" + 
                    System.Environment.NewLine +
                    reblog +
                    text +
                    media
                    );
            }
        }
    }

    public static string StripHTML(string HTMLText, bool decode = true)
    {
        Regex reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
        var stripped = reg.Replace(HTMLText, "");
        return decode ? HttpUtility.HtmlDecode(stripped) : stripped;
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        this.Opened -= OnLoaded;
    }

    private void TemplatedControl_OnTemplateApplied(object sender, TemplateAppliedEventArgs e)
    {
        SnackbarHost.Post("Welcome to\r\nHairy!");
    }

    private void HelloButtonMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        SnackbarHost.Post("Hello user!");
    }

    private void GoodbyeButtonMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        if (Application.Current != null && Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown(0);
        }
    }
}