using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using STALKER2_Disable_Shader_Compilation.Lib;

namespace STALKER2_Disable_Shader_Compilation.ViewModels
{
    internal class TogglePageViewModel : INotifyPropertyChanged
    {


        //environment variable for local app data is %LOCALAPPDATA%

        //steam app data path
        //C:\Users\USERNAME\AppData\Local\Stalker2\Saved\Config\Windows

        //xbox game pass app data path
        //C:\Users\USERNAME\AppData\Local\Stalker2\Saved\Config\WinGDK

        //fileContents

        //[SystemSettings]
        //r.PSOWarmup.WarmupMaterials=0


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        static string USERNAME = Environment.UserName;
        const string STEAM_DIRECTORY = "Steam";
        const string XBOX_DIRECTORY = "Xbox";
        const string NOT_FOUND = "No Installation Found";

        const int DISABLED = 0;
        const int ENABLED = 1;

        string _steamFilePath = string.Format("C:\\\\Users\\{0}\\\\AppData\\\\Local\\\\Stalker2\\\\Saved\\\\Config\\\\Windows", USERNAME);
        string _xboxFilePath = string.Format("C:\\\\Users\\\\{0}\\\\AppData\\\\Local\\\\Stalker2\\\\Saved\\\\Config\\\\WinGDK", USERNAME);
        const string _fileName = "Engine.ini";


        string _platformString = string.Empty;

        string directoryPath = string.Empty;

        private bool _isRayTracingButtonChecked;
        private bool _isShaderCompButtonChecked;
        private bool _installationFound;
        private string _isVisible = "Hidden";
        private string _shaderCompButtonText = "Re-Enable";
        private string _rayTracingButtonText = "Re-Enable";



        public ICommand ToggleShaderCompCommand { get; }
        public ICommand ToggleRayTracingCommand { get; }

        private string _shaderCompilationMessage = "";
        private string _rayTracingMessage = "";



        public TogglePageViewModel()
        {

            ToggleShaderCompCommand = new RelayCommand(ToggleShaderComp);
            ToggleRayTracingCommand = new RelayCommand(ToggleRayTracing);
            //check what the value is in the file and set checked value accordingly
            getPlatform();
            if (PlatformString != NOT_FOUND)
            {
                int currentShaderCompValue = getCurrentShaderCompValue();
                int currentRayTracingValue = getCurrentRayTracingValue();

                IsShaderCompButtonChecked = currentShaderCompValue == DISABLED;
                IsRayTracingButtonChecked = currentRayTracingValue == DISABLED;

                ShaderCompButtonText = IsShaderCompButtonChecked ? "Re-Enable" : "Disable";
                RayTracingButtonText = IsRayTracingButtonChecked ? "Re-Enable" : "Disable";

                ShaderCompilationMessage = IsShaderCompButtonChecked ? "Disabled" : "Enabled";
                RayTracingMessage = IsRayTracingButtonChecked ? "Disabled" : "Enabled";

                IsVisible = "Visible";
            }
            IsVisible = (PlatformString == NOT_FOUND) ? "Hidden" : "Visible";

        }

        private void getPlatform()
        {
            if (Directory.Exists(_steamFilePath))
            {
                directoryPath = _steamFilePath;
                PlatformString = STEAM_DIRECTORY;
                InstallationFound = true;

            }
            else if (Directory.Exists(_xboxFilePath))
            {
                directoryPath = _xboxFilePath;
                PlatformString = XBOX_DIRECTORY;
                InstallationFound = true;

            }
            else
            {
                PlatformString = NOT_FOUND;
                InstallationFound = false;
            }
        }

