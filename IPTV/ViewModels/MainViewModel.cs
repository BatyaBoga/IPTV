using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using IPTV.Models;
using Windows.UI.Popups;

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
                var linkslist = await DataManager.GetLinksInfo();

                foreach (var item in linkslist.links)
                {
                    item.channellList = await ChannelManager.GetChanelsAsync(item.Link);
                    links.Add(item);
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
                OnPropertyChanged();
            }
        }

        public LinksInfo SelectedLink
        {
            get { return (selectedIndex >= 0) ? links[selectedIndex] : null; }
        }
        

        public ICommand UpdateLinks
        {
            get
            {
                return new RelayCommand(async (_) =>
                {
                    foreach (var item in links)
                    {
                        item.channellList = await ChannelManager.GetChanelsAsync(item.Link);
                    }
                    var dialog = new MessageDialog("Hi!");
                    await dialog.ShowAsync();

                });
            }
        }

        public ICommand DeleteLinks
        {
            get
            {
                return new RelayCommand((_) =>
                {
                  
                    links.Remove(SelectedLink);

                });
            }
        }




    }
}
