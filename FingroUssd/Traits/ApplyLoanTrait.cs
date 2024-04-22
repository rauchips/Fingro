using Newtonsoft.Json.Linq;

namespace FingroUssd.Traits;

public class ApplyLoanTrait: RepayLoanTrait
{
    public string[] ApplyLoan(string phone, string session, string shortCode, string? text)
    {
        const double loanLimit = 50000.00;

        var menu = new[]
        {
            nameof(ApplyLoan),
            "con",
            $"Your salary advance loan limit is KSH. {loanLimit}\nHow much do you want to apply for?"
        };

        if(text is null) return menu;

        var isNumber = int.TryParse(text, out var n);
        if (!isNumber || n > loanLimit) return Default(menu);

        var customerKey = $"{phone}-{session}-{shortCode}-customer";
        var customer = GetData<JObject>(customerKey) ?? new JObject();
        customer.Add("loan", text);
        SetData(customerKey, customer);

        return DisbursementOption(phone, session, shortCode, null);
    }

    public string[] DisbursementOption(string phone, string session, string shortCode, string? text)
    {
        var menu = new[]
        {
            nameof(DisbursementOption),
            "con",
            "Kindly confirm your disbursement option\n\n1. Mpesa\n0. Exit"
        };

        if(text is null) return menu;

        if (text == "0") return Exit();
        if (text != "1") return Default(menu);

        //TODO: what if fetch from db fails, handle that error as well
        var customerKey = $"{phone}-{session}-{shortCode}-customer";
        var customer = GetData<JObject>(customerKey) ?? new JObject();
        customer.Add("disbursement", "Mpesa");
        SetData(customerKey, customer);

        return ConfirmLoan(phone, session, shortCode, null);
    }
    public string[] ConfirmLoan(string phone, string session, string shortCode, string? text)
    {
        if (text == "0") return Exit();

        if(text == "1")  return EnterPin(phone, session, shortCode, null);

        //TODO: call cache only if valid to call - get rid of return switch statement
        //TODO: what if fetch from db fails, handle that error as well
        var customerKey = $"{phone}-{session}-{shortCode}-customer";
        var customer = GetData<JObject>(customerKey);

        //TODO: calculate loan term and interest fee at service level
        const int loanTerm = 30;
        const int interestFee = 20;
        var interest = interestFee * (int)(customer["loan"] ?? throw new InvalidOperationException()) / 100;

        var loanBalance = (int)(customer["loan"] ?? throw new InvalidOperationException()) + interest;
        var disbursementOption = customer["disbursement"];
        var menu = new[]
        {
            nameof(ConfirmLoan),
            "con",
            $"Kindly confirm your salary advance loan\nLoan Balance - KSH. {loanBalance}.00\nLoan Term - {loanTerm} days\nInterest Fee - {interestFee}%\nDisbursement Option - {disbursementOption}\n\n1. Agree\n0. Exit"
        };

        if (text != null) return Default(menu);

        return menu;
    }
    public string[] EnterPin(string phone, string session, string shortCode, string? text = null)
    {
        var menu = new[]
        {
            nameof(EnterPin),
            "con",
            "Enter your Fingro pin to confirm your loan request. (3 trials remaining)"
        };

        if(text is null) return menu;

        var isNumber = int.TryParse(text, out var n);
        if (!isNumber || text.Length != 4) return Default(menu);

        return Convert.ToInt32(text) != 1234 ? PinBlock() : LoanRequestMessage();
    }
    protected static string[] LoanRequestMessage()
    {
        return new[]
        {
            nameof(LoanRequestMessage),
            "end",
            "Your loan will be disbursed to you shortly, thank you for growing with Fingro."
        };
    }
}