using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Windows.Storage;
using IPTV.Models.Model;

namespace IPTV.Models.Interfaces
{
    public interface IMediaFile
    {
        Task<ObservableCollection<LocalChannel>> LoadAllLocalVideo();

        Task<bool> AddFile(ObservableCollection<LocalChannel> localChannels);

        Task DeleteFile(ObservableCollection<LocalChannel> localChannels, StorageFile file);
    }
}
