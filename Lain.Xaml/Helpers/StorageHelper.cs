using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace Lain.Xaml.Helpers
{
    public enum StorageStrategies
    {
        /// <summary>Local, isolated folder</summary>
        Local,

        /// <summary>Cloud, isolated folder. 100k cumulative limit.</summary>
        Roaming,

        /// <summary>Local, temporary folder (not for settings)</summary>
        Temporary
    }

    public static class StorageHelper
    {
        public static async void DeleteFileFireAndForget(string key, StorageStrategies location)
        {
            await DeleteFileAsync(key, location);
        }
        public static async void WriteFileFireAndForget<T>(string key, T value, StorageStrategies location)
        {
            await WriteFileAsync(key, value, location);
        }

        #region Settings

        public static IPropertySet GetAllSettings(StorageStrategies location = StorageStrategies.Local)
        {
            switch (location)
            {
            case StorageStrategies.Local:
                return ApplicationData.Current.LocalSettings.Values;
            case StorageStrategies.Roaming:
                return ApplicationData.Current.RoamingSettings.Values;
            default:
                throw new NotSupportedException(location.ToString());
            }
        }

        /// <summary>Returns if a setting is found in the specified storage strategy</summary>
        /// <param name="key">Path of the setting in storage</param>
        /// <param name="location">Location storage strategy</param>
        /// <returns>Boolean: true if found, false if not found</returns>
        public static bool SettingExists(string key, StorageStrategies location = StorageStrategies.Local)
        {
            switch (location)
            {
            case StorageStrategies.Local:
                return ApplicationData.Current.LocalSettings.Values.ContainsKey(key);
            case StorageStrategies.Roaming:
                return ApplicationData.Current.RoamingSettings.Values.ContainsKey(key);
            default:
                throw new NotSupportedException(location.ToString());
            }
        }

        /// <summary>Reads and converts a setting into specified type T</summary>
        /// <typeparam name="T">Specified type into which to value is converted</typeparam>
        /// <param name="key">Path to the file in storage</param>
        /// <param name="otherwise">Return value if key is not found or convert fails</param>
        /// <param name="location">Location storage strategy</param>
        /// <returns>Specified type T</returns>
        public static T GetSetting<T>(string key, T otherwise = default(T),
            StorageStrategies location = StorageStrategies.Local)
        {
            try
            {
                if (!SettingExists(key, location))
                    return otherwise;
                switch (location)
                {
                case StorageStrategies.Local:
                    return (T)ApplicationData.Current.LocalSettings.Values[key];
                case StorageStrategies.Roaming:
                    return (T)ApplicationData.Current.RoamingSettings.Values[key];
                default:
                    throw new NotSupportedException(location.ToString());
                }
            }
            catch
            {
                /* error casting */
                return otherwise;
            }
        }

        /// <summary>Serializes an object and write to file in specified storage strategy</summary>
        /// <typeparam name="T">Specified type of object to serialize</typeparam>
        /// <param name="key">Path to the file in storage</param>
        /// <param name="value">Instance of object to be serialized and written</param>
        /// <param name="location">Location storage strategy</param>
        public static void SetSetting<T>(string key, T value, StorageStrategies location = StorageStrategies.Local)
        {
            switch (location)
            {
            case StorageStrategies.Local:
                ApplicationData.Current.LocalSettings.Values[key] = value;
                break;
            case StorageStrategies.Roaming:
                ApplicationData.Current.RoamingSettings.Values[key] = value;
                break;
            default:
                throw new NotSupportedException(location.ToString());
            }
        }

        public static void DeleteSetting(string key, StorageStrategies location = StorageStrategies.Local)
        {
            switch (location)
            {
            case StorageStrategies.Local:
                ApplicationData.Current.LocalSettings.Values.Remove(key);
                break;
            case StorageStrategies.Roaming:
                ApplicationData.Current.RoamingSettings.Values.Remove(key);
                break;
            default:
                throw new NotSupportedException(location.ToString());
            }
        }

        #endregion

        #region File

        /// <summary>Returns if a file is found in the specified storage strategy</summary>
        /// <param name="key">Path of the file in storage</param>
        /// <param name="location">Location storage strategy</param>
        /// <returns>Boolean: true if found, false if not found</returns>
        public static async Task<bool> FileExistsAsync(string key, StorageStrategies location = StorageStrategies.Local)
        {
            return await GetIfFileExistsAsync(key, location) != null;
        }

        public static async Task<bool> FileExistsAsync(string key, StorageFolder folder)
        {
            return await GetIfFileExistsAsync(key, folder) != null;
        }

        /// <summary>Deletes a file in the specified storage strategy</summary>
        /// <param name="key">Path of the file in storage</param>
        /// <param name="location">Location storage strategy</param>
        public static async Task<bool> DeleteFileAsync(string key, StorageStrategies location = StorageStrategies.Local)
        {
            var file = await GetIfFileExistsAsync(key, location);
            if (file != null)
                await file.DeleteAsync();
            return !await FileExistsAsync(key, location);
        }

        /// <summary>Reads and deserializes a file into specified type T</summary>
        /// <typeparam name="T">Specified type into which to deserialize file content</typeparam>
        /// <param name="key">Path to the file in storage</param>
        /// <param name="location">Location storage strategy</param>
        /// <returns>Specified type T</returns>
        public static async Task<T> ReadFileAsync<T>(string key, StorageStrategies location = StorageStrategies.Local)
        {
            var file = await GetIfFileExistsAsync(key, location);
            if (file == null)
                return default(T);
            try
            {
                var text = await FileIO.ReadTextAsync(file);
                return JsonConvert.DeserializeObject<T>(text);
            }
            catch (IOException)
            {
            }
            catch (JsonException)
            {
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            return default(T);
        }

        /// <summary>Serializes an object and write to file in specified storage strategy</summary>
        /// <typeparam name="T">Specified type of object to serialize</typeparam>
        /// <param name="key">Path to the file in storage</param>
        /// <param name="value">Instance of object to be serialized and written</param>
        /// <param name="location">Location storage strategy</param>
        public static async Task<bool> WriteFileAsync<T>(string key, T value,
            StorageStrategies location = StorageStrategies.Local)
        {
            // create file
            var file = await CreateFileAsync(key, location, CreationCollisionOption.ReplaceExisting);
            if (file == null)
                return false;
            // convert to string
            var s = JsonConvert.SerializeObject(value);
            // save string to file
            await FileIO.WriteTextAsync(file, s);
            // result
            return await FileExistsAsync(key, location);
        }

        private static async Task<StorageFile> CreateFileAsync(string key,
            StorageStrategies location = StorageStrategies.Local,
            CreationCollisionOption option = CreationCollisionOption.OpenIfExists)
        {
            try
            {
                switch (location)
                {
                case StorageStrategies.Local:
                    return await ApplicationData.Current.LocalFolder.CreateFileAsync(key, option);
                case StorageStrategies.Roaming:
                    return await ApplicationData.Current.RoamingFolder.CreateFileAsync(key, option);
                case StorageStrategies.Temporary:
                    return await ApplicationData.Current.TemporaryFolder.CreateFileAsync(key, option);
                default:
                    throw new NotSupportedException(location.ToString());
                }
            }
            catch (IOException)
            {
                return null;
            }
        }

        private static async Task<StorageFile> GetIfFileExistsAsync(string key, IStorageFolder folder)
        {
            StorageFile retval;
            try
            {
                retval = await folder.GetFileAsync(key);
            }
            catch (IOException)
            {
                Debug.WriteLine("GetIfFileExistsAsync:FileNotFoundException");
                return null;
            }
            return retval;
        }

        /// <summary>Returns a file if it is found in the specified storage strategy</summary>
        /// <param name="key">Path of the file in storage</param>
        /// <param name="location">Location storage strategy</param>
        /// <returns>StorageFile</returns>
        private static async Task<StorageFile> GetIfFileExistsAsync(string key,
            StorageStrategies location = StorageStrategies.Local)
        {
            try
            {
                switch (location)
                {
                case StorageStrategies.Local:
                    return await ApplicationData.Current.LocalFolder.GetFileAsync(key);
                case StorageStrategies.Roaming:
                    return await ApplicationData.Current.RoamingFolder.GetFileAsync(key);
                case StorageStrategies.Temporary:
                    return await ApplicationData.Current.TemporaryFolder.GetFileAsync(key);
                default:
                    throw new NotSupportedException(location.ToString());
                }
            }
            catch (IOException)
            {
                Debug.WriteLine("GetIfFileExistsAsync:FileNotFoundException");
                return null;
            }
        }

        #endregion
    }
}