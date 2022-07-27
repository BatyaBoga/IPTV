﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace IPTV.Models
{
    public static class DataManager
    {

        //public static async Task SaveLinksInfo(List<string> links)
        //{
        //    StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

        //    StorageFile jsonFile = await storageFolder.GetFileAsync("Links.json");

        //    if(jsonFile == null)
        //    {
        //        jsonFile = await storageFolder.CreateFileAsync("Links.json");
        //    }

        //    string linksjson = JsonConvert.SerializeObject(links);

        //    await FileIO.WriteTextAsync(jsonFile, linksjson);
        //}

        public static async Task<LinksInfoList> GetLinksInfo()
        {
            LinksInfoList links = new LinksInfoList();

            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

            StorageFile jsonFile = await storageFolder.GetFileAsync("Links.json");

            if(jsonFile == null)
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