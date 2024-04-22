namespace FingroUssd.Interfaces;

public interface IMenuService
{
    /// <summary>
    /// Calls USSD menus dynamically
    /// </summary>
    /// <param name="method"></param>
    /// <param name="shortCode"></param>
    /// <param name="text"></param>
    /// <param name="phone"></param>
    /// <param name="session"></param>
    /// <returns>Array with three elements => 0: method name | 1: con or end | 2: message</returns>
    public string[]? Render(string method, string phone, string session, string shortCode, string? text = null);
}