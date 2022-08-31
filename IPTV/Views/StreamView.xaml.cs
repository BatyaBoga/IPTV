using Windows.Storage;
using IPTV.Services;
using IPTV.ViewModels;
using IPTV.Constants;

namespace IPTV.Views
{
    public sealed partial class StreamView : PageWithPlayer
    {
        public StreamView()
        {
            InitializeComponent();

            ViewModel = ViewModelLocator.Instance.Stream;
        }

        protected override void SetMediaSource(object parameter)
        {
            var viewModel = ViewModel as StreamViewModel;

            if (parameter is StorageFile)
            {
                viewModel.SetSource(parameter as StorageFile);

                SaveServise.ActiveSave(Constant.Local, viewModel);
            }
            else
            {
                viewModel.SetSource(parameter.ToString());

                SaveServise.ActiveSave(Constant.Remote, viewModel);
            }
        }
    }
}
