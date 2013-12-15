using System;
using System.Globalization;
using System.IO;
using TDF.Graphics.Cameras;
using TDF.Graphics.Render;
using SharpDX.Direct3D;

namespace TDF.Core
{
    /// <summary>
    /// Конфигурация приложения
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// The camera
        /// </summary>

        public static Camera CurrentCamera;

        private static IniFile _iniFile;

        #region Статические свойства

        /// <summary>
        /// Gets the data file path.
        /// </summary>
        /// <value>
        /// The data file path.
        /// </value>
        public static string DataFilePath { get; private set; }

        /// <summary>
        /// Gets the FPS.
        /// </summary>
        /// <value>
        /// The FPS.
        /// </value>
        public static ushort Fps { get; private set; }

        /// <summary>
        /// DirectX version for Effects
        /// </summary>
        /// <value>
        /// DX version
        /// </value>
        public static string FShaderVersion { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether [full screen].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [full screen]; otherwise, <c>false</c>.
        /// </value>
        public static bool FullScreen { get; set; }

        /// <summary>
        /// Gets or sets the height of the window.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public static int Height { get; set; }

        /// <summary>
        /// Gets the model file path.
        /// </summary>
        /// <value>
        /// The model file path.
        /// </value>
        public static string ModelFilePath { get; private set; }

        /// <summary>
        /// Gets or sets the pixel shader version.
        /// </summary>
        /// <value>
        /// The pixel shader version.
        /// </value>
        public static string PShaderVersion { get; set; }

        /// <summary>
        /// Gets the screen depth.
        /// </summary>
        /// <value>
        /// The screen depth.
        /// </value>
        public static float ScreenDepth { get; private set; }

        /// <summary>
        /// Gets the screen near.
        /// </summary>
        /// <value>
        /// The screen near.
        /// </value>
        public static float ScreenNear { get; private set; }

        /// <summary>
        /// Gets the shaders file path.
        /// </summary>
        /// <value>
        /// The shaders file path.
        /// </value>
        public static string ShadersFilePath { get; private set; }

        /// <summary>
        /// Gets the texture file path.
        /// </summary>
        /// <value>
        /// The texture file path.
        /// </value>
        public static string TextureFilePath { get; private set; }

        /// <summary>
        /// Gets or sets the window title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public static string Title { get; set; }

        /// <summary>
        /// Gets a value indicating whether [vertical synchronize enabled].
        /// </summary>
        /// <value>
        /// <c>true</c> if [vertical synchronize enabled]; otherwise, <c>false</c>.
        /// </value>
        public static bool VerticalSyncEnabled { get; private set; }

        /// <summary>
        /// Gets or sets the vertex shader version.
        /// </summary>
        /// <value>
        /// The vertex shader version.
        /// </value>
        public static string VShaderVersion { get; set; }

        /// <summary>
        /// Gets or sets the width of the window.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public static int Width { get; set; }

        #endregion Статические свойства

        #region Инициализвация

        /// <summary>
        /// Initializes the specified configuration file path.
        /// </summary>
        /// <param name="configFilePath">The configuration file path.</param>
        public static void Initialize(string configFilePath)
        {
            if (!File.Exists(configFilePath))
            {
                ErrorProvider.Send(new Exception(
                    "Файл конфигурации не был найден. Программа создала новый файл со стандартной конфигруцией"));;
                Initialize();
                File.Create(configFilePath).Dispose();
                SaveConfig(AppDomain.CurrentDomain.BaseDirectory + configFilePath);
                return;
            }

            _iniFile = new IniFile(AppDomain.CurrentDomain.BaseDirectory + configFilePath);

            DataFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\");

            Title = _iniFile.ReadValue("Window", "Title", "Engine Demo");

            {
                int t;
                int.TryParse(_iniFile.ReadValue("Window", "Height", "1080"), out t);
                Height = t;
                int.TryParse(_iniFile.ReadValue("Window", "Width", "1920"), out t);
                Width = t;
            }

            VerticalSyncEnabled = _iniFile.ReadValue("Window", "VerticalSyncEnabled", "True") == bool.TrueString;

            FullScreen = _iniFile.ReadValue("Window", "FullScreen", "False") == bool.TrueString;

            {
                float t;
                float.TryParse(_iniFile.ReadValue("Window", "ScreenDepth", "10000"), NumberStyles.Number, CultureInfo.InvariantCulture, out t);
                ScreenDepth = t;
            }

            {
                float t;
                var d = _iniFile.ReadValue("Window", "ScreenNear", "0.1");
                float.TryParse(d, NumberStyles.Number, CultureInfo.InvariantCulture, out t);
                ScreenNear = t;
            }

            {
                ushort t;
                ushort.TryParse(_iniFile.ReadValue("Window", "Fps", "60"), out t);
                Fps = t;
            }

            ModelFilePath = Path.Combine(DataFilePath, _iniFile.ReadValue("FilePath", "ModelFilePath", @"Models\"));
            ShadersFilePath = Path.Combine(DataFilePath, _iniFile.ReadValue("FilePath", "ShadersFilePath", @"Shaders\"));
            TextureFilePath = Path.Combine(DataFilePath, _iniFile.ReadValue("FilePath", "TextureFilePath", @"Textures\"));
        }

        /// <summary>
        /// Initializes the feature.
        /// </summary>
        public static void InitializeFeature()
        {
            switch (DirectX11.Device.FeatureLevel)
            {
                case FeatureLevel.Level_11_0:
                    {
                        VShaderVersion = "vs_5_0";
                        PShaderVersion = "ps_5_0";
                        FShaderVersion = "DirectX11";
                        break;
                    }
                case FeatureLevel.Level_10_1:
                    {
                        VShaderVersion = "vs_4_1";
                        PShaderVersion = "ps_4_1";
                        FShaderVersion = "DirectX10";
                        break;
                    }
                case FeatureLevel.Level_10_0:
                    {
                        VShaderVersion = "vs_4_0";
                        PShaderVersion = "ps_4_0";
                        FShaderVersion = "DirectX10";
                        break;
                    }
                case FeatureLevel.Level_9_3:
                    {
                        VShaderVersion = "vs_4_0_level_9_3";
                        PShaderVersion = "ps_4_0_level_9_3";
                        FShaderVersion = "DirectX9";
                        break;
                    }
                case FeatureLevel.Level_9_2:
                case FeatureLevel.Level_9_1:
                    {
                        VShaderVersion = "vs_4_0_level_9_1";
                        PShaderVersion = "ps_4_0_level_9_1";
                        FShaderVersion = "DirectX9";
                        break;
                    }
            }
        }

        /// <summary>
        /// Saves the configuration.
        /// </summary>
        /// <param name="configFilePath">The configuration file path.</param>
        public static void SaveConfig(string configFilePath)
        {
            if (_iniFile == null) _iniFile = new IniFile(configFilePath);

            _iniFile["Window", "Title"] = Title;
            _iniFile["Window", "Height"] = Height.ToString(CultureInfo.InvariantCulture);
            _iniFile["Window", "Width"] = Width.ToString(CultureInfo.InvariantCulture);
            _iniFile["Window", "VerticalSyncEnabled"] = VerticalSyncEnabled ? bool.TrueString : bool.FalseString;
            _iniFile["Window", "FullScreen"] = FullScreen ? bool.TrueString : bool.FalseString;
            _iniFile["Window", "ScreenDepth"] = ScreenDepth.ToString(CultureInfo.InvariantCulture);
            _iniFile["Window", "ScreenNear"] = ScreenNear.ToString(CultureInfo.InvariantCulture);
            _iniFile["Window", "Fps"] = Fps.ToString(CultureInfo.InvariantCulture);
            _iniFile["FilePath", "ModelFilePath"] = ModelFilePath.Substring(DataFilePath.Length);
            _iniFile["FilePath", "ShadersFilePath"] = ShadersFilePath.Substring(DataFilePath.Length);
            _iniFile["FilePath", "TextureFilePath"] = TextureFilePath.Substring(DataFilePath.Length);
        }

        /// <summary>
        /// Initializes configuration.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        private static void Initialize(string title = "Engine Demo", int width = 1920, int height = 1080)
        {
            Title = title;
            Width = width;
            Height = height;

            FullScreen = false;
            VerticalSyncEnabled = true;
            ScreenDepth = 10000;

            ScreenNear = 0.1F;

            Fps = 60;

            VShaderVersion = "vs_4_0_level_9_1";
            PShaderVersion = "ps_4_0_level_9_1";
            FShaderVersion = "fx_4_0_level_9_1";

            DataFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\");

            ShadersFilePath = DataFilePath + @"Shaders\";
            ModelFilePath = DataFilePath + @"Models\";
            TextureFilePath = DataFilePath + @"Textures\";
        }

        #endregion Инициализвация
    }
}