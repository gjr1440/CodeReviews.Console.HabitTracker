using HabitTracker.Entities;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTracker.Structure;

static class Records
{
    public static void List()
    {
        Console.Clear();
        
        using var connection = Database.Open();
        var cmd = connection.CreateCommand();
        cmd.CommandText = $"SELECT * FROM records";

        List<Record> tableData = [];

        SqliteDataReader reader = cmd.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                tableData.Add(
                new Record
                {
                    Id = reader.GetInt32(0),
                    HabitId = reader.GetInt32(1),
                    Date = DateTime.ParseExact(reader.GetString(2), "dd-MM-yy", new CultureInfo("en-US")),
                    Quantity = reader.GetDouble(3)
                });
            }
        }

        connection.Close();

        Console.WriteLine("\t\tRECORDS");
        Console.WriteLine("-----------------------------------------");
        if (tableData.Count != 0)
        {
            foreach (var data in tableData)
            {
                Console.WriteLine($"Id: {data.Id} - HabitId: {data.HabitId} - Date: {data.Date.ToString("dd-MMM-yyyy")} - Quantity: {data.Quantity.ToString(CultureInfo.InvariantCulture)}");
            }
        }
        else
            Console.WriteLine("[No rows found]");

        Console.WriteLine("-----------------------------------------");
    }

    public static void Insert()
    {
        Console.Clear();
        Habits.List();

        int habitId = Habits.AskId();
        // -----------------------------
        if (habitId == 0) return;
        // -----------------------------
        string date = Helpers.AskDate();
        double quantity = Helpers.AskQuantity();
        
        using var connection = Database.Open();
        var cmd = connection.CreateCommand();
        cmd.CommandText =
            "INSERT INTO records(habit_id, date, quantity) VALUES(@habitId, @date, @quantity)";
        cmd.Parameters.AddWithValue("@habitId", habitId);
        cmd.Parameters.AddWithValue("@date", date);
        cmd.Parameters.AddWithValue("@quantity", quantity);

        cmd.ExecuteNonQuery();

        connection.Close();
    }

    public static void Delete()
    {
        Console.Clear();
        List();

        int recordId = AskId();

        using var connection = Database.Open();
        var cmd = connection.CreateCommand();
        cmd.CommandText = "DELETE FROM records WHERE id = @id";
        cmd.Parameters.AddWithValue("@id", recordId);

        cmd.ExecuteNonQuery();
    }

    public static void Update()
    {
        List();

        int recordId = AskId();

        using var connection = Database.Open();

        var checkCmd = connection.CreateCommand();
        checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM records WHERE id = @id)";
        checkCmd.Parameters.AddWithValue("@id", recordId);
        checkCmd.ExecuteScalar();

        string date = Helpers.AskDate();
        double quantity = Helpers.AskQuantity();

        var cmd = connection.CreateCommand();
        cmd.CommandText =
            $"UPDATE records SET date = @date, quantity = @quantity WHERE id = @id";
        cmd.Parameters.AddWithValue("@date", date);
        cmd.Parameters.AddWithValue("@quantity", quantity);
        cmd.Parameters.AddWithValue("@id", recordId);

        cmd.ExecuteNonQuery();

        connection.Close();
    }

    public static int AskId()
    {
        while (true)
        {
            Console.WriteLine("\nPlease type the Id of the record you want. Type 0 and go to Main Menu.");
            string? idInput = Console.ReadLine();

            if (idInput == "0") return 0;

            if (!int.TryParse(idInput, out int id) || id <= 0)
            {
                Console.WriteLine("Invalid Id. Try again.");
                continue;
            }

            using var connection = Database.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = $"SELECT id FROM records WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", idInput);

            var exists = cmd.ExecuteScalar();

            if (exists != null) return id;

            Console.WriteLine($"\nRecord with Id {id} doesn't exist.");
        }
    }
}
