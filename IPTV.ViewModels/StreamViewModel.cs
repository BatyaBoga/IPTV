using System;
using Windows.Storage;
using Windows.Media.Core;
using Windows.Media.Streaming.Adaptive;
using CommunityToolkit.Mvvm.ComponentModel;
using IPTV.Interfaces;

namespace IPTV.ViewModels
{
    public class StreamViewModel : ObservableObject
    {
        private readonly IMessageDialog message;

        private readonly INavigationService navigation;

        private MediaSource stream;

        public string StorageFilePath;

        public StreamViewModel(IMessageDialog message, INavigationService navigation)
        {
            this.message = message;

            this.navigation = navigation;
        }

        public MediaSource Stream
        {
            get
            {
                return stream;
            }
            set
            {
                SetProperty(ref stream, value);
            }
        }

        public async void SetSource(StorageFile file)
        {
            try
            {
                var adaptiveSource = await AdaptiveMediaSource
                    .CreateFromStreamAsync(await file.OpenReadAsync(), new Uri(file.Path), file.ContentType);

                Stream = MediaSource.CreateFromAdaptiveMediaSource(adaptiveSource.MediaSource);
                
                StorageFilePath = file.Path;
            }
            catch (Exception)
            {
                Stream = null;

                await message.ShowInfoMsg("NotSupported");

                navigation.GoBack();
            }
        }

        public void SetSource(string link)
        {
            Stream = MediaSource.CreateFromUri(new Uri(link));
        }
    }
}
