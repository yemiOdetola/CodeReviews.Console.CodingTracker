using System.Configuration;
using Spectre.Console;


namespace CodingTracker.yemiOdetola;
public class CodingController
{
  static string? ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
  public static void Insert()
  {
    DateTime StartTime = UserInput.GetDateTimeInput(TimeType.StartTime);
    DateTime EndTime = UserInput.GetDateTimeInput(TimeType.EndTime);

    TimeSpan Duration = CalculateDuration(StartTime, EndTime);
    int DurationMinutes = (int)Duration.TotalMinutes;

    try
    {
      DbQuery.CreateRecord(StartTime, EndTime, DurationMinutes);
      AnsiConsole.MarkupLine("[green]Record added successfully[/]");
    }
    catch (Exception ex)
    {
      AnsiConsole.MarkupLine($"[red]Error updating record: {ex.Message}[/]");
      return;
    }
  }

  public static void Delete()
  {
    Console.Clear();
    GetAllRecords();

    var recordId = UserInput.GetNumberInput("\nPlease type the Id of the record you want to delete or type 0 to go back to Main Menu\n");

    try
    {
      DbQuery.DeleteRecord(recordId);
      AnsiConsole.MarkupLine($"[red]Record with Id {recordId} was deleted.[/]");
    }
    catch (Exception ex)
    {
      AnsiConsole.MarkupLine($"[red]Error deleting record: {ex.Message}[/]");
      return;
    }
    UserInput.GetUserInput();
  }

  public static void Update()
  {
    GetAllRecords();

    var recordId = UserInput.GetNumberInput("\nPlease type Id of the record you would like to update. Type 0 to return to main menu.\n");

    try
    {
      int checkQuery = DbQuery.FetchSingleRecord(recordId);
      if (checkQuery == 0)
      {
        AnsiConsole.MarkupLine($"[red]Record with Id {recordId} does not exist[/] \n \n");
      }
      else
      {
        DateTime StartTime = UserInput.GetDateTimeInput(TimeType.StartTime);
        DateTime EndTime = UserInput.GetDateTimeInput(TimeType.EndTime);
        TimeSpan Duration = CalculateDuration(StartTime, EndTime);
        int DurationMinutes = (int)Duration.TotalMinutes;

        DbQuery.UpdateRecord(recordId, StartTime, EndTime, DurationMinutes);
      }
    }
    catch (Exception ex)
    {
      AnsiConsole.WriteLine(ex.Message);
      AnsiConsole.MarkupLine($"[red]Unable to update record with Id: {recordId} \n");
    }
  }

  public static void GetAllRecords()
  {
    Console.Clear();
    try
    {
      List<CodingSession> tableData = DbQuery.FetchAllRecords();
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
    return EndTime.Subtract(StartTime);
  }


}
