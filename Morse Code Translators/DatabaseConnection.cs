using JollyWrapper;

public static class DatabaseConnection
{
    // Initialising the database
    public static void InitialiseDatabase()
    {
        Database.Init("plesk.remote.ac",
                       "ws374429_OOP",
                       "y6f3U^g17",
                       "ws374429_OOP",
                       "SSLMode=None");
        if (Database.CheckConnection().Result)
        {
            Console.WriteLine("Database connected successfully!");
        }

    }

    // Inserting into database
    public static void InsertDataIntoDb(int userID, string content)
    {
        InitialiseDatabase();

        QueryParms parms = new QueryParms()
        {
            { "content", content},
            { "@userID", userID.ToString()}
        };

        Database.ExecuteNonQuery("INSERT INTO `Data` (`userID`, `content`) VALUES (@userID, @content)", parms);
    }

} 