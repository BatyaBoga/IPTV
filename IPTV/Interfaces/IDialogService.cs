using System.Threading.Tasks;

namespace IPTV.Interfaces
{
    public interface IDialogService
    {
        Task ShowDialog<TViewModel>();

        Task ShowDialog<TViewModel>(params object[] parametr);

        void CloseDialog();
    }
}
