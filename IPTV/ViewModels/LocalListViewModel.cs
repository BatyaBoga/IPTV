using System.IO;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Windows.Storage;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using IPTV.Interfaces;
using IPTV.Models.Model;

namespace IPTV.ViewModels
{
    public class LocalListViewModel : ObservableObject
    {
        private readonly INavigationService navigation;

        private readonly IMediaFile mediaFile;

        private readonly IMessageDialog messageDialog;

        public LocalListViewModel(INavigationService navigation, IMediaFile mediaFile, IMessageDialog messageDialog)
        {
            this.navigation = navigation;

            this.mediaFile = mediaFile;

            this.messageDialog = messageDialog;

            Task.Run(async () => LocalChannels = await mediaFile.LoadAllLocalVideo()).Wait();
        }

        public ObservableCollection<LocalChannel> LocalChannels { get; set; }

        public int SelectedIndex
        {
            set
            {
                if(value >= 0)
                {
                    navigation.Navigate<StreamViewModel>(LocalChannels[value].LocalFile);
                } 
            }
        }

        public ICommand DeleteFile
        {
            get
            {
                return new RelayCommand<StorageFile>(async(file) => {

                    await messageDialog.ShureMsg("DeleteMsg", async (_) =>
                    {
                       await mediaFile.DeleteFile(LocalChannels, file);
                    });
                });
            }
        }
        
        public ICommand AddLocalFile
        {
            get
            {
                return new RelayCommand(async () => 
                {
                    try
                    {
                        if (await mediaFile.AddFile(LocalChannels))
                        {
                            await messageDialog.ShowInfoMsg("Successfully");
                        }
                    }
                    catch (FileLoadException ex)
                    {
                        await messageDialog.ShowInfoMsg(ex.Message);
                    }
                });
            }
        }
    }
}
