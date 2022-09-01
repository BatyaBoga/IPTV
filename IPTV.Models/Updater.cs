using Windows.ApplicationModel.Background;
using IPTV.Constants;
using IPTV.Models.Interfaces;

namespace IPTV.Models
{
    public class Updater : IBackgroundTask 
    {
        private readonly IIptvManager iptvManager;

        public Updater(IIptvManager iptvManager)
        {
            this.iptvManager = iptvManager;

            BuildTask(); 
        }

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var collection = await iptvManager.GetPlaylistCollection();

            foreach (var item in collection)
            {
                await iptvManager.UpdatePlaylist(item);
            }
        }

        private void BuildTask()
        {
            var builder = new BackgroundTaskBuilder();

            builder.Name = Constant.TaskName;

            builder.SetTrigger(new TimeTrigger(15, true));

            builder.AddCondition(new SystemCondition(SystemConditionType.InternetAvailable));

            builder.AddCondition(new SystemCondition(SystemConditionType.UserNotPresent));

            bool taskRegistered = false;

            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == builder.Name)
                {
                    taskRegistered = true;
                    break;
                }
            }

            if (!taskRegistered)
            {
                var task = builder.Register();
            }
        }
    }
}
