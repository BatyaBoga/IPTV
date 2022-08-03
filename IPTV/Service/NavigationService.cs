using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;


namespace IPTV.Service
{

    sealed class NavigationService
    {
        private NavigationService() { }

        private static readonly Lazy<NavigationService> instance =
            new Lazy<NavigationService>(() => new NavigationService());

        public static NavigationService Instance => instance.Value;

        public static Dictionary<Type, Type> Map = new Dictionary<Type, Type>();

        public void Navigate(Type sourcePage)
        {
            if (Map.Count > 0 && Map.ContainsKey(sourcePage))
            {
                var frame = (Frame)Window.Current.Content;
                frame.CacheSize = 0;
                frame.Navigate(Map[sourcePage]);
            }
        }

        public void Navigate(Type sourcePage, object parameter)
        {
            if (Map.Count > 0 && Map.ContainsKey(sourcePage))
            {  
                var frame = (Frame)Window.Current.Content;
                frame.Navigate(Map[sourcePage], parameter);
            }
        }

        public async Task Refresh(Type sourcePage)
        {
            this.Navigate(sourcePage, DateTime.Now.Ticks);
            await Task.Delay(100);
            this.GoBack();
        }

        public void GoBack()
        {
            var frame = (Frame)Window.Current.Content;
            frame.CacheSize=0;

            if (frame.CanGoBack)
            {
                frame.GoBack();
            }

        }

    }




}
