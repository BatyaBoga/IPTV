using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.Storage.Pickers;
using IPTV.Constants;
using IPTV.Interfaces;
using IPTV.Models.Model;

namespace IPTV.Models
{
    class MediaFileManager : IMediaFile
    {
        private readonly IExplorer explorer;

        public MediaFileManager(IExplorer explorer)
        {
            this.explorer = explorer;

            Task.Run(async () => await ConfigExplorer()).Wait();
        }

        public async Task<ObservableCollection<LocalChannel>> LoadAllLocalVideo()
        {
            var collection = await LoadAllChannelsFromViedos();

            var loaclFiles = await explorer.GetAllFiles();

            foreach (var item in loaclFiles)
            {
                collection.Add(new LocalChannel() {LocalFile = item, CanDelete = true});
            }

            return collection;
        }

        public async Task<bool> AddFile(ObservableCollection<LocalChannel> localChannels)
        {
            bool added = false;

            var picker = new FileOpenPicker();

            picker.ViewMode = PickerViewMode.Thumbnail;

            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;

            picker.FileTypeFilter.Add(".m3u8");

            var file = await picker.PickSingleFileAsync();

            if (file != null && !HasFile(localChannels, file))
            {
                localChannels.Add(new LocalChannel() { LocalFile = file, CanDelete = true });

                await explorer.SaveNewFile(file);

                added = true;
            }

            return added;
        }

        public async Task DeleteFile(ObservableCollection<LocalChannel> localChannels, StorageFile file)
        {
           localChannels.Remove(LocalChanelByFile(localChannels, file));

           await file.DeleteAsync();
        }

        private LocalChannel LocalChanelByFile(ObservableCollection<LocalChannel> localChannels, StorageFile file)
        {
            return (from item in localChannels where item.LocalFile.Path == file.Path select item).FirstOrDefault();
        }

        private async Task<ObservableCollection<LocalChannel>> LoadAllChannelsFromViedos()
        {
            var channels = new ObservableCollection<LocalChannel>();

            var queryOption = new QueryOptions(CommonFileQuery.OrderByTitle, new string[] { ".m3u8" });

            queryOption.FolderDepth = FolderDepth.Deep;

            var files = await KnownFolders.VideosLibrary.CreateFileQueryWithOptions(queryOption).GetFilesAsync();

            foreach (var file in files)
            {
                channels.Add(new LocalChannel() { LocalFile = file, CanDelete = false });
            }

            return channels;
        }

        private bool HasFile(ObservableCollection<LocalChannel> localChannels, StorageFile file)
        {
            bool hasFile = false;

            foreach (var item in localChannels)
            {
                if(item.LocalFile.Path == file.Path)
                {
                    hasFile = true;
                    break;
                }
            }

            return hasFile;
        }

        private async Task ConfigExplorer()
        {
            try
            {
                await explorer.ConfigureFolder(Constant.LocalVideoFolderName);
            }
            catch (FileNotFoundException)
            {
                await explorer.CreateFolder(Constant.LocalVideoFolderName);
            }
        }
    }
}
