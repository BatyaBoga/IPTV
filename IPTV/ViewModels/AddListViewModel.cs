using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using IPTV.Models;
using IPTV.Services;
using IPTV.Managers;
using Windows.ApplicationModel.Resources;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace IPTV.ViewModels
{
    public class AddListViewModel : ObservableObject
    {
        private readonly ResourceLoader resload;

        private readonly IMessageDialog messageDialog = new MessageDialogManager();

        private readonly IDialogService dialogService = new DialogService();

        private ObservableCollection<LinksInfo> links;

        private LinksInfo linkInfoToEdit;

        private bool isEnabledToEdit;

        private bool saveBtnEnabled;

        private string formTitle;

        private string saveBtn;

        private double opacity;

        public AddListViewModel(IDialogService dialogService, IMessageDialog messageDialog)
        {
            this.dialogService = dialogService;

            this.messageDialog = messageDialog;

            isEnabledToEdit = true;

            opacity = 1.0;

            resload = ResourceLoader.GetForCurrentView();

            links = MainViewModel.Links;
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
                return linkInfoToEdit.Title; 
            }
            set 
            {
                if(linkInfoToEdit.Title != value)
                {
                    linkInfoToEdit.Title = value;
                    
                    IsCorrect();

                    OnPropertyChanged();
                }
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
                if(linkInfoToEdit.Link != value)
                {
                    linkInfoToEdit.Link = value;
                
                    IsCorrect();

                    OnPropertyChanged();
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

                    await SaveLink();

                    LoadState(false);
                });
            }
        }

        public ICommand Cancel
        {
            get => new RelayCommand(() => dialogService.CloseDialog());
        }

        public void ConfigureToAdd()
        {
            InitializeField("AddPlaylistMsg", "Add");

            linkInfoToEdit = new LinksInfo();
        }

        public void ConfigureToEdit(LinksInfo link)
        {
            InitializeField("EditPlaylistMsq", "Save");

            linkInfoToEdit = link.Clone() as LinksInfo;

            IsCorrect();
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

            await messageDialog.ShowInfoMsg("Failed");
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

            await messageDialog.ShowInfoMsg("Successfully");

            dialogService.CloseDialog();
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
