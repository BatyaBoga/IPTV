using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using IPTV.Interfaces;

namespace IPTV.Models
{
    public class Explorer : IExplorer
    {
        private StorageFolder storageFolder;

        public Explorer()
        {
            storageFolder = ApplicationData.Current.LocalFolder;
        }

        public async Task ConfigureFolder(string folderName)
        {
            try
            {
                storageFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync(folderName);
            }
            catch (FileNotFoundException)
            {
                throw;
            }
        }

        public async Task DeleteFile(string name)
        {
            var file = await GetFile(name);

            await file.DeleteAsync();
        }

        public async Task SaveToFile(string fileName, string inform)
        {
            var file = await GetFile(fileName);

            if (file != null)
            {
                await FileIO.WriteTextAsync(file, inform);
            }
        }

        public async Task SaveToNewFile(string fileName, string inform)
        {
            await storageFolder.CreateFileAsync(fileName);

            await SaveToFile(fileName, inform);
        }

        public async Task<List<string>> LoadFromFiles()
        {
            var files = await GetAllFiles();

            var infoFromFiles = new List<string>();

            if (files != null)
            {
                foreach (var item in files)
                {
                    infoFromFiles.Add(await FileIO.ReadTextAsync(item));
                }
            }

            return infoFromFiles;
        }

        public async Task<string> LoadFromFile(string fileName)
        {
            var file = await GetFile(fileName);

            string text = String.Empty;

            if (file != null)
            {
                text = await FileIO.ReadTextAsync(file);
            }

            return text;
        }

        public async Task<IReadOnlyList<StorageFile>> GetAllFiles()
        {
            IReadOnlyList<StorageFile> fileList;

            try
            {
                fileList = await storageFolder.GetFilesAsync();
            }
            catch (FileNotFoundException)
            {
                fileList = null;
            }

            return fileList;
        }

        public async Task<StorageFile> GetFile(string fileName)
        {
            StorageFile storageFile;

            try
            {
                storageFile = await storageFolder.GetFileAsync(fileName);
            }
            catch (FileNotFoundException)
            {
                storageFile = null;
            }

            return storageFile;
        }
            
        public async Task CreateFolder(string folderName)
        {
           storageFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(folderName);
        }

        public static async Task<string> GetPreloadData()
        {
            var preloadFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Data/IptvData/Links.json"));

            return await FileIO.ReadTextAsync(preloadFile);
        }
    }
}
