using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Crosscutting.License
{
    public class LicenseKey
    {
        private static string fingerPrint = string.Empty;
        public static string Value()
        {
            if (string.IsNullOrEmpty(LicenseKey.fingerPrint))
            {
                LicenseKey.fingerPrint = LicenseKey.GetHash(string.Concat(new string[]
				{
					"\nBIOS >> ", 
					LicenseKey.biosId(), 
					"\nBASE >> ", 
					LicenseKey.baseId(), 
					"\nDISK >> ", 
					LicenseKey.diskId()
				}));
            }
            return LicenseKey.fingerPrint;
        }
        private static string GetHash(string s)
        {
            MD5 mD = new MD5CryptoServiceProvider();
            ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
            byte[] bytes = aSCIIEncoding.GetBytes(s);
            return LicenseKey.GetHexString(mD.ComputeHash(bytes));
        }
        private static string GetHexString(byte[] bt)
        {
            string text = string.Empty;
            for (int i = 0; i < bt.Length; i++)
            {
                byte b = bt[i];
                int num = (int)b;
                int num2 = num & 15;
                int num3 = num >> 4 & 15;
                if (num3 > 9)
                {
                    string arg_45_0 = text;
                    char c = (char)(num3 - 10 + 65);
                    text = arg_45_0 + c.ToString();
                }
                else
                {
                    text += num3.ToString();
                }
                if (num2 > 9)
                {
                    string arg_7D_0 = text;
                    char c = (char)(num2 - 10 + 65);
                    text = arg_7D_0 + c.ToString();
                }
                else
                {
                    text += num2.ToString();
                }
                if (i + 1 != bt.Length && (i + 1) % 2 == 0)
                {
                    text += "-";
                }
            }
            return text;
        }
        private static string identifier(string wmiClass, string wmiProperty, string wmiMustBeTrue)
        {
            string text = "";
            ManagementClass managementClass = new ManagementClass(wmiClass);
            ManagementObjectCollection instances = managementClass.GetInstances();
            using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = instances.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    ManagementObject managementObject = (ManagementObject)enumerator.Current;
                    if (managementObject[wmiMustBeTrue].ToString() == "True")
                    {
                        if (text == "")
                        {
                            try
                            {
                                text = managementObject[wmiProperty].ToString();
                                break;
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
            return text;
        }
        private static string identifier(string wmiClass, string wmiProperty)
        {
            string text = "";
            ManagementClass managementClass = new ManagementClass(wmiClass);
            ManagementObjectCollection instances = managementClass.GetInstances();
            using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = instances.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    ManagementObject managementObject = (ManagementObject)enumerator.Current;
                    if (text == "")
                    {
                        try
                        {
                            text = managementObject[wmiProperty].ToString();
                            break;
                        }
                        catch
                        {
                        }
                    }
                }
            }
            return text;
        }
        private static string cpuId()
        {
            string text = LicenseKey.identifier("Win32_Processor", "UniqueId");
            if (text == "")
            {
                text = LicenseKey.identifier("Win32_Processor", "ProcessorId");
                if (text == "")
                {
                    text = LicenseKey.identifier("Win32_Processor", "Name");
                    if (text == "")
                    {
                        text = LicenseKey.identifier("Win32_Processor", "Manufacturer");
                    }
                    text += LicenseKey.identifier("Win32_Processor", "MaxClockSpeed");
                }
            }
            return text;
        }
        private static string biosId()
        {
            return string.Concat(new string[]
			{
				LicenseKey.identifier("Win32_BIOS", "Manufacturer"), 
				LicenseKey.identifier("Win32_BIOS", "SMBIOSBIOSVersion"), 
				LicenseKey.identifier("Win32_BIOS", "IdentificationCode"), 
				LicenseKey.identifier("Win32_BIOS", "SerialNumber"), 
				LicenseKey.identifier("Win32_BIOS", "ReleaseDate"), 
				LicenseKey.identifier("Win32_BIOS", "Version")
			});
        }
        private static string diskId()
        {
            return LicenseKey.identifier("Win32_DiskDrive", "Model") + LicenseKey.identifier("Win32_DiskDrive", "Manufacturer") + LicenseKey.identifier("Win32_DiskDrive", "Signature") + LicenseKey.identifier("Win32_DiskDrive", "TotalHeads");
        }
        private static string baseId()
        {
            return LicenseKey.identifier("Win32_BaseBoard", "Model") + LicenseKey.identifier("Win32_BaseBoard", "Manufacturer") + LicenseKey.identifier("Win32_BaseBoard", "Name") + LicenseKey.identifier("Win32_BaseBoard", "SerialNumber");
        }
        private static string videoId()
        {
            return LicenseKey.identifier("Win32_VideoController", "DriverVersion") + LicenseKey.identifier("Win32_VideoController", "Name");
        }
        private static string macId()
        {
            return LicenseKey.identifier("Win32_NetworkAdapterConfiguration", "MACAddress", "IPEnabled");
        }
    }
}
