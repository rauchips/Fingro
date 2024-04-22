using System.ComponentModel.DataAnnotations;
using FingroUssd.Interfaces;
using Newtonsoft.Json.Linq;

namespace FingroUssd.Traits;

public class OnboardingTrait: LoginTrait
{
    public string[] Welcome(string phone, string session, string shortCode, string? text = null)
    {
        var menu = new[]
        {
            nameof(Welcome),
            "con",
            "Thank you for selecting FlashCredit. What would you like to do?\n\n1. Apply for a loan\n0. Exit"
        };

        if(text is null) return menu;

        return text switch
        {
            "1" => TermsAndConditions(phone, session, shortCode, null),
            "0" => Exit(),
            _ => Default(menu)
        };
    }

    public string[] TermsAndConditions(string phone, string session, string shortCode, string? text = null)
    {
        var menu = new[]
        {
            nameof(TermsAndConditions),
            "con",
            "By proceeding, you are accepting and are bound by Fingro's T&C on ke.fingro.io\n\n1. Accept and proceed\n0. Exit"
        };

        if (text is null) return menu;

        return text switch
        {
            "1" => EmailAddress(phone, session, shortCode, null),
            "0" => Exit(),
            _ => Default(menu)
        };
    }

    public string[] EmailAddress(string phone, string session, string shortCode, string? text = null)
    {
        var menu = new[]
        {
            nameof(EmailAddress),
            "con",
            "Enter your email address to register on Fingro Platform"
        };

        if (text is null) return menu;

        //validate email address
        var emailValidator = new EmailAddressAttribute();
        if (!emailValidator.IsValid(text)) return Default(menu);

        //if email is valid cache and return confirm email address menu

        var emailAddressKey = $"{phone}-{session}-{shortCode}-customer";

        var emailAddress = new JObject()
        {
            { "emailAddress", text}
        };

        SetData(emailAddressKey, emailAddress);

        return ConfirmEmailAddress(phone, session, shortCode, null);
    }

    public string[] ConfirmEmailAddress(string phone, string session, string shortCode, string? text = null)
    {
        var emailAddressKey = $"{phone}-{session}-{shortCode}-customer";

        var emailAddress = GetData<JObject>(emailAddressKey);

        //TODO: Find an optimal way of masking email address
        // var emailAddressPartOne = emailAddress.Split("@")[0];
        // var emailAddressPartTwo = emailAddress.Split("@")[1];
        //
        // var emailAddressPartOneLengthToMask = 3/5 * emailAddressPartOne.Length;
        //
        // emailAddress = emailAddress[..emailAddressPartOneLengthToMask] + new string('*', emailAddressPartOne.Length - emailAddressPartOneLengthToMask) + "@" + emailAddressPartTwo;

        var menu = new[]
        {
            nameof(ConfirmEmailAddress),
            "con",
            $"Confirm send link to complete your user profile and enjoy Fingro's loan services to email address: {emailAddress["emailAddress"]}\n1. Confirm\n2. Exit"
        };

        if (text is null) return menu;

        return text switch
        {
            "1" => SendOnboardingLink(),
            "0" => Exit(),
            _ => Default(menu)
        };
    }

    public string[] SendOnboardingLink()
    {
        return new[]
        {
            nameof(SendOnboardingLink),
            "end",
            "A link will be sent to you shortly, for a richer experience kindly interact with the link on your smart phone."
        };
    }
}