namespace FingroUssd.Traits;

public class LoginTrait: HomeTrait
{
    public string[] Login(string phone, string session, string shortCode, string? text = null)
    {
        const string profileFullName = "Benjamin Chipinde";
        var menu = new[]
        {
            nameof(Login),
            "con",
            $"Hello {profileFullName}\n\n1. Proceed\n0. Exit\n\n\n"
        };

        if(text is null) return menu;

        return text switch
        {
            "1" => Pin(phone, session, shortCode, null),
            "0" => Exit(),
            _ => Default(menu)
        };
    }

    public string[] Pin(string phone, string session, string shortCode, string? text = null)
    {
        var menu = new[]
        {
            nameof(Pin),
            "con",
            "Kindly enter your pin to proceed. (3 trials remaining)"
        };

        if(text is null) return menu;

        var isNumber = int.TryParse(text, out var n);
        if (!isNumber || text.Length != 4) return Default(menu);

        return Convert.ToInt32(text) != 1234 ? PinBlock() : Home(phone, session, shortCode, null);
    }
}