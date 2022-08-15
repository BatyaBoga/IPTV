using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace BackgroundUpdater
{
    public sealed class Updater : IBackgroundTask
    {
        private BackgroundTaskDeferral deferral;
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            //deferral = taskInstance.GetDeferral();

            //foreach (var item in collection)
            //{
            //    await manager.Update(item);
            //}

            //deferral.Complete();
        }
    }
}
