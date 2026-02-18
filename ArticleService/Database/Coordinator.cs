using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace ArticleService.Database;


public class Coordinator
{
    private IDictionary<string, DbConnection> ConnectionCache = new Dictionary<string, DbConnection>();
    //changed these to be port numbers instead
    private const string GLOBAL_DB = "1433";
    private const string GLOBAL_CONTINENT_1_DB = "1434";
    private const string GLOBAL_CONTINENT_2_DB = "1435";
    private const string GLOBAL_CONTINENT_3_DB = "1436";
    private const string GLOBAL_CONTINENT_4_DB = "1437";
    private const string GLOBAL_CONTINENT_5_DB = "1438";
    private const string GLOBAL_CONTINENT_6_DB = "1439";
    private const string GLOBAL_CONTINENT_7_DB = "1440";

    private List<string> GlobalContinentArray =
    [
        GLOBAL_DB, GLOBAL_CONTINENT_1_DB, GLOBAL_CONTINENT_2_DB, GLOBAL_CONTINENT_3_DB, GLOBAL_CONTINENT_4_DB,
        GLOBAL_CONTINENT_5_DB, GLOBAL_CONTINENT_6_DB, GLOBAL_CONTINENT_7_DB
    ];


    public DbConnection GetContinentGlobal()
    {
        return GetConnectionByPort(GLOBAL_DB);
    }
    public DbConnection GetContinent1()
    {
        return GetConnectionByPort(GLOBAL_CONTINENT_1_DB);
    }
    public DbConnection GetContinent2()
    {
        return GetConnectionByPort(GLOBAL_CONTINENT_2_DB);
    }
    public DbConnection GetContinent3()
    {
        return GetConnectionByPort(GLOBAL_CONTINENT_3_DB);
    }
    public DbConnection GetContinent4()
    {
        return GetConnectionByPort(GLOBAL_CONTINENT_4_DB);
    }
    public DbConnection GetContinent5()
    {
        return GetConnectionByPort(GLOBAL_CONTINENT_5_DB);
    }
    public DbConnection GetContinent6()
    {
        return GetConnectionByPort(GLOBAL_CONTINENT_6_DB);
    }
    public DbConnection GetContinent7()
    {
        return GetConnectionByPort(GLOBAL_CONTINENT_7_DB);
    }
    
    /*public DbConnection GetGlobalConnection(string word)
    {
        switch (word.Length)
        {
            case var l when (l <= 10):
                return GetConnectionByServerName(SHORT_WORD_DB);
            case var l when (l > 10 && l <= 20):
                return GetConnectionByServerName(MEDIUM_WORD_DB);
            case var l when (l >= 21):
                return GetConnectionByServerName(LONG_WORD_DB);
            default:
                throw new InvalidDataException();
        }
    }*/

    public IEnumerable<DbConnection> GetAllConnections()
    {
        yield return GetContinentGlobal();
        yield return GetContinent1();
        yield return GetContinent2();
        yield return GetContinent3();
        yield return GetContinent4();
        yield return GetContinent5();
        yield return GetContinent6();
        yield return GetContinent7();

    }

    private DbConnection GetConnectionByPort(string port)
    {
        if (ConnectionCache.TryGetValue(port, out var connection))
        {
            return connection;
        }
        
        connection = new SqlConnection($"server=tcp:(local), {port};User Id=sa;Password=SuperSecret7!;Encrypt=false;");
        //connection = new SqlConnection(connectionString);
        connection.Open();
        ConnectionCache.Add(port, connection);
        return connection;
    }
}