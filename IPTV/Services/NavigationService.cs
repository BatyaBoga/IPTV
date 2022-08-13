using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using IPTV.Interfaces;

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

        public async Task Refresh<TViewModel>()
        {
            suppress = new SuppressNavigationTransitionInfo();

            Navigate<TViewModel>();

            await Task.Delay(100);

            GoBack();

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
            var frame = Window.Current.Content as Frame;

            if(frame != null)
            {
                return frame;
            }
            else
            {
                throw new InvalidCastException("Current content is not a frame.");
            }
        }
    }
}
