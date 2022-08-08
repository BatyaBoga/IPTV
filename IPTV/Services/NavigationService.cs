using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace IPTV.Services
{
    sealed class NavigationService
    {
        private NavigationService() { }

        private static readonly Lazy<NavigationService> instance =
            new Lazy<NavigationService>(() => new NavigationService());

        public static NavigationService Instance => instance.Value;

        public void Navigate<TViewModel>()
        {
            Navigate<TViewModel>(null);
        }

        public void Navigate<TViewModel>(object parameter)
        {
            var type = DependencyContainer.GetDependecyType(typeof(TViewModel));

            if (type != null) { 

                GetFrame().Navigate(type, parameter);  
            }
        }

        public async Task Refresh<TViewModel>()
        {
            Navigate<TViewModel>(DateTime.Now.Ticks);

            await Task.Delay(100);

            GoBack();
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
