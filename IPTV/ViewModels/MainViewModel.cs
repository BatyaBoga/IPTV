using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using IPTV.Models;
using IPTV.Services;
using IPTV.Managers;
using CommunityToolkit.Mvvm.ComponentModel;

namespace IPTV.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly INavigationService navigation;

        private readonly IDialogService dialogServise;

        private readonly IMessageDialog messageDialog;

        public MainViewModel(INavigationService navigation, IDialogService dialog, IMessageDialog messageDialog)
        {
            Links = new ObservableCollection<LinksInfo>();

            InitializeLinks();

            this.navigation = navigation;

            this.dialogServise = dialog;

            this.messageDialog = messageDialog;
        }

        public static ObservableCollection<LinksInfo> Links { get; set; }

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
                return new RelayCommand(async (link) =>
                {
                    var linksInfoElement = GetLinksInfoElementBylink(link);

                    linksInfoElement.ChannellList = await ChannelManager.GetChanelsAsync(link.ToString());

                    await messageDialog.ShowInfoMsg("UpdateMsg");
                });
            }
        }

        public ICommand EditLink
        {
            get
            {
                return new RelayCommand(async (link) =>
                {
                    await dialogServise.ShowDialog<AddListViewModel>(GetLinksInfoElementBylink(link));
                });
            }
        }

        public ICommand DeleteLinks
        {
            get
            {
                return new RelayCommand(async(link) =>
                {  
                    await messageDialog.ShureMsg("DeleteMsg", async(_) => await DeletePlaylist(link));
                });
            }
        }

        public ICommand AddLinks
        {
            get => new RelayCommand(async(_) => await dialogServise.ShowDialog<AddListViewModel>());
        }

        public ICommand OpenOptions
        {
            get => new RelayCommand((_) => navigation.Navigate<OptionsViewModel>());   
        }

        private void OpenPlaylist(int selectedindex)
        {  
            navigation.Navigate<PlayListViewModel>(Links[selectedindex]);
        }

        private async Task DeletePlaylist(object link)
        {
           Links.Remove(GetLinksInfoElementBylink(link));
               
           await DataManager.SaveLinksInfo(new LinksInfoList() { Links = Links.ToList() });
        }

        private LinksInfo GetLinksInfoElementBylink(object bindObject)
        {
            return (from item in Links where item.Link == bindObject.ToString() select item).FirstOrDefault();
        }

        private void InitializeLinks()
        {
            Task.Run(async () =>
            {
                var linkslist = await DataManager.GetLinksInfo();

                if (linkslist.Links != null)
                {
                    foreach (var item in linkslist.Links)
                    {
                        item.ChannellList = await ChannelManager.GetChanelsAsync(item.Link);
                        Links.Add(item);
                    }
                }
            }).Wait();
        }
    }
}
