using System.Net;

namespace asp_vpn_checker.Services;

public interface IVPNChecker {
    bool IsVPN(IPAddress ip);
}
