using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsNOAAWidget.Models;

namespace WindowsNOAAWidget.Services
{
    public class OptionsService
    {
        private string _FilePath = "autosave.json";

        public ApplicationOptions LoadSavedOptions()
        {
            if (File.Exists(_FilePath))
            {
                var serialized = File.ReadAllText(_FilePath);
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
            File.WriteAllText(_FilePath, serialized);
        }
    }
}
