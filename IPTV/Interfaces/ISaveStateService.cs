using System.Threading.Tasks;

namespace IPTV.Interfaces
{
    public interface ISaveStateService
    {
        void ActiveSave(object viewModel);

        void DeactiveSave();

        Task LoadSaveState();

        Task LoadSaveStateAsync();

        void GoToSaveState();
    }
}
