using Windows.UI.Xaml.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using IPTV.Interfaces;
using IPTV.Constants;

namespace IPTV.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly INavigationService navigation;

        private object selectedItem;

        public MainViewModel(INavigationService navigation)
        {
            this.navigation = navigation;
        }

        public object SelectedItem
        {
            get 
            { 
                return selectedItem; 
            }
            set 
            { 
                if(value != null && SetProperty(ref selectedItem, value))
                {
                    GoToSelectedItem();
                } 
            }
        }

        public void GoToSelectedItem()
        {
            string tag = (selectedItem as NavigationViewItem).Tag as string;

            switch (tag)
            {
                case Constant.Remote: navigation.NavigateToFrame<RemoteListViewModel>(); break;

                case Constant.Local: navigation.NavigateToFrame<LocalListViewModel>(); break;

                default: navigation.NavigateToFrame<OptionsViewModel>(); break;
            }
        }
    }
}
