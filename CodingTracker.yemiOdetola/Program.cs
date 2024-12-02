using CodingTracker.yemiOdetola;

string connectionString = DbConnectionHelper.GetConnectionString();
var dbQuery = new DbQuery(connectionString);
dbQuery.CreateTable();
UserInput.GetUserInput();