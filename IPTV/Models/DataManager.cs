using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Windows.Storage;
using IPTV.Constants;
using System.IO;

namespace IPTV.Models
{
    public static class DataManager
    {
        public static async Task SaveLinksInfo(LinksInfoList links)
        {
            var jsonFile = await GetFile(Constant.DataFileName);

            string linksjson = JsonConvert.SerializeObject(links);

            await FileIO.WriteTextAsync(jsonFile, linksjson);
        }

        public static async Task<LinksInfoList> GetLinksInfo()
        {
            var jsonFile = await GetFile(Constant.DataFileName);

            string linksjson = await FileIO.ReadTextAsync(jsonFile);

            if (linksjson.Length == 0)
            {
                return new LinksInfoList();
            }

            return JsonConvert.DeserializeObject<LinksInfoList>(linksjson);
        }


        private static async Task<StorageFile> GetFile(string fileName)
        {
            var storageFolder = ApplicationData.Current.LocalFolder;

            try
            {
                return await storageFolder.GetFileAsync(fileName);
            }
            catch (FileNotFoundException)
            {
                return await storageFolder.CreateFileAsync(fileName);
            }  

        }
    }
}
