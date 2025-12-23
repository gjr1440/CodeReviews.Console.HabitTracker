using Microsoft.Data.Sqlite;

namespace HabitTracker.Structure;

static class Database
{
    private static string _connectionString = @"Data Source=habit-Tracker.db";

    public static SqliteConnection Open()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }

    public static void Init()
    {
        using var connection = Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText =
        @"CREATE TABLE IF NOT EXISTS habits (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT,
            measure TEXT
            );

          INSERT INTO habits(name, measure) 
          VALUES
          ('READING', 'PAGES'),
          ('DRINKING_WATER', 'LITERS'),
          ('WALKING', 'STEPS'),
          ('STUDYING', 'HOURS');;

          CREATE TABLE IF NOT EXISTS records (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            habit_id INTEGER,
            date TEXT,
            quantity REAL,
            FOREIGN KEY (habit_id) REFERENCES habits(id)
            );

          INSERT INTO records(habit_id, date, quantity) 
          VALUES
          (1, '23-12-25', 15),
          (2, '23-12-25', 2000),
          (3, '23-12-25', 5000),
          (4, '23-12-25', 1.5);";

        cmd.ExecuteNonQuery();
    }
}
