using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace IPTV.Service
{
    public class DialogService 
    {

        private readonly static Lazy<DialogService> lazyInstance = 
            new Lazy<DialogService>(() => new DialogService(), true);

        public static DialogService CurrentInstance => lazyInstance.Value;

        private DialogService() { }


        private  ContentDialog dialog;


        private static Dictionary<Type,Type> typeMap = new Dictionary<Type, Type>();

        public void RegisterDialog<TView,TViewModel>() where TView : ContentDialog 
        {
            typeMap.Add(typeof(TViewModel), typeof(TView));
        }

        public async Task ShowDialog<TViewModel>(params object[] parametr)
        {
            var type = typeMap[typeof(TViewModel)];

            await ShowDialogInternal(type, parametr, typeof(TViewModel));
        }

        public async Task ShowDialog<TViewModel>()
        {
            await ShowDialog<TViewModel>(null);
        }

        private async Task ShowDialogInternal(Type type, object[] parametr, Type vmType)
        {
            if(parametr == null)
            {
                dialog = (ContentDialog)Activator.CreateInstance(type);
            }
            else
            {
                dialog = (ContentDialog)Activator.CreateInstance(type, parametr);
            }
             
             await dialog.ShowAsync();
           
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
