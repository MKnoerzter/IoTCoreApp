using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IotCoreApp.Model.MessageData
{
    public class DeviceType
    {
        public string ObjectType { get; set; }
        public bool IsSimulatedDevice { get; set; }
        public string Version { get; set; }

        public DeviceProperties DeviceProperties { get; set; }

        public List<Command> Commands { get; set; }
    }

    public class DeviceProperties
    {
        public string DeviceID { get; set; }
        public bool HubEnabledState { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

    }

    public class Command
    {
        public string Name { get; set; }
        public List<Parameter> Parameters { get; set; }
    }

    public class Parameter
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
