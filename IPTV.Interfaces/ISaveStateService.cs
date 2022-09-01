using System.Threading.Tasks;

namespace IPTV.Interfaces
{
    public interface ISaveStateService
    {
        object ParametrToMain { get; }

        void ActiveSave(string menuItem, object viewModel);

        void DeactiveSave();

        void LoadSavedMedia();

        Task LoadSaveMediaAsync();

        void GoToLoadedMedia();
    }
}
