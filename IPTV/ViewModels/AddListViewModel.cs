using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using IPTV.Models;
using IPTV.Services;
using IPTV.Managers;
using IPTV.Constants;
using Windows.ApplicationModel.Resources;

namespace IPTV.ViewModels
{
    public class AddListViewModel : ViewModel
    {
        private readonly ObservableCollection<LinksInfo> links;

        private readonly LinksInfo linkInfoToEdit;

        private readonly ResourceLoader resload;

        private bool isEnabledToEdit;

        private bool saveBtnEnabled;

        private string formTitle;

        private string saveBtn;

        private double opacity;

        public AddListViewModel(ObservableCollection<LinksInfo> links)
        {
            this.links = links;

            linkInfoToEdit = new LinksInfo();

            isEnabledToEdit = true;

            opacity = 1.0;

            resload = ResourceLoader.GetForCurrentView();

            InitializeField("AddPlaylistMsg", "Add");
        }

        public AddListViewModel(ObservableCollection<LinksInfo> links, LinksInfo link) : this(links)
        {
            InitializeField("EditPlaylistMsq", "Save");

            linkInfoToEdit = link.Clone() as LinksInfo;

            IsCorrect();
        }

        public bool IsEnabledToEdit
        {
            get
            {
                return isEnabledToEdit;
            }
            set
            {
                isEnabledToEdit = value;

                OnPropertyChanged();

                OnPropertyChanged("IsRingActive");
            }
        }

        public bool IsRingActive
        {
            get => !IsEnabledToEdit;
        }

        public double Opacity
        {
            get
            {
                return opacity;
            }
            set
            {
                opacity = value;

                OnPropertyChanged();
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
                formTitle = value;

                OnPropertyChanged();
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
                saveBtn = value;

                OnPropertyChanged();
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
                saveBtnEnabled = value;

                OnPropertyChanged();
            }
        }

        public string Title
        {
            get 
            { 
                return linkInfoToEdit.Title; 
            }
            set 
            {
                linkInfoToEdit.Title = value; 

                IsCorrect();

                OnPropertyChanged();
            }
        }

        public string Link
        {
            get 
            { 
                return linkInfoToEdit.Link; 
            }
            set 
            {
                linkInfoToEdit.Link = value;

                IsCorrect();

                OnPropertyChanged();
            }
        }

        public ICommand Save
        {
            get
            {
                return new RelayCommand(async (_) =>
                {
                    LoadState(true);

                    await SaveLink();

                    LoadState(false);
                });
            }
        }

        public ICommand Cancel
        {
            get => new RelayCommand((_) => DialogService.CurrentInstance.CloseDialog());
        }

        private void LoadState(bool state)
        {
            IsEnabledToEdit = !state;

            Opacity = state ? 0.5 : 1;
        }

        private async Task SaveLink()
        {
            var chnangeList = new Action(() =>links.Add(linkInfoToEdit));

            if (linkInfoToEdit.ChannellList != null)
            {
                chnangeList = new Action(()=> links[IndexOfEditLink()] = linkInfoToEdit);
            }

            if (IsUniqueLink(linkInfoToEdit.Link))
            {
                linkInfoToEdit.ChannellList = await ChannelManager.GetChanelsAsync(Link);
            }

            if (linkInfoToEdit.ChannellList != null && linkInfoToEdit.ChannellList.Count > 0)
            {
                chnangeList.Invoke();

                await SaveToFile();

                return;
            }

            await MessageDialogManager.ShowInfoMsg("Failed");
        }

        private bool IsUniqueLink(string link)
        {
            return !(from item in links where item.Link == link select item).Any();
        }

        private int IndexOfEditLink()
        {
            return links.IndexOf((from item in links where item.ChannellList == linkInfoToEdit.ChannellList select item).FirstOrDefault());
        }

        private async Task SaveToFile()
        {
            await DataManager.SaveLinksInfo(new LinksInfoList() { Links = this.links.ToList() });

            await MessageDialogManager.ShowInfoMsg("Successfully");

            DialogService.CurrentInstance.CloseDialog();
        }

        private void IsCorrect()
        {
            SaveBtnEnabled = RegexCheck.IsLink(Link) && RegexCheck.IsTitle(Title);
        }

        private void InitializeField(string title, string save)
        {
            FormTitle = resload.GetString(title);

            SaveBtn = resload.GetString(save);
        }
    }
}
