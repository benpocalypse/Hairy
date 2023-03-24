using System;
using System.IO;
using System.Net;
using Avalonia.Controls;
using Avalonia.Threading;
using Mastonet;
using Neumorphism.Avalonia.Styles;

namespace Hairy.Pages.Panels
{
    public sealed class TootPanelViewModel : ViewModelBase
    {
        public TootPanelViewModel(string tootAuthor, string tootContent, string avatarImageUrl)
        {
            TootAuthor = tootAuthor;
            TootContent = tootContent;
            AvatarImageUrl = avatarImageUrl;
            //_avatarImage = new(string.Empty);
        }

        private Avalonia.Media.Imaging.Bitmap _avatarImage;
        public Avalonia.Media.Imaging.Bitmap AvatarImage
        {
            get => _avatarImage;
            set
            {
                _avatarImage = value;
                OnPropertyChanged();
            }
        }

        private string _tootAuthor = "";
        public string TootAuthor
        {
            get => _tootAuthor;
            set
            {
                _tootAuthor = value;
                OnPropertyChanged();
            }
        }

        private string _tootContent = "";
        public string TootContent
        {
            get => _tootContent;
            set
            {
                _tootContent = value;
                OnPropertyChanged();
            }
        }

        private string _avatarImageUrl = "";
        public string AvatarImageUrl
        {
            get => _avatarImageUrl;
            set
            {
                _avatarImageUrl = value;
                if (_avatarImageUrl != "")
                {
                    DownloadImage(_avatarImageUrl);
                }

                OnPropertyChanged();
            }
        }

        public void DownloadImage(string url)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadDataCompleted += DownloadComplete;
                client.DownloadDataAsync(new Uri(url));
            }
        }

        private void DownloadComplete(object sender, DownloadDataCompletedEventArgs e)
        {
            try
            {
                byte[] bytes = e.Result;

                Stream stream = new MemoryStream(bytes);

                var image = new Avalonia.Media.Imaging.Bitmap(stream);
                AvatarImage = image;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                AvatarImage = null; // Could not download...
            }
            
        }
    }
}