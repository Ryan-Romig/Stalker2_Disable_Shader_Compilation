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

        private bool _isButtonChecked;
        private bool _installationFound;
        private string _isVisible = "Hidden";
        private string _buttonText = "Re-Enable";

        public ICommand ToggleCommand { get; }

        private string _shaderCompilationMessage = "";



        public TogglePageViewModel()
        {

            ToggleCommand = new RelayCommand(Toggle);
            //check what the value is in the file and set checked value accordingly
            getPlatform();
            if (PlatformString != NOT_FOUND)
            {
                int currentValue = getCurrentValue();
                IsButtonChecked = currentValue == DISABLED;
                ButtonText = IsButtonChecked ? "Re-Enable" : "Disable";
                ShaderCompilationMessage = IsButtonChecked ? "Disabled" : "Enabled";
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

        public string ButtonText
        {
            get => _buttonText;
            set
            {
                _buttonText = value;
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
        public string IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                OnPropertyChanged();
            }
        }
        public bool IsButtonChecked
        {
            get => _isButtonChecked;
            set
            {
                _isButtonChecked = value;
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

        private void Toggle()
        {
            setShaderCompValue(IsButtonChecked ? DISABLED : ENABLED);
            ButtonText = IsButtonChecked ? "Re-Enable" : "Disable";
            ShaderCompilationMessage = IsButtonChecked ? "Disabled" : "Enabled";



        }


        private int getCurrentValue()
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
        private void setShaderCompValue(int value)
        {
            string filePath = Path.Combine(directoryPath, _fileName);
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("r.PSOWarmup.WarmupMaterials"))
                    {
                        lines[i] = $"r.PSOWarmup.WarmupMaterials={value}";
                        break;
                    }
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

    }
}
