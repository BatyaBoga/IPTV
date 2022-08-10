using System.Threading.Tasks;

namespace IPTV.Services
{
    public interface IDialogService
    {
        Task ShowDialog<TViewModel>();

        Task ShowDialog<TViewModel>(params object[] parametr);

        void CloseDialog();
    }
}
