namespace FingroUssd.Traits;

public class RepayLoanTrait: LoanStatementTrait
{
    public string[] RepayLoan(string phone, string session, string shortCode, string? text)
    {
        //TODO: add partial payment option
        const int loanBalance = 60000;

        var menu = new[]
        {
            nameof(RepayLoan),
            "con",
            $"Kindly confirm loan account you wish to repay\n\n1. Loan balance of KSH. {loanBalance}.00"
        };

        if(text is null) return menu;

        if (text != "1") return Default(menu);

        return CollectionOption(phone, session, shortCode, null);
    }

    public string[] CollectionOption(string phone, string session, string shortCode, string? text)
    {
        var menu = new[]
        {
            nameof(CollectionOption),
            "con",
            "Kindly confirm your collection option\n\n1. Mpesa\n0. Exit"
        };

        if(text is null) return menu;

        if (text == "0") return Exit();
        if (text != "1") return Default(menu);

        return LoanRepaymentMessage();
    }
    protected static string[] LoanRepaymentMessage()
    {
        return new[]
        {
            nameof(LoanRepaymentMessage),
            "end",
            "You will receive an M-Pesa pin prompt to facilitate your loan repayment. Thank you for growing with Fingro."
        };
    }
}