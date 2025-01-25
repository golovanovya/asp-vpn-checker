using System.Net;

namespace asp_vpn_checker.Services;

public class VPNChecker : IVPNChecker
{
    private List<Tuple<UInt32, UInt32>> _inets = [];
    private readonly ILogger<VPNChecker> _logger;

    private static uint ConvertIpAddressToUint(IPAddress ip)
    {
        byte[] bytes = ip.GetAddressBytes();
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes);
        }
        return BitConverter.ToUInt32(bytes, 0);
    }

    private int BinarySearch(IPAddress ip)
    {
        UInt32 uintAddress = ConvertIpAddressToUint(ip);
        int left = 0;
        int right = _inets.Count - 1;
        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            if (_inets[mid].Item1 <= uintAddress && _inets[mid].Item2 >= uintAddress)
            {
                return mid;
            }
            else if (_inets[mid].Item1 < uintAddress)
            {
                left = mid + 1;
            }
            else
            {
                right = mid - 1;
            }
        }
        return -1;
    }

    private List<Tuple<UInt32, UInt32>> Parse(String path)
    {
        System.Collections.Generic.IEnumerable<String> lines = File.ReadLines(path);
        List<Tuple<UInt32, UInt32>> inets = [];
        foreach (String line in lines)
        {
            try
            {
                IPNetwork2 inet = IPNetwork2.Parse(line);
                UInt32 start = ConvertIpAddressToUint(inet.Network);
                UInt32 end = ConvertIpAddressToUint(inet.Broadcast);
                inets.Add(new Tuple<UInt32, UInt32>(start, end));
            }
            catch (System.Exception)
            {
                _logger.LogError($"Can't parse address: {line}");
            }
        }
        return inets;
    }

    private List<Tuple<UInt32, UInt32>> Merge(List<Tuple<UInt32, UInt32>> list1, List<Tuple<UInt32, UInt32>> list2)
    {
        List<Tuple<UInt32, UInt32>> merged = [];
        int i = 0;
        int j = 0;
        while (i < list1.Count && j < list2.Count)
        {
            if (list1[i].Item1 < list2[j].Item1)
            {
                merged.Add(list1[i]);
                i++;
            }
            else if (list1[i].Item1 > list2[j].Item1)
            {
                merged.Add(list2[j]);
                j++;
            }
            else
            {
                if (list1[i].Item2 < list2[j].Item2)
                {
                    merged.Add(new Tuple<UInt32, UInt32>(list1[i].Item1, list2[j].Item2));
                }
                else
                {
                    merged.Add(new Tuple<UInt32, UInt32>(list1[i].Item1, list2[j].Item2));
                }
                i++;
                j++;
            }
        }
        while (i < list1.Count)
        {
            merged.Add(list1[i]);
            i++;
        }
        while (j < list2.Count)
        {
            merged.Add(list2[j]);
            j++;
        }
        return merged;
    }

    public VPNChecker(ILogger<VPNChecker> logger)
    {
        _logger = logger;
        List<String> paths = [
            "data/josephrocca-vpn-or-datacenter-ipv4-ranges.txt",
            "data/X4BNet-datacenter.txt",
            "data/X4BNet-main.txt",
            "data/X4BNet-vpn.txt",
        ];
        List<Tuple<UInt32, UInt32>> current;
        foreach (var path in paths) {
            current = Parse(path);
            _inets = Merge(_inets, current);
        }
        
        _logger.LogInformation($"Loaded {_inets.Count()} VPN ranges.");
    }

    public bool IsVPN(IPAddress ip)
    {
        return BinarySearch(ip) != -1;
    }
}
