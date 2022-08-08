using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace IPTV.Services
{
    sealed class DialogService 
    {
        private readonly static Lazy<DialogService> lazyInstance = 
            new Lazy<DialogService>(() => new DialogService(), true);

        public static DialogService CurrentInstance => lazyInstance.Value;

        private DialogService() { }

        private ContentDialog dialog;

        public async Task ShowDialog<TViewModel>(params object[] parametr)
        {
            var type = DependencyContainer.GetDependecyType(typeof(TViewModel));

            if(type != null)
            {
               dialog = Activator.CreateInstance(type, parametr) as ContentDialog;

               if(dialog != null)
               {
                   await dialog.ShowAsync();
               }  
            }
        }

        public async Task ShowDialog<TViewModel>()
        {
            await ShowDialog<TViewModel>(null);
        }

        public void CloseDialog()
        {
            if (dialog != null)
            {
                dialog.Hide();
            }
        }
    }
}
