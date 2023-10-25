using System.Net;

namespace Totira.Support.CommonLibrary.Shared
{
    public class IpAddressValidation
    {
        public static bool ValidateIPAddress(string yourIpAddressString, string ranges)
        {
            IPAddress ipAddressToCheck;

            if (string.IsNullOrEmpty(yourIpAddressString))
            {
                Console.WriteLine("Error: You must provide a valid IP address.");
                return false;
            }
            else
            {
                if (!IPAddress.TryParse(yourIpAddressString, out ipAddressToCheck))
                {
                    Console.WriteLine("Error: Invalid IP address format.");
                    return false;
                }
            }

            if (string.IsNullOrEmpty(ranges))
            {
                Console.WriteLine("Error: You must provide valid IP ranges.");
                return false;
            }

            string[] arrayIps = ranges.Split(";");

            if (arrayIps.Contains(yourIpAddressString))
            {
                Console.WriteLine("The ip is the list.");
                return true;
            }
            else
            {
                foreach (string ip in arrayIps)
                {
                    if (string.IsNullOrEmpty(ip))
                    {
                        Console.WriteLine("Error: An IP range is missing.");
                        return false;
                    }

                    if (ip.Contains($"/"))
                    {
                        string[] ipSubRange = ip.Split("/");
                        if (ipSubRange.Length != 2 || !int.TryParse(ipSubRange[1], out int subnetBits))
                        {
                            Console.WriteLine($"Error: Invalid IP range format: {ip}");
                            continue;
                        }

                        IPAddress networkAddress = IPAddress.Parse(ipSubRange[0]);
                        IPAddress subnetMask = CalculateSubnetMask(subnetBits);
                        IPAddress endIpAddress = CalculateEndIpAddress(networkAddress, subnetMask);

                        bool isInRange = IsIpAddressInRange(ipAddressToCheck, networkAddress, endIpAddress);

                        if (isInRange)
                        {
                            Console.WriteLine($"Your IP address ({yourIpAddressString}) is in the range {ip}.");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine($"Your IP address ({yourIpAddressString}) is NOT in the range {ip}.");
                        }
                    }
                }

                return false;
            }
        }

        static IPAddress CalculateEndIpAddress(IPAddress startIpAddress, IPAddress subnetMask)
        {
            byte[] startIpBytes = startIpAddress.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();
            byte[] endIpBytes = new byte[startIpBytes.Length];

            for (int i = 0; i < startIpBytes.Length; i++)
            {
                endIpBytes[i] = (byte)(startIpBytes[i] | ~subnetMaskBytes[i]);
            }

            return new IPAddress(endIpBytes);
        }

        static bool IsIpAddressInRange(IPAddress ipAddress, IPAddress startIpAddress, IPAddress endIpAddress)
        {
            byte[] ipBytes = ipAddress.GetAddressBytes();
            byte[] startBytes = startIpAddress.GetAddressBytes();
            byte[] endBytes = endIpAddress.GetAddressBytes();

            bool isGreaterThanOrEqualStart = true;
            bool isLessThanOrEqualEnd = true;

            for (int i = 0; i < ipBytes.Length; i++)
            {
                if (ipBytes[i] < startBytes[i])
                {
                    isGreaterThanOrEqualStart = false;
                    break;
                }

                if (ipBytes[i] > endBytes[i])
                {
                    isLessThanOrEqualEnd = false;
                    break;
                }
            }

            return isGreaterThanOrEqualStart && isLessThanOrEqualEnd;
        }
        static IPAddress CalculateSubnetMask(int subnetBits)
        {
            if (subnetBits < 0 || subnetBits > 32)
            {
                throw new ArgumentOutOfRangeException("The number of bits to subnet should be between 0 and 32.");
            }

            uint subnetMaskValue = ~((1u << (32 - subnetBits)) - 1);
            byte[] maskBytes = BitConverter.GetBytes(subnetMaskValue);

            return new IPAddress(maskBytes.Reverse().ToArray());
        }
    }
}
