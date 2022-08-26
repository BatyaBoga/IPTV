using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IPTV.Models.Model;
using IPTV.Interfaces;

namespace IPTV.ViewModels
{
    public class RemoteListViewModel : ObservableObject
    {
        private readonly INavigationService navigation;

        private readonly IDialogService dialogServise;

        private readonly IMessageDialog messageDialog;

        private readonly IIptvManager manager;

        public RemoteListViewModel(INavigationService navigation, IDialogService dialog, IMessageDialog messageDialog, IIptvManager manager)
        {
            PlaylistCollection = new ObservableCollection<Playlist>();

            this.manager = manager;

            InitializeLinks();

            this.navigation = navigation;

            this.dialogServise = dialog;

            this.messageDialog = messageDialog;
        }

        public static ObservableCollection<Playlist> PlaylistCollection { get; set; }

        public int SelectedIndex
        {
            set
            {
                OpenPlaylist(value);
            }
        }

        public ICommand UpdateLinks
        {
            get
            {
                return new RelayCommand<string>(async (link) =>
                {
                    string msg = await manager.UpdatePlaylist(GetPlaylistBylink(link)) ? "UpdateMsg" : "Failed";

                    await messageDialog.ShowInfoMsg(msg);

                });
            }
        }

        public ICommand EditLink
        {
            get
            {
                return new RelayCommand<string>(async (link) =>
                {
                    await dialogServise.ShowDialog<AddListViewModel>(GetPlaylistBylink(link));
                });
            }
        }

        public ICommand DeleteLinks
        {
            get
            {
                return new RelayCommand<string>(async (link) =>
                {
                    await messageDialog.ShureMsg("DeleteMsg", async (_) =>
                    {
                        await manager.DeletePlaylist(PlaylistCollection, GetPlaylistBylink(link));
                    });
                });
            }
        }

        public ICommand AddLinks => new RelayCommand(async () => await dialogServise.ShowDialog<AddListViewModel>());
 
        public ICommand OpenOptions => new RelayCommand(() => navigation.Navigate<OptionsViewModel>());

        private void OpenPlaylist(int selectedindex)
        {
            var playlist = PlaylistCollection[selectedindex];

            if (playlist.ChannelList.Count > 1)
            {
                navigation.Navigate<PlayListViewModel>(playlist);
            }
            else
            {
                navigation.Navigate<StreamViewModel>(playlist.Link);
            }
        }

        private Playlist GetPlaylistBylink(object bindObject)
        {
            return (from item in PlaylistCollection where item.Link == bindObject.ToString() select item).FirstOrDefault();
        }

        private void InitializeLinks()
        {
            Task.Run(async () =>
            {
                var list = new List<Playlist>();

                list = await manager.GetPlaylistCollection();

                foreach (var item in list)
                {
                    PlaylistCollection.Add(item);
                }
            }).Wait();
        }
    }
}
