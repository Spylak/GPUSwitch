using System.Management;

namespace SwitchGPU;

public class VariousFunctions
{
    

void GetGPUInfo()
{
    var searcher = new ManagementObjectSearcher("select * from Win32_VideoController");
    foreach (ManagementObject obj in searcher.Get())
    {
        foreach (var property in obj.Properties)
        {
            Console.WriteLine(property.Name + " : " + property.Value);
        }
    }
}

bool GetBatteryStatus()
{
    ObjectQuery query = new ObjectQuery("Select * FROM Win32_Battery");
    ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

    ManagementObjectCollection collection = searcher.Get();
    foreach (var item in collection)
    {
        foreach (PropertyData property in item.Properties)
        {
            if (property.Name.Equals("BatteryStatus"))
            {
                return ((UInt16)property.Value).Equals(1);
            }
        }
    }

    return false;
}

void EnableDisableGPU()
{
    var newProcessInfo = new System.Diagnostics.ProcessStartInfo();
    newProcessInfo.FileName = @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe";
    newProcessInfo.Verb = "runas";
    var onBattery = GetBatteryStatus();
    if (onBattery)
    {
        newProcessInfo.Arguments = string.Format("pnputil /disable-device \"{0}\"", "PCI\\VEN_1002'&'DEV_73EF'&'SUBSYS_1DCC1043'&'REV_C0\\6'&'E1D2937'&'0'&'00000009");
    }
    else
    {
        newProcessInfo.Arguments = string.Format("pnputil /enable-device \"{0}\"", "PCI\\VEN_1002'&'DEV_73EF'&'SUBSYS_1DCC1043'&'REV_C0\\6'&'E1D2937'&'0'&'00000009");
    }
    System.Diagnostics.Process.Start(newProcessInfo);
}
}