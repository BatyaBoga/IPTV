using CommunityToolkit.Mvvm.ComponentModel;
using System;
using Windows.Media.Core;
using Windows.Media.Streaming.Adaptive;
using Windows.Storage;

namespace IPTV.ViewModels
{
    public class StreamViewModel : ObservableObject
    {
        private MediaSource stream;

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
           var adaptiveSource = await AdaptiveMediaSource
                .CreateFromStreamAsync(await file.OpenReadAsync(), new Uri(file.Path), file.ContentType);

           Stream = MediaSource.CreateFromAdaptiveMediaSource(adaptiveSource.MediaSource);
        }

        public void SetSource(string link)
        {
            var a = MediaSource.CreateFromUri(new Uri(link));

            Stream = a;
        }
    }
}
