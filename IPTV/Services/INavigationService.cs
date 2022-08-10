using System.Threading.Tasks;

namespace IPTV.Services
{
    public interface INavigationService
    {
        void Navigate<TViewModel>(object parameter);

        void Navigate<TViewModel>();
        void GoBack();

        Task Refresh<TVIewModel>();
    }
}
