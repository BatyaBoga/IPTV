using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using IPTV.Interfaces;

namespace IPTV.Services
{
    public class DialogService : IDialogService
    {
        private ContentDialog dialog;

        public async Task ShowDialog<TViewModel>(params object[] parametr)
        {
            var type = DependencyTypeContainer.GetDependecyType(typeof(TViewModel));

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
