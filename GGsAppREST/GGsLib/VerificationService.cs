using Serilog;
using System;
using System.Text.RegularExpressions;
namespace GGsLib
{
    public static class VerificationService
    {
        public static void InvalidInput()
        {
            Log.Warning("Invalid input. Please select another option.");
            Console.WriteLine("Invalid input. Please select another option.");
        }

        public static bool IsValidEmail(string email)
        {
            if (Regex.IsMatch(email, @"\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}\b", RegexOptions.IgnoreCase))
            {
                return true;
            }
            else
            {
                Log.Warning("Entered invalid email");
                Console.WriteLine("Please enter a valid email.");
                return false;
            }
        }

        public static bool IsValidName(string name)
        {
            if (Regex.IsMatch(name, @"[\\d]", RegexOptions.IgnoreCase))
                return true;
            else
            {
                Log.Warning("Entered invalid name");
                Console.WriteLine("Please enter a valid name.");
                return false;
            }
        }

        public static bool IsValidQuantity(int locQuantity, int reqQuantity)
        {
            if (reqQuantity > locQuantity)
            {
                Log.Error("User selected more quantity than what is available");
                Console.WriteLine($"You cannot select more than the {locQuantity} available products.");
                return false;
            }
            return true;
        }
    }
}