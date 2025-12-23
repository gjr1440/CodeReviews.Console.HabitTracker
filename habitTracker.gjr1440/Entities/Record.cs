namespace HabitTracker.Entities;

public class Record
{
    public int Id { get; set; }
    public int HabitId { get; set; }
    public DateTime Date { get; set; }
    public double Quantity { get; set; }
}
