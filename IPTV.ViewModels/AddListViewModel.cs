using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Resources;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IPTV.Interfaces;
using IPTV.Models.Model;
using IPTV.Models.Interfaces;


namespace IPTV.ViewModels
{
    public class AddListViewModel : ObservableObject
    {
        private readonly ResourceLoader resload;

        private readonly IMessageDialog messageDialog;

        private readonly IDialogService dialogService;

        private readonly IIptvManager manager;

        private readonly IRegexCheck regex;

        private ObservableCollection<Playlist> playlistCollection;

        private string oldlink;

        private string title;

        private string link;

        private bool isEnabledToEdit;

        private bool saveBtnEnabled;

        private string formTitle;

        private string saveBtn;

        private double opacity;

        public AddListViewModel(IDialogService dialogService, IMessageDialog messageDialog, IIptvManager manager, IRegexCheck regex)
        {
            this.dialogService = dialogService;

            this.messageDialog = messageDialog;

            this.manager = manager;

            this.regex = regex;

            isEnabledToEdit = true;

            opacity = 1.0;

            resload = ResourceLoader.GetForCurrentView();

            playlistCollection = RemoteListViewModel.PlaylistCollection;
        }

        public bool IsEnabledToEdit
        {
            get
            {
                return isEnabledToEdit;
            }
            set
            {
                if(SetProperty(ref isEnabledToEdit, value))
                OnPropertyChanged(nameof(IsRingActive));
            }
        }

        public bool IsRingActive => !IsEnabledToEdit;

        public ICommand Cancel => new RelayCommand(() => dialogService.CloseDialog());

        public double Opacity
        {
            get
            {
                return opacity;
            }
            set
            {
                SetProperty(ref opacity, value);
            }
        }

        public string FormTitle
        {
            get
            {
                return formTitle;
            }
            set
            {
                SetProperty(ref formTitle, value);
            }
        }

        public string SaveBtn
        {
            get 
            { 
                return saveBtn; 
            }
            set
            {
                SetProperty(ref saveBtn, value);
            }
        }

        public bool SaveBtnEnabled
        {
            get 
            {
                return saveBtnEnabled; 
            }
            set 
            {
                SetProperty(ref saveBtnEnabled, value);
            }
        }

        public string Title
        {
            get 
            { 
                return title; 
            }
            set 
            {
                if(SetProperty(ref title, value))
                {   
                    IsCorrect();
                }
            }
        }

        public string Link
        {
            get 
            { 
                return link; 
            }
            set 
            {
                if(SetProperty(ref link, value))
                {  
                    IsCorrect();
                }
            }
        }

        public ICommand Save
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    LoadState(true);

                    await SaveEdit();

                    LoadState(false);
                });
            }
        }


        public void ConfigureToAdd()
        {
            InitializeField("AddPlaylistMsg", "Add");

            title = string.Empty;

            link = oldlink = string.Empty;

            SaveBtnEnabled = false;
        }

        public void ConfigureToEdit(Playlist playlist)
        {
            InitializeField("EditPlaylistMsq", "Save");

            title = playlist.PlaylistTitle;

            link  = oldlink = playlist.Link;

            IsCorrect();
        }

        private void LoadState(bool state)
        {
            IsEnabledToEdit = !state;

            Opacity = state ? 0.5 : 1;
        }

        private async Task SaveEdit()
        {
            var newplaylist = await manager.CreatePlaylist(title, link);

            if (newplaylist.ChannelList != null && newplaylist.ChannelList.Count > 0)
            {
                if(oldlink == String.Empty && IsUniqueLink(link))
                {
                    await AddPlaylist(newplaylist);
                }
                else if(oldlink != link && IsUniqueLink(link) || oldlink == link)
                {
                    await EditPlaylist(newplaylist);
                }
                else
                {
                    await messageDialog.ShowInfoMsg("NotUnique");
                }
            }
            else
            {
                await messageDialog.ShowInfoMsg("NoChannels");
            }
        }

        private async Task AddPlaylist(Playlist playlist)
        {
            await manager.AddPlayList(playlistCollection, playlist);

            await GoodMessageClose();
        }

        private async Task EditPlaylist(Playlist playlist)
        {
            await manager.EditPlaylist(playlistCollection, playlist, IndexOfEditPlaylist());

            await GoodMessageClose();
        }

        private async Task GoodMessageClose()
        {
            await messageDialog.ShowInfoMsg("Successfully");

            dialogService.CloseDialog();
        }

        private bool IsUniqueLink(string link)
        {
            return !(from item in playlistCollection where item.Link == link select item).Any();
        }

        private int IndexOfEditPlaylist()
        {
            return playlistCollection.IndexOf((from item in playlistCollection where 
                                               item.Link == oldlink select item)
                                               .FirstOrDefault());
        }

        private void IsCorrect()
        {
            SaveBtnEnabled = regex.IsLink(Link) && regex.IsTitle(Title);
        }

        private void InitializeField(string title, string save)
        {
            FormTitle = resload.GetString(title);

            SaveBtn = resload.GetString(save);
        }
    }
}
