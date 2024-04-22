using System;
using System.Reflection;
using FingroUssd.Interfaces;
using FingroUssd.Traits;

namespace FingroUssd.Services
{
    public class MenuService: OnboardingTrait, IMenuService
    {
        public string[]? Render(string method, string phone, string session, string shortCode, string? text = null)
        {
            var methodInfo = GetType().GetMethod(method);

            if (methodInfo == null)
            {
                Console.WriteLine($"Unable to find method to invoke: {method}");
                //TODO: throw error if I don't find the method
                return new[]
                {
                    nameof(Render),
                    "end",
                    "Something went wrong, please try again later"
                };
            }

            var parameters = new object[] { phone, session, shortCode, text };
            return methodInfo.Invoke(this, parameters) as string[];

            //TODO: throw error if I am not able to invoke the method dynamically
        }
    }
}