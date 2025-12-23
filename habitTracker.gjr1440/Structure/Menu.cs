namespace HabitTracker.Structure;

static class Menu
{
    public static void Run()
    {
        Console.Clear();

        while (true)
        {
            Console.WriteLine("\n\nMAIN MENU");

            Console.WriteLine("\nWhat would you like to do?");

            Console.WriteLine("\nType 0 to Close Application.");

            Console.WriteLine("\nType 1 to View All Habits."); // New options
            Console.WriteLine("Type 2 to Insert Habit.");

            Console.WriteLine("\nType 3 to View All Records.");
            Console.WriteLine("Type 4 to Insert Record.");
            Console.WriteLine("Type 5 to Delete Record.");
            Console.WriteLine("Type 6 to Update Record.");
            Console.WriteLine("----------------------------------------------\n");

            switch (Console.ReadLine())
            {
                case "0": return;

                case "1": Habits.List(); break;

                case "2": Habits.Insert(); break;

                case "3": Records.List(); break;

                case "4": Records.Insert(); break;

                case "5": Records.Delete(); break;

                case "6": Records.Update(); break;

                default: Console.WriteLine("\nInvalid Command. Please type a number from 0 to 6.\n"); break;
            }
        }
    }
}
