using System.Configuration;
using Microsoft.Data.Sqlite;

namespace CodingTracker.yemiOdetola;

public class DbQuery
{
  public static string? ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
  public static void CreateTable()
  {
    using (var connection = new SqliteConnection(ConnectionString))
    {
      connection.Open();

      var tableCmd = connection.CreateCommand();

      tableCmd.CommandText =
          @"CREATE TABLE IF NOT EXISTS Records (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            StartTime TEXT,
            EndTime TEXT,
            Duration INTEGER
        )";
      tableCmd.ExecuteNonQuery();

      connection.Close();
    }
  }

  public static void CreateRecord(DateTime StartTime, DateTime EndTime, int DurationMinutes)
  {
    using (var connection = new SqliteConnection(ConnectionString))
    {
      connection.Open();
      var command = connection.CreateCommand();
      command.CommandText = $"INSERT INTO Records (StartTime, EndTime, Duration) VALUES (@startTime, @endTime, @duration)";

      command.Parameters.AddWithValue("@startTime", StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
      command.Parameters.AddWithValue("@endTime", EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
      command.Parameters.AddWithValue("@duration", DurationMinutes);

      command.ExecuteNonQuery();
      connection.Close();
    }


  }
  public static void UpdateRecord(int recordId, DateTime StartTime, DateTime EndTime, int DurationMinutes)
  {

    using (var connection = new SqliteConnection(ConnectionString))
    {
      connection.Open();

      var EditCommand = connection.CreateCommand();
      EditCommand.CommandText = $"UPDATE Records SET StartTime = @startTime, EndTime = @endTime, Duration = @duration WHERE Id = {recordId}";

      EditCommand.Parameters.AddWithValue("@startTime", StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
      EditCommand.Parameters.AddWithValue("@endTime", EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
      EditCommand.Parameters.AddWithValue("@duration", DurationMinutes);

      EditCommand.ExecuteNonQuery();
      connection.Close();
    }
  }
  public static void DeleteRecord(int recordId)
  {

    using (var connection = new SqliteConnection(ConnectionString))
    {
      connection.Open();
      var tableCmd = connection.CreateCommand();
      tableCmd.CommandText = $"DELETE FROM Records WHERE Id = {recordId}";

      int rowCount = tableCmd.ExecuteNonQuery();
      if (rowCount == 0)
      {
        Console.WriteLine($"\nRecord with Id {recordId} doesn't exist.\n");
      }
      else
      {
        Console.WriteLine($"\nRecord with Id {recordId} was deleted.\n");
      }
      connection.Close();
    }


  }

  public static int FetchSingleRecord(int recordId)
  {
    using (var connection = new SqliteConnection(ConnectionString))
    {
      connection.Open();
      var checkCmd = connection.CreateCommand();
      checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM Records WHERE Id = {recordId})";
      var output = checkCmd.ExecuteScalar();
      connection.Close();
      return Convert.ToInt32(output);
    }

  }

  public static List<CodingSession> FetchAllRecords()
  {

    using (var connection = new SqliteConnection(ConnectionString))
    {
      connection.Open();
      var tableCmd = connection.CreateCommand();
      tableCmd.CommandText = $"SELECT * FROM Records";

      List<CodingSession> tableData = new List<CodingSession>();
      SqliteDataReader reader = tableCmd.ExecuteReader();
      if (reader.HasRows)
      {
        while (reader.Read())
        {
          tableData.Add(new CodingSession
          {
            Id = reader.GetInt32(0),
            StartTime = reader.GetString(1),
            EndTime = reader.GetString(2),
            Duration = reader.GetInt32(3)
          });
        }
      }
      connection.Close();
      return tableData;
    }

  }


}
