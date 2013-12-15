using System;
using System.ComponentModel;
using System.Text;

namespace TDF.Core
{
    /// <summary>
    /// INI file helper
    /// </summary>
    public class IniFile
    {
        private const int MaxSize = 256;

        private readonly string _filePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="IniFile"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <exception cref="System.ArgumentNullException">filePath</exception>
        public IniFile(string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException("filePath");

            _filePath = filePath;
        }

        /// <summary>
        /// Read or write value to INI file
        /// </summary>
        /// <value>
        /// The <see cref="System.String"/>.
        /// </value>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string this[string section, string key]
        {
            get
            {
                return ReadValue(section, key);
            }

            set
            {
                WriteValue(section, key, value);
            }
        }

        /// <summary>
        /// Reads the value from INI file.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        /// <exception cref="System.ComponentModel.Win32Exception"></exception>
        public string ReadValue(string section, string key, string defaultValue = null)
        {
            var result = new StringBuilder(MaxSize);
            if (WinAPI.GetPrivateProfileString(section, key, defaultValue ?? string.Empty, result, (uint)result.Capacity, _filePath) > 0)
                return result.ToString();

            throw new Win32Exception();
        }

        /// <summary>
        /// Writes the value to INI file.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ComponentModel.Win32Exception"></exception>
        public void WriteValue(string section, string key, string value)
        {
            if (!WinAPI.WritePrivateProfileString(section, key, value, _filePath))
                throw new Win32Exception();
        }
    }
}