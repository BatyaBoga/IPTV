using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using IPTV.Models;
using IPTV.Service;
using Windows.UI.Popups;
using Windows.ApplicationModel.Resources;

namespace IPTV.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private ObservableCollection<LinksInfo> links = new ObservableCollection<LinksInfo>();

        private ResourceLoader resourceLoader;

        private int selectedIndex;

        public MainViewModel()
        {
            Task.Run(async () =>
            {
                
                var linkslist =  await DataManager.GetLinksInfo();

                if (linkslist.Links != null)
                {
                    foreach (var item in linkslist.Links)
                    {
                        item.ChannellList = await ChannelManager.GetChanelsAsync(item.Link);
                        links.Add(item);
                    }
                }


            }).Wait();

            selectedIndex = -1;

            resourceLoader = ResourceLoader.GetForCurrentView();
        }
        public ObservableCollection<LinksInfo> Links
        {
            get { return links; }
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
                return this.selectedIndex;
            }

            set
            {
                this.selectedIndex = value;
                OpenPlaylist();
                OnPropertyChanged();
            }
        }
        public ICommand UpdateLinks
        {
            get
            {
                return new RelayCommand(async (obj) =>
                {
                    var linksInfoElement = GetLinksInfoElementBylink(obj);

                    if(linksInfoElement != null)
                    {
                        linksInfoElement.ChannellList = await ChannelManager.GetChanelsAsync(linksInfoElement.Link);

                        var dialog = new MessageDialog(resourceLoader.GetString("UpdateMsg"));
                        dialog.Commands.Add(new UICommand("OK"));
                        await dialog.ShowAsync();
                    }
                   
                    OnPropertyChanged("Links");
                });
            }
        }
        public ICommand EditLink
        {
            get
            {
                return new RelayCommand(async (obj) =>
                {
                    var linksInfoElement = GetLinksInfoElementBylink(obj);
                    var LinkToEditId = links.IndexOf(linksInfoElement);
                    await DialogService.CurrentInstance.ShowDialog<AddListViewModel>(links, LinkToEditId);
                });
            }
        }
        public ICommand DeleteLinks
        {
            get
            {
                return new RelayCommand(async(obj) =>
                {  
                    var dialog = new MessageDialog(resourceLoader.GetString("DeleteMsg"));

                    dialog.Commands.Add(new UICommand(resourceLoader.GetString("Yes"), (_)=>DeletePlaylist(obj)));
                    dialog.Commands.Add(new UICommand(resourceLoader.GetString("No")));
                    await dialog.ShowAsync();

                    OnPropertyChanged("Links");

                });
            }
        }
        public ICommand AddLinks
        {
            get
            {
                return new RelayCommand(async(_) => await DialogService.CurrentInstance.ShowDialog<AddListViewModel>(links));
            }
        }
        public ICommand OpenOptions
        {
            get
            {
                return new RelayCommand((_) => NavigationService.Instance.Navigate(typeof(OptionsViewModel)));
            }
        }
        private void OpenPlaylist()
        {  
            NavigationService.Instance.Navigate(typeof(PlayListViewModel), links[selectedIndex]);
        }
        private async void DeletePlaylist(object obj)
        {
            var linksInfoElement = GetLinksInfoElementBylink(obj);

            if (linksInfoElement != null)
            {
                links.Remove(linksInfoElement);
            } 
               
            await DataManager.SaveLinksInfo(new LinksInfoList() { Links = this.links.ToList() });
        }
        private LinksInfo GetLinksInfoElementBylink(object bindObject)
        {
            var linksInfoElement = (from item in links where item.Link == bindObject.ToString() select item).FirstOrDefault();
            return linksInfoElement;
        }

    }
}
