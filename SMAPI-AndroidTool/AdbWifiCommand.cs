using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace SMAPI_AndroidTool;

class AdbWifiCommandSetting : CommandSettings
{

}

internal class AdbWifiCommand : Command<AdbWifiCommandSetting>
{
    public override int Execute(CommandContext context, AdbWifiCommandSetting settings)
    {
        Console.WriteLine("Try ADB Wifi Connect..");
        var interfaces = NetworkInterface.GetAllNetworkInterfaces()
       .Where(ni =>
           ni.OperationalStatus == OperationalStatus.Up &&
           ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211);

        Console.WriteLine($"found interfaces: {interfaces.Count()}");
        if (interfaces.Count() == 1)
        {
            var ip = interfaces.First().GetIPProperties().GatewayAddresses[0].Address;
            Console.WriteLine("try open port 5555");
            Adb.Run("tcpip 5555");

            Console.WriteLine($"try connect at first interface ip: {ip}");
            if (Adb.Run($"connect {ip}") is false)
                Console.WriteLine("error can't connect");

            return 0;
        }

        Console.WriteLine("failed!, you need to manual adb connect {ip}!!");

        foreach (var ni in interfaces)
        {
            Console.WriteLine($"[Interface] {ni.Name} ({ni.Description})");

            var ipProps = ni.GetIPProperties();
            foreach (var gateway in ipProps.GatewayAddresses)
            {
                Console.WriteLine($" - gateway: {gateway.Address}");
            }
        }

        return 0;
    }
}
