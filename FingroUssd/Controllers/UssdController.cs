using System.Globalization;
using FingroUssd.Interfaces;
using FingroUssd.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace FingroUssd.Controllers;

[ApiController]
public class UssdController : Controller
{
    private readonly IMenuService _menuService;
    private readonly ICacheService _cacheService;
    private static int _lifetime;

    public UssdController(IMenuService menuService, ICacheService cacheService, IConfiguration configuration)
    {
        _menuService = menuService;
        _cacheService = cacheService;
        _lifetime = configuration.GetValue<int>("Lifetime");
    }
    [Route("ussd/v1/{phone}/{session}/{shortCode}/{text?}")]
    [HttpGet]
    public string Serve([FromRoute] string phone, [FromRoute] string session, [FromRoute] string shortCode, [FromRoute] string? text = null)
    {
        //TODO: Log request received
        //TODO: Declare USSD key variables: session | customer | menu | start | end |
        //TODO: Check if session exists | Check session start time | Check session end time
        //TODO: Fetch customer profile
        //TODO: If customer profile does not exist return registration menu
        //TODO: If customer profile exists and this is their first dial return the login menu
        //TODO: If customer profile exists and they have an existing session fetch method menu in cache and return that menu

        Console.WriteLine(DateTime.Now.ToString(CultureInfo.InvariantCulture).Replace("/", "-") + "\t" + "--------------------START--------------------");

        //If session time exists but it has exceeded its lifetime clear cache so that the user can start session again
        //return Cache::get("{$serviceCode}.{$sessionId}.{$phoneNumber}.{$key}");
        var sessionKey = $"{phone}-{session}-{shortCode}-session";
        var startKey = $"{phone}-{session}-{shortCode}-start";
        var endKey = $"{phone}-{session}-{shortCode}-end";
        var methodKey = $"{phone}-{session}-{shortCode}-method";
        var customerKey = $"{phone}-{session}-{shortCode}-customer";

        if (_cacheService.HasData<string>(sessionKey) && (_cacheService.GetData<DateTime>(endKey) - _cacheService.GetData<DateTime>(startKey)).Seconds > Convert.ToInt32(_lifetime))
        {
            _cacheService.RemoveData(sessionKey);
            _cacheService.RemoveData(startKey);
            _cacheService.RemoveData(endKey);
            _cacheService.RemoveData(methodKey);
            _cacheService.RemoveData(customerKey);
        }

        var method = _cacheService.GetData<string>(methodKey);
        var customer = _cacheService.GetData<JObject>(customerKey);

        if (!_cacheService.HasData<string>(sessionKey))
        {
            _cacheService.SetData(sessionKey, session);
            _cacheService.SetData(startKey, DateTimeOffset.Now);
            _cacheService.SetData(endKey, DateTimeOffset.Now.AddSeconds(_lifetime));

            method = "Welcome";
            //method = "Login";
            text = null;
        }

        Console.WriteLine(DateTime.Now.ToString(CultureInfo.InvariantCulture).Replace("/", "-") + "\t" + "USSD Request" + "\t" + $"Phone: {phone}   |   Session: {session}  |   Short Code: {shortCode}    |   Text:    {text}     |    Method:     {method}");

        var render = _menuService.Render(method, phone, session, shortCode, text);

        var name = render[0];
        var action = render[1];
        var message = render[2];

        if (action == "end")
        {
            _cacheService.RemoveData(sessionKey);
            _cacheService.RemoveData(startKey);
            _cacheService.RemoveData(endKey);
            _cacheService.RemoveData(methodKey);
            _cacheService.RemoveData(customerKey);
        }
        else
        {
            _cacheService.SetData(methodKey, name);
        }


        Console.WriteLine(DateTime.Now.ToString(CultureInfo.InvariantCulture).Replace("/", "-") + "\t" + "--------------------END--------------------");

        return action + " " + message;

    }
}