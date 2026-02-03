using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace ArticleService.Database;


public class Coordinator
{
    private IDictionary<string, DbConnection> ConnectionCache = new Dictionary<string, DbConnection>();
    private const string GLOBAL_DB = "mssql-global";
    private const string GLOBAL_CONTINENT_1_DB = "mssql-continent-1";
    private const string GLOBAL_CONTINENT_2_DB = "mssql-continent-2";
    private const string GLOBAL_CONTINENT_3_DB = "mssql-continent-3";
    private const string GLOBAL_CONTINENT_4_DB = "mssql-continent-4";
    private const string GLOBAL_CONTINENT_5_DB = "mssql-continent-5";
    private const string GLOBAL_CONTINENT_6_DB = "mssql-continent-6";
    private const string GLOBAL_CONTINENT_7_DB = "mssql-continent-7";


    public DbConnection GetContinentGlobal()
    {
        return GetConnectionByServerName(GLOBAL_DB);
    }
    public DbConnection GetContinent1()
    {
        return GetConnectionByServerName(GLOBAL_CONTINENT_1_DB);
    }
    public DbConnection GetContinent2()
    {
        return GetConnectionByServerName(GLOBAL_CONTINENT_2_DB);
    }
    public DbConnection GetContinent3()
    {
        return GetConnectionByServerName(GLOBAL_CONTINENT_3_DB);
    }
    public DbConnection GetContinent4()
    {
        return GetConnectionByServerName(GLOBAL_CONTINENT_4_DB);
    }
    public DbConnection GetContinent5()
    {
        return GetConnectionByServerName(GLOBAL_CONTINENT_5_DB);
    }
    public DbConnection GetContinent6()
    {
        return GetConnectionByServerName(GLOBAL_CONTINENT_6_DB);
    }
    public DbConnection GetContinent7()
    {
        return GetConnectionByServerName(GLOBAL_CONTINENT_7_DB);
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

    private DbConnection GetConnectionByServerName(string serverName)
    {
        if (ConnectionCache.TryGetValue(serverName, out var connection))
        {
            return connection;
        }
        
        connection = new SqlConnection($"Server={serverName};User Id=sa;Password=SuperSecret7!;Encrypt=false;");
        connection.Open();
        ConnectionCache.Add(serverName, connection);
        return connection;
    }
}