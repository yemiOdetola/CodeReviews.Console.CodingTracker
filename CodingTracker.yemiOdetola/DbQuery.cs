using Microsoft.Data.Sqlite;
using Dapper;

namespace CodingTracker.yemiOdetola;

public class DbQuery
{
  private readonly string ConnectionString;
  public DbQuery(string connectionString)
  {
    ConnectionString = connectionString;
  }

  public void CreateTable()
  {
    using (var connection = new SqliteConnection(ConnectionString))
    {
      connection.Open();
      connection.Execute(@"CREATE TABLE IF NOT EXISTS Records (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                StartTime TEXT,
                EndTime TEXT,
                Duration INTEGER
            )");
    }
  }

  public void CreateRecord(DateTime StartTime, DateTime EndTime, int DurationMinutes)
  {
    using (var connection = new SqliteConnection(ConnectionString))
    {
      connection.Open();
      connection.Execute(
          "INSERT INTO Records (StartTime, EndTime, Duration) VALUES (@StartTime, @EndTime, @Duration)",
          new
          {
            StartTime = StartTime.ToString("yyyy-MM-dd HH:mm:ss"),
            EndTime = EndTime.ToString("yyyy-MM-dd HH:mm:ss"),
            Duration = DurationMinutes
          }
      );
    }
  }
  public void UpdateRecord(int recordId, DateTime? StartTime = null, DateTime? EndTime = null, int? DurationMinutes = null)
  {
    using var connection = new SqliteConnection(ConnectionString);
    connection.Open();

    var updates = new List<string>();
    var parameters = new DynamicParameters();
    parameters.Add("@Id", recordId);

    if (StartTime.HasValue)
    {
      updates.Add("StartTime = @StartTime");
      parameters.Add("@StartTime", StartTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
    }
    if (EndTime.HasValue)
    {
      updates.Add("EndTime = @EndTime");
      parameters.Add("@EndTime", EndTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
    }
    if (DurationMinutes.HasValue)
    {
      updates.Add("Duration = @Duration");
      parameters.Add("@Duration", DurationMinutes.Value);
    }

    if (!updates.Any())
    {
      Console.WriteLine("No fields to update.");
      return;
    }

    var query = $"UPDATE Records SET {string.Join(", ", updates)} WHERE Id = @Id";
    connection.Execute(query, parameters);
  }


  public void DeleteRecord(int recordId)
  {
    using (var connection = new SqliteConnection(ConnectionString))
    {
      connection.Open();
      int rowCount = connection.Execute("DELETE FROM Records WHERE Id = @Id", new { Id = recordId });

      if (rowCount == 0)
      {
        Console.WriteLine($"\nRecord with Id {recordId} doesn't exist.\n");
      }
      else
      {
        Console.WriteLine($"\nRecord with Id {recordId} was deleted.\n");
      }
    }
  }

  public CodingSession? FetchSingleRecord(int recordId)
  {
    using (var connection = new SqliteConnection(ConnectionString))
    {
      connection.Open();
      var record = connection.QuerySingleOrDefault<CodingSession>(
          "SELECT * FROM Records WHERE Id = @Id",
          new { Id = recordId }
      );
      return record;
    }

  }

  public List<CodingSession> FetchAllRecords()
  {

    using (var connection = new SqliteConnection(ConnectionString))
    {
      connection.Open();
      return connection.Query<CodingSession>("SELECT * FROM Records").ToList();
    }

  }


}
