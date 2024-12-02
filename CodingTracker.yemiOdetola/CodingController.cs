using System.Configuration;
using System.Globalization;
using Spectre.Console;


namespace CodingTracker.yemiOdetola;
public class CodingController
{
  public static void Insert()
  {
    DateTime StartTime = UserInput.GetDateTimeInput(TimeType.StartTime);
    DateTime EndTime = UserInput.GetDateTimeInput(TimeType.EndTime);

    TimeSpan Duration = CalculateDuration(StartTime, EndTime);
    int DurationMinutes = (int)Duration.TotalMinutes;

    try
    {
      string connectionString = DbConnectionHelper.GetConnectionString();
      var dbQuery = new DbQuery(connectionString);
      dbQuery.CreateRecord(StartTime, EndTime, DurationMinutes);
      AnsiConsole.MarkupLine("[green] Record added successfully[/]");
    }
    catch (Exception ex)
    {
      AnsiConsole.MarkupLine($"[red]Error updating record: {ex.Message}[/]");
      return;
    }
    Console.Clear();
  }

  public static void Delete()
  {
    GetAllRecords();

    var recordId = UserInput.GetNumberInput("\nPlease type the Id of the record you want to delete or type 0 to go back to Main Menu\n");

    try
    {
      string connectionString = DbConnectionHelper.GetConnectionString();
      var dbQuery = new DbQuery(connectionString);
      dbQuery.DeleteRecord(recordId);
      AnsiConsole.MarkupLine($"[red]Record with Id {recordId} was deleted.[/]");
    }
    catch (Exception ex)
    {
      AnsiConsole.MarkupLine($"[red]Error deleting record: {ex.Message}[/]");
      return;
    }
    Console.Clear();
    UserInput.GetUserInput();
  }

  public static void Update()
  {
    GetAllRecords();

    var recordId = UserInput.GetNumberInput("\nPlease type Id of the record you would like to update. Type 0 to return to main menu.\n");

    try
    {
      string connectionString = DbConnectionHelper.GetConnectionString();
      var dbQuery = new DbQuery(connectionString);
      CodingSession? record = dbQuery.FetchSingleRecord(recordId);

      Console.WriteLine($"record starttime {record?.StartTime}");

      if (record == null)
      {
        AnsiConsole.MarkupLine($"[red]Record with Id {recordId} does not exist[/] \n \n");
      }
      else
      {
        DateTime StartConverted = DateTime.ParseExact(record.StartTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        DateTime EndConverted = DateTime.ParseExact(record.EndTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        DateTime StartTime = UserInput.GetDateTimeInput(TimeType.StartTime, StartConverted);
        DateTime EndTime = UserInput.GetDateTimeInput(TimeType.EndTime, EndConverted);
        TimeSpan Duration = CalculateDuration(StartTime, EndTime);
        int DurationMinutes = (int)Duration.TotalMinutes;

        dbQuery.UpdateRecord(recordId, StartTime, EndTime, DurationMinutes);
      }
    }
    catch (Exception ex)
    {
      AnsiConsole.WriteLine(ex.Message);
      AnsiConsole.MarkupLine($"[red]Unable to update record with Id: {recordId} \n");
    }
    Console.Clear();
  }

  public static void GetAllRecords()
  {
    try
    {
      string connectionString = DbConnectionHelper.GetConnectionString();
      var dbQuery = new DbQuery(connectionString);
      List<CodingSession> tableData = dbQuery.FetchAllRecords();
      foreach (var record in tableData)
      {
        AnsiConsole.MarkupLine($"[purple]{record.Id} - StartTime: {record.StartTime} EndTime: {record.EndTime} - Duration: {record.Duration} minutes \n[/]");
      }
    }
    catch (Exception ex)
    {
      AnsiConsole.WriteLine(ex.Message);
      AnsiConsole.MarkupLine($"[red]No records found!.[/]");
    }
  }

  public static TimeSpan CalculateDuration(DateTime StartTime, DateTime EndTime)
  {
    if (EndTime < StartTime)
    {
      Console.WriteLine("\nEnd time cannot be earlier than the start time. Please enter valid times.");
      return TimeSpan.Zero;
    }
    return EndTime.Subtract(StartTime);
  }


}
