using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPTV.ViewModels;
using IPTV.Models;
using Windows.UI.Xaml.Controls;
using System.Threading;
using System.Reflection;
using IPTV.Views;
using Windows.UI.Xaml;


namespace IPTV.Service
{
    public class NavigationService
    {
        private readonly static Lazy<NavigationService> lazyInstance = new Lazy<NavigationService>(() => new NavigationService(), true);

        public static NavigationService CurrentInstance
        {
            get
            {
                return lazyInstance.Value;
            }
        }

        private NavigationService() { }

        public Frame NavigationFrame { get; set; }

        private Dictionary<string, Type> pages = new Dictionary<string, Type>();

        public void GoBack()
        {
            if (NavigationFrame.CanGoBack)
            {
                NavigationFrame.GoBack();
            }
        }

        public void NavigateTo(string pageKey)
        {
            this.NavigateTo(pageKey, (object)null);
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            Dictionary<string, Type> pagesByKey = this.pages;
            bool lockTaken = false;
            try
            {
                Monitor.Enter((object)pagesByKey, ref lockTaken);
                Configure(pageKey);
                //this.NavigationFrame.Navigate(this.pages[pageKey], parameter);
                this.NavigationFrame.Navigate(typeof(PlayListView), parameter);
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit((object)pagesByKey);
            }
        }
        private void Configure(string pageName)
        {
            Dictionary<string, Type> pagesByKey = this.pages;
            if (string.IsNullOrEmpty(pageName))
                throw new ArgumentException("The page key cannot be empty: ");
            if (!this.pages.ContainsKey(pageName))
            {
                try
                {
                    StringBuilder name = new StringBuilder();
                    name.Append("YourPageNamespace");
                    name.Append(pageName);
                    Assembly currentAssm = this.GetType().GetTypeInfo().Assembly;
                    Type pageType = currentAssm.GetType(name.ToString());
                    this.pages.Add(pageName, pageType);
                }
                catch
                {
                    throw new ArgumentException(string.Format("Can not map {0} page name to actual type", pageName), "pageName");
                }
            }
        }

    }



    sealed class NS
    {

        public static Dictionary<Type, Type> typeMap = new Dictionary<Type, Type>();

        public void Navigate(Type sourcePage)
        {
            if (typeMap.Count > 0 && typeMap.ContainsKey(sourcePage))
            {
                var frame = (Frame)Window.Current.Content;
                frame.Navigate(typeMap[sourcePage]);
            }
        }

        public void Navigate(Type sourcePage, object parameter)
        {
            if (typeMap.Count > 0 && typeMap.ContainsKey(sourcePage))
            {  
                var frame = (Frame)Window.Current.Content;
                frame.Navigate(typeMap[sourcePage], parameter);
            }
        }

        public void GoBack()
        {
            var frame = (Frame)Window.Current.Content;

            if (frame.CanGoBack)
            {
                frame.GoBack();
            }

        }

        private NS() { }

        private static readonly Lazy<NS> instance =
            new Lazy<NS>(() => new NS());

        public static NS Instance => instance.Value;
    }




}
