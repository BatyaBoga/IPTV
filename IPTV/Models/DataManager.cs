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

            var linksInfoList = new LinksInfoList();

            if (linksjson.Length != 0)
            {
                linksInfoList = JsonConvert.DeserializeObject<LinksInfoList>(linksjson);
            }

            return linksInfoList;
        }


        private static async Task<StorageFile> GetFile(string fileName)
        {
            var storageFolder = ApplicationData.Current.LocalFolder;

            StorageFile storageFile;

            try
            {
                storageFile = await storageFolder.GetFileAsync(fileName);
            }
            catch (FileNotFoundException)
            {               
                storageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Data/Links.json"));

                await storageFile.CopyAsync(ApplicationData.Current.LocalFolder, fileName);
            }
            
            return storageFile;

        }
    }
}
