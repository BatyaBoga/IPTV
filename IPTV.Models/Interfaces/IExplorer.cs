﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace IPTV.Models.Interfaces
{
    public interface IExplorer
    {
        Task DeleteFile(string name);

        Task SaveToFile(string fileName, string inform);

        Task SaveNewFile(StorageFile file);

        Task SaveToNewFile(string fileName, string inform);

        Task<List<string>> LoadFromFiles();

        Task<string> LoadFromFile(string fileName);

        Task<IReadOnlyList<StorageFile>> GetAllFiles();

        Task<StorageFile> GetFile(string fileName);

        Task ConfigureFolder(string folderName);

        Task CreateFolder(string folderName);
    }
}
