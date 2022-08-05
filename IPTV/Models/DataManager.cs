using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Windows.Storage;
using IPTV.Constants;

namespace IPTV.Models
{
    public static class DataManager
    {
        public static async Task SaveLinksInfo(LinksInfoList links)
        {
            var storageFolder = ApplicationData.Current.LocalFolder;

            StorageFile jsonFile;

            try
            {
                jsonFile = await storageFolder.GetFileAsync(Constant.DataFileName);
            }
            catch
            {
                jsonFile = await storageFolder.CreateFileAsync(Constant.DataFileName);
            }
           


            string linksjson = JsonConvert.SerializeObject(links);

            await FileIO.WriteTextAsync(jsonFile, linksjson);
        }

        public static async Task<LinksInfoList> GetLinksInfo()
        {
            var links = new LinksInfoList();

            var storageFolder = ApplicationData.Current.LocalFolder;

            StorageFile jsonFile;

            try
            {
                jsonFile = await storageFolder.GetFileAsync(Constant.DataFileName);
            }
            catch
            {
                return links;
            }

            string linksjson = await FileIO.ReadTextAsync(jsonFile);

            if (linksjson.Length == 0)
            {
                return links;
            }

            return links =  JsonConvert.DeserializeObject<LinksInfoList>(linksjson);

        }
    }
}
