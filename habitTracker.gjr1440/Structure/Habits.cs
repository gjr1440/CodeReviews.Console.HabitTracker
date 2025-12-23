using HabitTracker.Entities;
using Microsoft.Data.Sqlite;

namespace HabitTracker.Structure;

static class Habits
{
    public static void List()
    {
        Console.Clear();

        using var connection = Database.Open();
        var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT * FROM habits";

        List<Habit> tableData = [];

        SqliteDataReader reader = cmd.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                tableData.Add(
                new Habit
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Measure = reader.GetString(2)
                });
            }
        }

        connection.Close();

        Console.WriteLine("\t\tHABITS");
        Console.WriteLine("-----------------------------------------");
        if (tableData.Count != 0)
        {
            foreach (var data in tableData)
            {
                Console.WriteLine($"Id: {data.Id} - Name: {data.Name} - Measure: {data.Measure}");
            }
        }
        else
            Console.WriteLine("[No rows found]");

        Console.WriteLine("-----------------------------------------");
    }

    public static void Insert()
    {
        string? name = "";
        string? measure = "";

        var question = new
        {
            name = "\nWhat habit do you want to insert? Type 0 if you want to go back to the Main Menu.",
            measure = "\nWhat unit of measurement would you like to use for this habit? For example: \r\nhours, liters, pages, etc."
        };

        bool nameConfirmed = false;
        while (nameConfirmed == false)
        {
            Console.WriteLine(question.name);
            name = Console.ReadLine()?.ToUpper();
            if (!string.IsNullOrEmpty(name))
            {
                // -----------------------------
                if (name == "0") return;
                // -----------------------------

                while (true)
                {
                    Console.Write($"Do you confirm: {name}? (y/n) ");
                    string? confirmName = Console.ReadLine();
                    if (confirmName?.ToLower() == "y")
                    {
                        nameConfirmed = true;
                        break;
                    }
                    else if (confirmName?.ToLower() == "n")
                        break;
                }
            }
            else
                Console.WriteLine("Habit must have name. Try again.");
        }

        Console.WriteLine("\n[HABIT CREATED]");

        bool measureConfirmed = false;
        while (measureConfirmed == false)
        {
            Console.WriteLine(question.measure);
            measure = Console.ReadLine()?.ToUpper();
            if (!string.IsNullOrEmpty(measure))
            {
                while (true)
                {
                    Console.Write($"Do you confirm: {measure}? (y/n) ");
                    string? confirmMeasure = Console.ReadLine();
                    if (confirmMeasure?.ToLower() == "y")
                    {
                        measureConfirmed = true;
                        break;
                    }
                    else if (confirmMeasure?.ToLower() == "n")
                        break;
                }
            }
            else
                Console.WriteLine("Habit must have a measure. Try again.");
        }

        using var connection = Database.Open();
        var cmd = connection.CreateCommand();
        cmd.CommandText = "INSERT INTO habits(name, measure) VALUES(@name, @measure)";
        cmd.Parameters.AddWithValue("@name", name);
        cmd.Parameters.AddWithValue("@measure", measure);

        cmd.ExecuteNonQuery();

        connection.Close();

        Console.WriteLine("\n[HABIT INSERTED]");
    }
    
    public static int AskId()
    { 
        while (true)
        {
            Console.WriteLine("\nPlease insert the Id of the habit which you want to add a record. Type 0 to return to Main Menu.");
            string? idInput = Console.ReadLine();

            if (idInput == "0") return 0;

            if (!int.TryParse(idInput, out int id) || id <= 0)
            {
                Console.WriteLine("Invalid Id. Try again.");
                continue;
            }

            using var connection = Database.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = $"SELECT id FROM habits WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            var exists = cmd.ExecuteScalar();

            if (exists != null) return id;

            Console.WriteLine($"\nHabit with Id {id} doesn't exist.");
        }
    }
}
