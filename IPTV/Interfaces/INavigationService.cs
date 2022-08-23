using System.Threading.Tasks;

namespace IPTV.Interfaces
{
    public interface INavigationService
    {
        void Navigate<TViewModel>(object parameter);

        void Navigate<TViewModel>();

        void GoBack();

        Task Refresh<TVIewModel>();
    }
}
