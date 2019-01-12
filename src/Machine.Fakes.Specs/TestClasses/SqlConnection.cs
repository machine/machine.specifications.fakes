namespace Machine.Fakes.Specs.TestClasses
{
    public class SqlConnection : IDbConnection
    {
        public SqlConnection()
        {
        }

        public SqlConnection(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; }
    }
}