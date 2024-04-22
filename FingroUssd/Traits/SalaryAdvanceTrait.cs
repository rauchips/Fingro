using FingroUssd.Interfaces;
using Newtonsoft.Json.Linq;

namespace FingroUssd.Traits;

public class SalaryAdvanceTrait: ApplyLoanTrait
{
    public string[] SalaryAdvance(string phone, string session, string shortCode, string? text = null)
    {
        var menu = new[]
        {
            nameof(SalaryAdvance),
            "con",
            "What would you like to do?\n\n1. Apply for a loan\n2. Repay an existing loan\n3. View your loan statements\n0. Exit"
        };

        if(text is null) return menu;

        return text switch
        {
            "1" => ApplyLoan(phone, session, shortCode, null),
            "2" => RepayLoan(phone, session, shortCode, null),
            "3" => LoanStatement(phone, session, shortCode, null),
            "0" => Exit(),
            _ => Default(menu)
        };
    }
}