using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace IPTV.Interfaces
{
    public interface INavigationService
    {
        Frame HomeFrame { get; set; }

        void Navigate<TViewModel>(object parameter);

        void Navigate<TViewModel>();

        void NavigateToFrame<TViewModel>();

        void GoBack();

        Task Refresh<TVIewModel>();
    }
}
