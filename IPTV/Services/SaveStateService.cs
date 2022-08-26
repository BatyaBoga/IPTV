using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Core.Preview;
using Newtonsoft.Json;
using IPTV.Interfaces;
using IPTV.ViewModels;
using IPTV.Constants;
using IPTV.Models.Model.SettingsModel;

namespace IPTV.Services
{
    public class SaveStateService : ISaveStateService
    {
        private readonly INavigationService navigationService;

        private readonly IIptvManager manager;

        private SavedMedia savedMedia;

        private string menuItem;

        private object viewModel;

        private Action actionToLoad;

        public SaveStateService(INavigationService navigationService, IIptvManager manager)
        {
            this.navigationService = navigationService;

            this.manager = manager;
        }

        public object ParametrToMain => savedMedia != null ? savedMedia.MenuItem : null;
   
        public void ActiveSave(string menuItem, object viewModel)
        {
            this.menuItem = menuItem;

            this.viewModel = viewModel;
                
            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += this.OnCloseRequest;
        }

        public void DeactiveSave()
        {
            savedMedia = null;

            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested -= this.OnCloseRequest;
        }

        public async void LoadSavedMedia()
        {
            var settingsFile = await GetSettingsFile();

            string savedtext = await FileIO.ReadTextAsync(settingsFile);

            if(savedtext != String.Empty)
            {
               savedMedia = JsonConvert.DeserializeObject<SavedMedia>(savedtext);
            }

            if(savedMedia != null)
            {
                if (savedMedia.SavedPlaylist != null)
                {
                    actionToLoad = LoadForPlaylist;
                }
                else if(savedMedia.SavedStream != null)
                {
                    actionToLoad = LoadForStream;
                }
            }   
        }

        public Task LoadSaveMediaAsync() 
        {
            return Task.Run(LoadSavedMedia);
        }

        public async void GoToLoadedMedia()
        {
            if(actionToLoad != null)
            {
                actionToLoad.Invoke();

                var file = await GetSettingsFile();

                await FileIO.WriteTextAsync(file, "");
            }
        }

        private async Task<StorageFile> GetSettingsFile()
        {
            return await ApplicationData.Current.LocalFolder
                .CreateFileAsync(Constant.SettingsFile, CreationCollisionOption.OpenIfExists);
        }

        private async void LoadForPlaylist()
        {
            var playlist = await manager.GetPlaylistByFileName(savedMedia.SavedPlaylist.FileName);

            ViewModelLocator.Instance.PlayList.SelectedIndex = savedMedia.SavedPlaylist.StreamId;

            navigationService.Navigate<PlayListViewModel>(playlist);
        }

        private async void LoadForStream()
        {
            if(savedMedia.MenuItem == Constant.Remote)
            {
                navigationService.Navigate<StreamViewModel>(savedMedia.SavedStream.Link);
            }
            else
            {
                var file = await TryLoadFile(savedMedia.SavedStream.FilePath);

                if(file != null)
                {
                    navigationService.Navigate<StreamViewModel>(file);
                }
            }
        }

        private async Task<StorageFile> TryLoadFile(string FilePath)
        {
            StorageFile storageFile;

            try
            {
               storageFile = await StorageFile.GetFileFromPathAsync(savedMedia.SavedStream.FilePath);

            }
            catch (FileNotFoundException)
            {
                storageFile = null;
            }

            return storageFile;
        }

        private void InizializedSavedMedia()
        {
            savedMedia = new SavedMedia
            {
                MenuItem = menuItem,

                SavedPlaylist = null,

                SavedStream = null
            };

            if (viewModel is PlayListViewModel)
            {
                InizializedSavedPlaylist();
            }
            else if (viewModel is StreamViewModel)
            {
                InizializedSavedStream();
            }
        }

        private Task InizializedSavedMediaAsync()
        {
            return Task.Run(InizializedSavedMedia);
        }

        private void InizializedSavedPlaylist()
        {
            var playlistViewModel = viewModel as PlayListViewModel;

            var savedplaylist = new SavedPlaylist()
            {
                FileName = playlistViewModel.PlayList.FileName,

                StreamId = playlistViewModel.SelectedIndex
            };

            savedMedia.SavedPlaylist = savedplaylist;
        }

        private void InizializedSavedStream()
        {
            var streamViewModel = viewModel as StreamViewModel;

            var savedstream = new SavedStream();

            if (menuItem == Constant.Local)
            {
                savedstream.FilePath = streamViewModel.StorageFilePath;
            }
            else
            {
                savedstream.Link = streamViewModel.Stream.Uri.OriginalString;
            }

            savedMedia.SavedStream = savedstream;
        }

        private async void OnCloseRequest(object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
            await InizializedSavedMediaAsync();

            var settingsFile = await GetSettingsFile();

            if (savedMedia != null)
            {
                await FileIO.WriteTextAsync(settingsFile, JsonConvert.SerializeObject(savedMedia));
            }
        }
    }
}
