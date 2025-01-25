using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using asp_vpn_checker.Models;
using asp_vpn_checker.Services;
using System.Net;

namespace asp_vpn_checker.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IVPNChecker _vpnChecker;

    public HomeController(IVPNChecker vpnChecker, ILogger<HomeController> logger)
    {
        _logger = logger;
        _vpnChecker = vpnChecker;
    }

    public IActionResult Index()
    {
        IPAddress? ip = Request.HttpContext.Connection.RemoteIpAddress;
        bool isVpn = false;
        if (ip != null)
        {
            isVpn = _vpnChecker.IsVPN(ip);
        }
        var jsonData = new {
            isVpn,
            isIpSum = false,
        };
        return new JsonResult(jsonData);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
