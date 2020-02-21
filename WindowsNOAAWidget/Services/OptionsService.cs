using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WindowsNOAAWidget.Models;

namespace WindowsNOAAWidget.Services
{
    public class OptionsService
    {
        private string _FileName = "autosave.json";

        public ApplicationOptions LoadSavedOptions()
        {
            var applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var optionsFilePath = Path.Combine(applicationPath, _FileName);

            if (File.Exists(optionsFilePath))
            {
                var serialized = File.ReadAllText(optionsFilePath);
                var deserialized = JsonConvert.DeserializeObject<ApplicationOptions>(serialized);
                return deserialized;
            }
            else
            {
                return new ApplicationOptions();
            }
        }

        public void SaveOptions(ApplicationOptions options)
        {
            var serialized = JsonConvert.SerializeObject(options);
            var applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var optionsFilePath = Path.Combine(applicationPath, _FileName);

            File.WriteAllText(optionsFilePath, serialized);
        }
    }
}
