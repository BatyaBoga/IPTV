using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using IPTV.Models;
using IPTV.Services;
using IPTV.Managers;

namespace IPTV.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private static ObservableCollection<LinksInfo> links = new ObservableCollection<LinksInfo>();

        private int selectedIndex;

        public MainViewModel()
        {
            InitializeLinks();

            selectedIndex = -1;
        }

        public ObservableCollection<LinksInfo> Links
        {
            get 
            { 
                return links; 
            }
            set 
            { 
                links = value; 

                OnPropertyChanged();
            }
        }

        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                selectedIndex = value;

                OpenPlaylist();
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

                    await MessageDialogManager.ShowInfoMsg("UpdateMsg");
                    
                    OnPropertyChanged("Links");
                });
            }
        }

        public ICommand EditLink
        {
            get
            {
                return new RelayCommand(async (link) =>
                {
                    await DialogService.CurrentInstance.ShowDialog<AddListViewModel>(links, GetLinksInfoElementBylink(link));
                });
            }
        }

        public ICommand DeleteLinks
        {
            get
            {
                return new RelayCommand(async(link) =>
                {  
                    await MessageDialogManager.ShureMsg("DeleteMsg", (_) => DeletePlaylist(link));

                    OnPropertyChanged("Links");
                });
            }
        }

        public ICommand AddLinks
        {
            get => new RelayCommand(async(_) => await DialogService.CurrentInstance.ShowDialog<AddListViewModel>(links));
        }

        public ICommand OpenOptions
        {
            get => new RelayCommand((_) => NavigationService.Instance.Navigate<OptionsViewModel>());   
        }

        private void OpenPlaylist()
        {  
            NavigationService.Instance.Navigate<PlayListViewModel>(links[selectedIndex]);
        }

        private async void DeletePlaylist(object link)
        {
           links.Remove(GetLinksInfoElementBylink(link));
               
           await DataManager.SaveLinksInfo(new LinksInfoList() { Links = links.ToList() });
        }

        private LinksInfo GetLinksInfoElementBylink(object bindObject)
        {
            return (from item in links where item.Link == bindObject.ToString() select item).FirstOrDefault();
        }

        private void InitializeLinks()
        {
            if (links.Count > 0) return;

            Task.Run(async () =>
            {
                var linkslist = await DataManager.GetLinksInfo();

                if (linkslist.Links != null)
                {
                    foreach (var item in linkslist.Links)
                    {
                        item.ChannellList = await ChannelManager.GetChanelsAsync(item.Link);
                        links.Add(item);
                    }
                }
            }).Wait();
        }
    }
}
