using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using IPTV.Models;
using IPTV.Service;
using Windows.UI.Popups;

namespace IPTV.ViewModels
{
    class AddListViewModel : ViewModel
    {
        private bool saveBtnEnabled;

        private string link;

        private string title;

        private int linksInfoToEditId;

        private string formTitle;

        private string saveBtn;

        private ObservableCollection<LinksInfo> links;

        public AddListViewModel(ObservableCollection<LinksInfo> links)
        {
            saveBtnEnabled = false;
            FormTitle = "Add PlayList";
            SaveBtn = "Add";
            this.links = links;
            linksInfoToEditId = -1;
        }

        public AddListViewModel(ObservableCollection<LinksInfo> links, int LinksInfoToEditId)
        {
            saveBtnEnabled = true;
            this.links = links;
            this.linksInfoToEditId = LinksInfoToEditId;
            link = links[LinksInfoToEditId].Link;
            title = links[LinksInfoToEditId].Title;
            FormTitle = "Edit PlayList";
            SaveBtn = "Save";
        }

        public string FormTitle
        {
            get
            {
                return formTitle;
            }
            set
            {
                formTitle = value;
                OnPropertyChanged();
            }
        }

        public string SaveBtn
        {
            get { return saveBtn; }
            set
            {
                saveBtn = value;
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get { return title; }
            set { 
                title = value; 
                IsCorrect();
                OnPropertyChanged();
            }
        }

        public string Link
        {
            get { return link; }
            set 
            { 
                link = value;
                IsCorrect();
                OnPropertyChanged();
            }
        }

        public bool SaveBtnEnabled
        {
            get { return saveBtnEnabled; }
            set 
            {
                saveBtnEnabled = value;
                OnPropertyChanged();
            }
        }

        public ICommand Save
        {
            get
            {
                return new RelayCommand(async (_) =>
                {

                    var linksInfo = new LinksInfo()
                    {
                        Link = link,
                        Title = title,
                        channellList = await ChannelManager.GetChanelsAsync(Link)
                    };
                   


                    await SaveLink(linksInfo);

                });
            }
        }


        public ICommand Cancel
        {
            get
            {
                return new RelayCommand((_) =>
                {
                    DialogService.CurrentInstance.CloseDialog();
                });
            }
        }

        private async Task SaveLink(LinksInfo linksInfo)
        {
            

            if (linksInfo.channellList.Count > 0)
            {
                if (linksInfoToEditId >= 0)
                {
                    links[linksInfoToEditId] = linksInfo;
                    await SaveToFile();
                    return;
                }
                else if((from item in links where item.Link == linksInfo.Link select item).Count() == 0 )
                {
                    links.Add(linksInfo);
                    await SaveToFile();
                    return;
                }
            }

            await new MessageDialog("Failed").ShowAsync();

        }

        private bool IsLink(string link)
        {
            string regex = @"https?:\/\/[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*).m3u";
            return Regex.IsMatch(link, regex);
        }

        private void IsCorrect()
        {
            if (IsLink(Link) && Title !="")
            {
                SaveBtnEnabled = true;
            }
            else
            {
                SaveBtnEnabled = false;
            }

            
        }

        private async Task SaveToFile()
        {
            await new MessageDialog("Successfully").ShowAsync();

            await DataManager.SaveLinksInfo(new LinksInfoList() { links = this.links.ToList() });

            DialogService.CurrentInstance.CloseDialog();
        }
    }
}
