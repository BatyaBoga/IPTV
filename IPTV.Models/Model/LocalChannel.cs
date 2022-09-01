using Windows.Storage;

namespace IPTV.Models.Model
{
    public class LocalChannel
    {
        public StorageFile LocalFile { get; set; }

        public bool CanDelete { get; set; }
    }
}
