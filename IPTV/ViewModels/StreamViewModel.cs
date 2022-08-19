using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Media.Streaming.Adaptive;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

namespace IPTV.ViewModels
{
    public class StreamViewModel : ObservableObject
    {

        private Uri stream;
        public Uri Stream
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

            var a = MediaSource.CreateFromStorageFile(file);

            Stream = new Uri(file.Path);
        }


    }
}
