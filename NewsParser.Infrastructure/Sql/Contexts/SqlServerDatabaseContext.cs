namespace NewsParser.Infrastructure.Sql.Contexts;

public class SqlServerDatabaseContext : DatabaseContext, IDisposable
{
    public SqlServerDatabaseContext(string connectString) : base(connectString)
    {
    }

    protected override void Init()
    {
        base.Init();

        ParameterPrefix = "@";
    }

    public override IDbConnection CreateConnection(string connectString)
    {
        return new SqlConnection(connectString);
    }

    public override SqlCommand CreateCommand(IDbConnection cnn, string sql)
    {
        CommandObject = new SqlCommand(sql, (SqlConnection) cnn);
        CommandObject.CommandType = CommandType.Text;
        return CommandObject;
    }

    public override SqlCommand CreateProcedureWithParameters(IDbConnection cnn, string sql, IDataParameter[] parameters)
    {
        CommandObject = new SqlCommand(sql, (SqlConnection) cnn);
        CommandObject.CommandType = CommandType.StoredProcedure;
        foreach (var parameter in parameters)
        {
            CommandObject.Parameters.Add(parameter);
        }

        return CommandObject;
    }

    public override IDataParameter CreateParameter(string paramName, object value)
    {
        if (!paramName.StartsWith(ParameterPrefix))
        {
            paramName = ParameterPrefix + paramName;
        }

        return new SqlParameter(paramName, value);
    }

    public override IDataParameter CreateParameter()
    {
        return new SqlParameter();
    }

    public override IDataParameter GetParameter(string paramName)
    {
        if (!paramName.StartsWith(ParameterPrefix))
        {
            paramName = ParameterPrefix + paramName;
        }

        return CommandObject.Parameters[paramName];
    }

    public override async Task<SqlDataReader> CreateDataReaderAsync(SqlCommand cmd,
        CommandBehavior cmdBehavior = CommandBehavior.CloseConnection)
    {
        await cmd.Connection.OpenAsync();
        
        DataReaderObject = await cmd.ExecuteReaderAsync(cmdBehavior);
        
        return DataReaderObject;
    }
}