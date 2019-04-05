using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace SysHv.Client.TopShelfService.DTOs
{
    public class NetworkInterfaceDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsReceiveOnly { get; set; }

        public NetworkInterfaceType NetworkInterfaceType { get; set; }

        public OperationalStatus OperationalStatus { get; set; }

        public long Speed { get; set; }

        public bool SupportsMulticast { get; set; }


        public static NetworkInterfaceDTO FromNetworkInterface(NetworkInterface netInterface)
        {
            return new NetworkInterfaceDTO()
            {
                Id = netInterface.Id,
                Name = netInterface.Name,
                Description = netInterface.Description,
                IsReceiveOnly = netInterface.IsReceiveOnly,
                NetworkInterfaceType = netInterface.NetworkInterfaceType,
                OperationalStatus = netInterface.OperationalStatus,
                Speed = netInterface.Speed,
                SupportsMulticast = netInterface.SupportsMulticast,
            };
        }
    }
}
