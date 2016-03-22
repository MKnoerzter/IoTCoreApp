using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IotCoreApp.Model.MessageData
{
    class DeviceInfo
    {

        public const int MaxLength = 200;

        public string PrimaryKey { get; set; }

        public string SecondaryKey { get; set; }
        
        public string ObjectName { get; set; }
        
        public string Version { get; set; }

        public string Manufacturer { get; set; }
        
        public string ModelNumber { get; set; }
        
        public string SerialNumber { get; set; }
        
        public string FirmwareVersion { get; set; }
        
        public string Platform { get; set; }
        
        public string Processor { get; set; }
        
        public string InstalledRAM { get; set; }

        public bool IsEdit { get; set; }

        public bool SupportsChangeConfigCommand { get; set; }

        public string HostName { get; set; }

        public string InstructionsUrl { get; set; }

        public string IsCellular { get; set; }

    }
}
