using System.Globalization;

namespace FingroUssd.Traits;

public class LoanStatementTrait: ResetPinTrait
{
    public string[] LoanStatement(string phone, string session, string shortCode, string? text)
    {
        var menu = new[]
        {
            nameof(LoanStatement),
            "con",
            "1. Full Statement\n2. Mini Statement"
        };

        if(text is null) return menu;

        return text switch
        {
            "1" => FullStatementFromDate(phone, session, shortCode, null),
            "2" => MiniStatement(phone, session, shortCode, null),
            "0" => Exit(),
            _ => Default(menu)
        };
    }
    public string[] FullStatementFromDate(string phone, string session, string shortCode, string? text)
    {
        var menu = new[]
        {
            nameof(FullStatementFromDate),
            "con",
            "Enter start date (YYYY-MM-DD)"
        };

        if(text is null) return menu;

        string[] dateFormat =
        {
            "YYYY-MM-DD"
        };

        return DateTime.TryParseExact(text, dateFormat,
            new CultureInfo("en-US"),
            DateTimeStyles.None,
            out var dateValue) ? FullStatementToDate(phone, session, shortCode, null) : Default(menu);
    }

    public string[] FullStatementToDate(string phone, string session, string shortCode, string? text)
    {
        var menu = new[]
        {
            nameof(FullStatementToDate),
            "con",
            "Enter end date (YYYY-MM-DD)"
        };

        if(text is null) return menu;

        string[] dateFormat =
        {
            "YYYY-MM-DD"
        };

        return DateTime.TryParse(text,
            new CultureInfo("en-US"),
            DateTimeStyles.None,
            out var dateValue) ? EnterPinFull(phone, session, shortCode, null) : Default(menu);
    }
    public string[] EnterPinFull(string phone, string session, string shortCode, string? text = null)
    {
        var menu = new[]
        {
            nameof(EnterPinFull),
            "con",
            "Enter your Fingro pin to confirm your full statement request. (3 trials remaining)"
        };

        if(text is null) return menu;

        var isNumber = int.TryParse(text, out var n);
        if (!isNumber || text.Length != 4) return Default(menu);

        return Convert.ToInt32(text) != 1234 ? PinBlock() : FullStatementMessage();
    }
    public string[] EnterPinMini(string phone, string session, string shortCode, string? text = null)
    {
        var menu = new[]
        {
            nameof(EnterPinMini),
            "con",
            "Enter your Fingro pin to confirm your mini statement request. (3 trials remaining)"
        };

        if(text is null) return menu;

        var isNumber = int.TryParse(text, out var n);
        if (!isNumber || text.Length != 4) return Default(menu);

        return Convert.ToInt32(text) != 1234 ? PinBlock() : FullStatementMessage();
    }
    public string[] MiniStatement(string phone, string session, string shortCode, string? text)
    {
        return EnterPinMini(phone, session, shortCode, null);
    }
    protected static string[] FullStatementMessage()
    {
        return new[]
        {
            nameof(FullStatementMessage),
            "end",
            "Your request has been received. Kindly be patient as we send your statement via EMAIL"
        };
    }
    protected static string[] MiniStatementMessage()
    {
        return new[]
        {
            nameof(MiniStatementMessage),
            "end",
            "Your request has been received. Kindly be patient as we send your statement via SMS"
        };
    }
}