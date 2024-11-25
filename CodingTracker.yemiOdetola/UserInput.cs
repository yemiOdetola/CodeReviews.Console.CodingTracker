using System;
using System.Globalization;

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
      Console.WriteLine("------------------------------------------\n");


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


  public static DateTime GetDateTimeInput(TimeType TimeInput)
  {
    string timeType = TimeInput == TimeType.StartTime ? "start" : "end";
    Console.WriteLine($"\nPlease insert the {timeType} date (Format: dd-MM-yyyy).\nType 00 to set as today.\nType 0 to return to the main menu.");

    string? dateInput = Console.ReadLine();

    if (dateInput == "0")
    {
      GetUserInput();
    }

    DateTime date;

    if (dateInput == "00")
    {
      date = DateTime.Today;
    }
    else
    {
      while (!DateTime.TryParseExact(dateInput, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
      {
        Console.WriteLine("\nInvalid date. Format: dd-MM-yyyy. \n\n Type 0 to return to the main menu or try again:\n");
        dateInput = Console.ReadLine();
        if (dateInput == "0")
        {
          GetUserInput();
        }
      }
    }

    Console.WriteLine($"\nPlease insert the {timeType} time (Format: HH:mm).");
    string? timeInput = Console.ReadLine();
    TimeSpan time;

    while (!TimeSpan.TryParseExact(timeInput, "hh\\:mm", CultureInfo.InvariantCulture, out time))
    {
      Console.WriteLine("\nInvalid time. Format: HH:mm. Try again:\n");
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