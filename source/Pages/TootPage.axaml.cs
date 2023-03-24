using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Hairy.Pages.Panels;

namespace Hairy.Pages
{
    public partial class TootPage : UserControl
    {
        private ListBox _tootsListBox;
        public ObservableCollection<TootPanel> TootsList { get; set; } = new();
        public TootPage()
        {
            this.InitializeComponent();
            _tootsListBox = this.FindControl<ListBox>("_lbToots");
            TootsList.CollectionChanged += TootsListChanged;
        }

        private void TootsListChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender != null)
            {
                _tootsListBox.Items = ((ObservableCollection<TootPanel>)sender);
            }
            //_tootsListBox.InvalidateVisual();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}