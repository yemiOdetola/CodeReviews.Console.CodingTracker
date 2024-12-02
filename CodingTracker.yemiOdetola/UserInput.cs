using System;
using System.Globalization;
using Spectre.Console;

namespace CodingTracker.yemiOdetola;

public class UserInput
{
  public static void GetUserInput()
  {
    Console.Clear();
    bool closeApp = false;
    while (closeApp == false)
    {
      Console.WriteLine("MAIN MENU \n");
      Console.WriteLine("What would you like to do? \n");
      Console.WriteLine("Enter 0 to Close Application. \n");
      Console.WriteLine("Enter 1 to View All Records.");
      Console.WriteLine("Enter 2 to Insert Record.");
      Console.WriteLine("Enter 3 to Delete Record.");
      Console.WriteLine("Enter 4 to Update Record.");

      string? userInput = Console.ReadLine();

      switch (userInput)
      {
        case "0":
          Console.WriteLine("\nGoodbye!\n");
          closeApp = true;
          Environment.Exit(0);
          break;
        case "1":
          CodingController.GetAllRecords();
          break;
        case "2":
          CodingController.Insert();
          break;
        case "3":
          CodingController.Delete();
          break;
        case "4":
          CodingController.Update();
          break;
        default:
          Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
          break;
      }
    }
  }

  public static DateTime GetDateTimeInput(TimeType timeType, DateTime? existingValue = null)
  {
    string label = timeType == TimeType.StartTime ? "start" : "end";

    string datePrompt = existingValue.HasValue
        ? $"\nEnter the {label} date (Format: dd-MM-yyyy) or press Enter to keep [{existingValue.Value:dd-MM-yyyy}]:"
        : $"\nEnter the {label} date (Format: dd-MM-yyyy).\nType 00 to set as today.\nType 0 to return to the main menu.";

    Console.WriteLine(datePrompt);
    string? dateInput = Console.ReadLine();

    DateTime date;

    while (true)
    {
      if (string.IsNullOrWhiteSpace(dateInput) && existingValue.HasValue)
      {
        date = existingValue.Value.Date;
        break;
      }

      if (dateInput == "0")
      {
        GetUserInput();
        return DateTime.MinValue;
      }

      if (dateInput == "00")
      {
        date = DateTime.Today;
        break;
      }

      if (DateTime.TryParseExact(dateInput, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
      {
        break;
      }
      dateInput = AnsiConsole.Prompt(new TextPrompt<string>("\nInvalid date. Format: dd-MM-yyyy. Try again:"));
      Console.WriteLine(dateInput);
    }

    string timePrompt = existingValue.HasValue
        ? $"\nEnter the {label} time (Format: HH:mm) or press Enter to keep [{existingValue.Value:HH:mm}]:"
        : $"\nEnter the {label} time (Format: HH:mm):";

    Console.WriteLine(timePrompt);
    string? timeInput = Console.ReadLine();
    TimeSpan time;

    while (true)
    {
      if (string.IsNullOrWhiteSpace(timeInput) && existingValue.HasValue)
      {
        time = existingValue.Value.TimeOfDay;
        break;
      }

      if (TimeSpan.TryParseExact(timeInput, "hh\\:mm", CultureInfo.InvariantCulture, out time))
      {
        break;
      }

      Console.WriteLine("\nInvalid time. Format: HH:mm. Try again:");
      timeInput = Console.ReadLine();
    }

    return date.Add(time);
  }


  public static int GetNumberInput(string message)
  {
    Console.WriteLine(message);

    string? numberInput = Console.ReadLine();

    if (numberInput == "0") GetUserInput();

    while (!int.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
    {
      Console.WriteLine("\nInvalid number. Try again.\n");
      numberInput = Console.ReadLine();
    }

    int finalInput = Convert.ToInt32(numberInput);

    return finalInput;
  }

}


public enum TimeType
{
  StartTime,
  EndTime
}