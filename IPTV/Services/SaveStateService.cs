using IPTV.Models.Model;
using IPTV.Interfaces;
using IPTV.ViewModels;
using IPTV.Constants;
using Windows.Storage;
using Windows.UI.Core.Preview;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;

namespace IPTV.Services
{
    public class SaveStateService : ISaveStateService
    {
        private readonly INavigationService navigationService;

        private readonly IIptvManager manager;

        private object viewModel;

        private object parametr;

        private Action goToViewModel;

        public SaveStateService(INavigationService navigationService, IIptvManager manager)
        {
            this.navigationService = navigationService;
            this.manager = manager;
        }

        private object SaveOptions
        {
            get
            {
                return ApplicationData.Current.LocalSettings.Values[Constant.OpenPlaylist];
            }
            set
            {
                ApplicationData.Current.LocalSettings.Values[Constant.OpenPlaylist] = value;
            }
        }

        public void ActiveSave(object viewModel)
        {
            this.viewModel = viewModel;

            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += this.OnCloseRequest;
        }

        public void DeactiveSave()
        {
            this.viewModel = null;

            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested -= this.OnCloseRequest;
        }

        public async Task LoadSaveState()
        {

            string saveText = SaveOptions as string ?? String.Empty;
            
            if(saveText != String.Empty && RegexCheck.IsLink(saveText))
            {
                parametr = saveText;

               goToViewModel = () => GoToViewModel<StreamViewModel>();  
            }

            OpenPlaylist openPlaylist = null;

            TryGetOpenPlaylist(saveText, out openPlaylist);

            if (openPlaylist != null)
            {
                ViewModelLocator.Instance.PlayList.SelectedIndex = openPlaylist.SelectedIndex;

                parametr = await manager.GetPlaylistByFileName(openPlaylist.FileName);

                goToViewModel = ()=> GoToViewModel<PlayListViewModel>();
            }

            SaveOptions = null;
        }

        public Task LoadSaveStateAsync()
        {
            return Task.Run(async() => await LoadSaveState());
        }

        public void GoToSaveState()
        {
            if(goToViewModel != null)
            {
                goToViewModel.Invoke();
            }
        }

        private void GoToViewModel<T>()
        {
            if(parametr != null)
            {
                navigationService.Navigate<T>(parametr);
            }
        }

        private void TryGetOpenPlaylist(string saveText, out OpenPlaylist openPlaylist)
        {
            try
            {
                openPlaylist = JsonConvert.DeserializeObject<OpenPlaylist>(saveText);
            }
            catch (JsonReaderException)
            {
                openPlaylist = null;
            }
        }

        private void OnCloseRequest(object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
            var playlistviewmodel = viewModel as PlayListViewModel;

            if(playlistviewmodel != null)
            {
                var openplaylist = new OpenPlaylist()
                {
                    FileName = playlistviewmodel.PlayList.FileName,

                    SelectedIndex = playlistviewmodel.SelectedIndex
                };
                string openplaylistText = JsonConvert.SerializeObject(openplaylist);

                SaveOptions = openplaylistText;
            }
            else
            {
                string saveLink = (viewModel as StreamViewModel).Stream.Uri.AbsoluteUri;

                SaveOptions = saveLink;
            }
        }
    }
}
