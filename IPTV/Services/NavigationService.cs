using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using IPTV.Interfaces;
using IPTV.Constants;
using IPTV.ViewModels;

namespace IPTV.Services
{
    public class NavigationService : INavigationService
    {
        private SuppressNavigationTransitionInfo suppress = null;

        public void Navigate<TViewModel>()
        {
            Navigate<TViewModel>(null);
        }

        public void Navigate<TViewModel>(object parameter)
        {
            var type = DependencyTypeContainer.GetDependecyType(typeof(TViewModel));

            if (type != null) { 

                GetFrame().Navigate(type, parameter, suppress);  
            }
        }

        public void NavigateToFrame<TViewModel>()
        {
            var type = DependencyTypeContainer.GetDependecyType(typeof(TViewModel));

            var homePage = GetFrame().Content as MainPage;

            if(homePage != null)
            {
                 homePage.NavigationFrame.Navigate(type, null, suppress);
            }
        }

        public async Task Refresh<TViewModel>()
        {
            suppress = new SuppressNavigationTransitionInfo();

            Navigate<MainViewModel>();

            Navigate<MainViewModel>(Constant.Options);

            await Task.Delay(100);

            GoBack();

            NavigateToFrame<TViewModel>();

            suppress = null;
        }

        public void GoBack()
        {
            var frame = GetFrame();

            if (frame.CanGoBack)
            {
                frame.GoBack();
            }
        }

        private Frame GetFrame()
        {
            var rootframe = Window.Current.Content as Frame;

            if(rootframe != null)
            {
                return rootframe;
            }
            else
            {
                throw new InvalidCastException("Current content is not a frame.");
            }
        }
    }
}
