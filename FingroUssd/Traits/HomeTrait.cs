using FingroUssd.Services;

namespace FingroUssd.Traits;

public class HomeTrait : SalaryAdvanceTrait
{
    public string[] Home(string phone, string session, string shortCode, string? text = null)
    {
        var menu = new[]
        {
            nameof(Home),
            "con",
            "Welcome back to Fingro\n\n1. Salary advance loan\n2. Reset pin\n3. Help\n0. Exit"
        };

        if(text is null) return menu;

        return text switch
        {
            "1" => SalaryAdvance(phone, session, shortCode, null),
            "2" => OldPin(phone, session, shortCode, null),
            "3" => Help(phone, session, shortCode, null),
            "0" => Exit(),
            _ => Default(menu)
        };
    }
}