using FingroUssd.Interfaces;
using FingroUssd.Services;

namespace FingroUssd.Traits;

public class ResetPinTrait: CommonTrait
{
    public string[] OldPin(string phone, string session, string shortCode, string? text = null)
    {
        var menu = new[]
        {
            nameof(OldPin),
            "con",
            "Enter your old pin"
        };

        if (text is null) return menu;

        var isNumber = int.TryParse(text, out var n);
        if (!isNumber || text.Length != 4) return Default(menu);

        return Convert.ToInt32(text) != 1234 ? PinBlock() : NewPin(phone, session, shortCode, null);
    }

    public string[] NewPin(string phone, string session, string shortCode, string? text = null)
    {
        var menu = new[]
        {
            nameof(NewPin),
            "con",
            "Enter your new pin"
        };

        if (text is null) return menu;

        var isNumber = int.TryParse(text, out var n);
        if (!isNumber || text.Length != 4) return Default(menu);

        return Convert.ToInt32(text) != 4321 ? PinBlock() : ConfirmPin(phone, session, shortCode, null);
    }

    public string[] ConfirmPin(string phone, string session, string shortCode, string? text = null)
    {
        //TODO: handle confirm pin accordingly or in a better and smoother way
        var menu = new[]
        {
            nameof(ConfirmPin),
            "con",
            "Confirm your new pin"
        };

        if (text is null) return menu;

        var isNumber = int.TryParse(text, out var n);
        if (!isNumber || text.Length != 4) return Default(menu);

        return Convert.ToInt32(text) != 4321 ? PinBlock() : PinUpdated();
    }

    protected static string[] PinUpdated()
    {
        return new[]
        {
            nameof(PinUpdated),
            "end",
            "Pin reset was successful"
        };
    }
}