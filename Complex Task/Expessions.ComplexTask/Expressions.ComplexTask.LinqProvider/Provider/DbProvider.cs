using System.Data.SqlClient;
using Expressions.ComplexTask.LinqProvider.Entities;

namespace Expressions.ComplexTask.LinqProvider.Provider;

public class DbProvider
{
    private readonly string _connectionString;

    public DbProvider(string connectionString)
    {
        _connectionString = connectionString.Replace("%CONTENTROOTPATH%", AppDomain.CurrentDomain.BaseDirectory);
    }

    public IEnumerable<T> GetQueryResults<T>(string query)
        where T : BaseEntity
    {
        var result = new List<T>();
        var properties = typeof(T).GetProperties();

        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = query;
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var entity = Activator.CreateInstance(typeof(T)) as T;
            foreach (var property in properties)
            {
                property.SetValue(entity, reader[property.Name]);
            }
            result.Add(entity);
        }

        return result;
    }


}