        public string ShaderCompButtonText
        {
            get => _shaderCompButtonText;
            set
            {
                _shaderCompButtonText = value;
                OnPropertyChanged();
            }
        }
        public string RayTracingButtonText
        {
            get => _rayTracingButtonText;
            set
            {
                _rayTracingButtonText = value;
                OnPropertyChanged();
            }
        }
        public string ShaderCompilationMessage
        {
            get => _shaderCompilationMessage;
            set
            {
                _shaderCompilationMessage = value;
                OnPropertyChanged();
            }
        }
        public string RayTracingMessage
        {
            get => _rayTracingMessage;
            set
            {
                _rayTracingMessage = value;
                OnPropertyChanged();
            }
        }
        public string IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                OnPropertyChanged();
            }
        }
        public bool IsShaderCompButtonChecked
        {
            get => _isShaderCompButtonChecked;
            set
            {
                _isShaderCompButtonChecked = value;
                OnPropertyChanged();
            }
        }
        public bool IsRayTracingButtonChecked
        {
            get => _isRayTracingButtonChecked;
            set
            {
                _isRayTracingButtonChecked = value;
                OnPropertyChanged();
            }
        }
        public bool InstallationFound
        {
            get => _installationFound;
            set
            {
                _installationFound = value;
                OnPropertyChanged();
            }
        }
        public string PlatformString
        {
            get => _platformString;
            set
            {
                _platformString = value;
                OnPropertyChanged();
            }
        }

        private void ToggleShaderComp()
        {
            setShaderCompValue(IsShaderCompButtonChecked ? DISABLED : ENABLED);
            ShaderCompButtonText = IsShaderCompButtonChecked ? "Re-Enable" : "Disable";
            ShaderCompilationMessage = IsShaderCompButtonChecked ? "Disabled" : "Enabled";

        }
        private void ToggleRayTracing()
        {
            setRayTracingValue(IsRayTracingButtonChecked ? DISABLED : ENABLED);
            RayTracingButtonText = IsRayTracingButtonChecked ? "Re-Enable" : "Disable";
            RayTracingMessage = IsRayTracingButtonChecked ? "Disabled" : "Enabled";

        }


        private int getCurrentShaderCompValue()
        {
            string filePath = Path.Combine(directoryPath, _fileName);
            if (File.Exists(filePath))
            {
                var lines = File.ReadLines(filePath);
                foreach (var line in lines)
                {
                    if (line.Contains("r.PSOWarmup.WarmupMaterials"))
                    {
                        var parts = line.Split('=');
                        if (parts.Length == 2 && int.TryParse(parts[1], out int value))
                        {
                            return value;
                        }
                    }
                }
            }
            return -1; // Return a default value or handle the case where the line is not found
        }
        private int getCurrentRayTracingValue()
        {
            string filePath = Path.Combine(directoryPath, _fileName);
            if (File.Exists(filePath))
            {
                var lines = File.ReadLines(filePath);
                foreach (var line in lines)
                {
                    if (line.Contains("r.RayTracing.ForceAllRayTracingEffects"))
                    {
                        var parts = line.Split('=');
                        if (parts.Length == 2 && int.TryParse(parts[1], out int value))
                        {
                            return value;
                        }
                    }
                }
            }
            return -1; // Return a default value or handle the case where the line is not found
        }
        private void setShaderCompValue(int value)
        {
            string filePath = Path.Combine(directoryPath, _fileName);
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                var found = false;
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("r.PSOWarmup.WarmupMaterials"))
                    {
                        lines[i] = $"r.PSOWarmup.WarmupMaterials={value}";
                        found = true;
                        break;
                    }
                }
                if(!found)
                {
                   lines = lines.Append("[SystemSettings]").ToArray();
                   lines = lines.Append($"r.PSOWarmup.WarmupMaterials={value}").ToArray();
                }
                File.WriteAllLines(filePath, lines);
            }
            else
            {
                // Create the file and write the default content
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine("[SystemSettings]");
                    sw.WriteLine($"r.PSOWarmup.WarmupMaterials={value}");
                    sw.Close();
                }
            }
        }
        private void setRayTracingValue(int value)
        {
            string filePath = Path.Combine(directoryPath, _fileName);
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                var found = false;
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("r.RayTracing.ForceAllRayTracingEffects"))
                    {
                        lines[i] = $"r.RayTracing.ForceAllRayTracingEffects={value}";
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    lines = lines.Append("[System]").ToArray();
                    lines = lines.Append($"r.RayTracing.ForceAllRayTracingEffects={value}").ToArray();

                }
                File.WriteAllLines(filePath, lines);
            }
            else
            {
                // Create the file and write the default content
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine("[System]");
                    sw.WriteLine($"r.RayTracing.ForceAllRayTracingEffects={value}");
                    sw.Close();
                }
            }
        }

    }
}
