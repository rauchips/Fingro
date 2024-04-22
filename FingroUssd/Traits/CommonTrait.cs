using FingroUssd.Services;

namespace FingroUssd.Traits;

public class CommonTrait: CacheService
{
    protected static string[] Help(string phone, string session, string shortCode, string? text = null)
    {
        return new[]
        {
            nameof(Help),
            "end",
            "For any help please send us an email to support@ke.fingro.io for help."
        };
    }
    protected static string[] Exit()
    {
        return new[]
        {
            nameof(Exit),
            "end",
            "Thank you for growing with Fingro see you soon."
        };
    }

    protected static string[] Default(string[] menu)
    {
        return new[]
        {
            menu[0],
            menu[1],
            "Invalid input, please enter the correct input\n\n" + menu[2]
        };
    }
    protected static string[] PinBlock()
    {
        return new[]
        {
            nameof(PinBlock),
            "end",
            "Your account has been blocked. Send us an email to support@ke.fingro.io for help."
        };
    }
}