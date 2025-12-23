using System.Globalization;

namespace HabitTracker.Structure;

static class Helpers
{
    public static string AskDate()
    {
        Console.WriteLine("\nPlease insert the date: (Format: dd-mm-yy).");
        string? dateInput = Console.ReadLine();

        while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("\nInvalid date. Try again:");
            dateInput = Console.ReadLine();
        }

        return dateInput;
    }

    public static double AskQuantity()
    {
        Console.WriteLine("\nPlease insert quantity.");
        string? numberInput = Console.ReadLine();

        double finalInput;
        while (!double.TryParse(numberInput, NumberStyles.Any, CultureInfo.InvariantCulture, out finalInput) || finalInput < 0)
        {
            Console.WriteLine("\nInvalid number. Try again:");
            numberInput = Console.ReadLine();
        }

        return finalInput;
    }
}
