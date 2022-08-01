using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using IPTV.Models;
using IPTV.Service;
using Windows.UI.Popups;
using IPTV.Views;
using Windows.UI.Xaml.Input;

namespace IPTV.ViewModels
{
    public class MainViewModel : ViewModel
    {

        private ObservableCollection<LinksInfo> links = new ObservableCollection<LinksInfo>();

        public int selectedIndex;


        public MainViewModel()
        {
            Task.Run(async () =>
            {
                
                var linkslist =  await DataManager.GetLinksInfo();

                if (linkslist.links != null)
                {
                    foreach (var item in linkslist.links)
                    {
                        item.channellList = await ChannelManager.GetChanelsAsync(item.Link);
                        links.Add(item);
                    }
                }


            }).Wait();

            selectedIndex = -1;
            
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
                    LinksInfo linksInfoElement = GetLinksInfoElementBylink(obj);

                    if(linksInfoElement != null)
                    {
                        linksInfoElement.channellList = await ChannelManager.GetChanelsAsync(linksInfoElement.Link);
                        var dialog = new MessageDialog("Playlist updated");
                        await dialog.ShowAsync();
                    }
                   
                    OnPropertyChanged("Links");
                });
            }
        }

        public ICommand DeleteLinks
        {
            get
            {
                return new RelayCommand(async(obj) =>
                {

                    
                    var dialog = new MessageDialog("Do you realy whant to deleat");

                    dialog.Commands.Add(new UICommand("Yes", async (_) => {
                        LinksInfo linksInfoElement = GetLinksInfoElementBylink(obj);
                        if (linksInfoElement != null) links.Remove(linksInfoElement);
                        await DataManager.SaveLinksInfo(new LinksInfoList() { links = this.links.ToList() });
                    }));

                    dialog.Commands.Add(new UICommand("No"));

                    await dialog.ShowAsync();
                    OnPropertyChanged("Links");

                });
            }
        }


        public  ICommand AddLinks
        {
            get
            {
                return new RelayCommand(async(_) =>
                {
                    await DialogService.CurrentInstance.ShowDialog<AddListViewModel>(links);
                });
            }
        }

        public ICommand EditLink
        {
            get
            {
                return new RelayCommand(async (obj) =>
                {
                    LinksInfo linksInfoElement = GetLinksInfoElementBylink(obj);
                    var LinkToEditId = links.IndexOf(linksInfoElement);
                    await DialogService.CurrentInstance.ShowDialog<AddListViewModel>(links, LinkToEditId);
                });
            }
        }

        private void OpenPlaylist()
        {
            var linkse = links[selectedIndex].channellList;
            //NavigationService.CurrentInstance.NavigateTo("PlayListView", linkse);

            NS.Instance.Navigate(typeof(PlayListViewModel), linkse);

            //NS.Instance.Navigate(typeof(PlayListView), linkse);
        }

        private LinksInfo GetLinksInfoElementBylink(object bindObject)
        {
            var linksInfoElement = (from item in links where item.Link == bindObject.ToString() select item).FirstOrDefault();
            return linksInfoElement;
        }



    }
}
