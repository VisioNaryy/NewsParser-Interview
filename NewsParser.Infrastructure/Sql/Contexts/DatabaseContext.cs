namespace NewsParser.Infrastructure.Sql.Contexts;

public abstract class DatabaseContext
{
    public string ConnectionString { get; set; }
    public string ParameterPrefix { get; set; }
    public SqlCommand CommandObject { get; set; }
    public SqlDataReader DataReaderObject { get; set; }
    
    public abstract IDbConnection CreateConnection(string connectString);
    public abstract SqlCommand CreateCommand(IDbConnection cnn, string sql);
    
    public abstract SqlCommand CreateProcedureWithParameters(IDbConnection cnn, string sql, IDataParameter[] parameters);
    
    public abstract IDataParameter CreateParameter(string paramName, object value);

    public abstract IDataParameter CreateParameter();
    
    public abstract IDataParameter GetParameter(string paramName);
    
    public virtual async Task<SqlDataReader> CreateDataReaderAsync() 
    {
        return await CreateDataReaderAsync(CommandObject, CommandBehavior.CloseConnection);
    }
    
    public virtual async Task<object?> ExecuteScalarAsync() 
    {
        return await ExecuteScalarAsync(CommandObject, CommandBehavior.CloseConnection);
    }

    public virtual async Task<SqlDataReader> CreateDataReaderAsync(CommandBehavior cmdBehavior) 
    {
        return await CreateDataReaderAsync(CommandObject, cmdBehavior);
    }

    public virtual async Task<SqlDataReader> CreateDataReaderAsync(SqlCommand cmd, CommandBehavior cmdBehavior = CommandBehavior.CloseConnection)
    {
        await cmd.Connection.OpenAsync();
        
        DataReaderObject = await cmd.ExecuteReaderAsync(cmdBehavior);
        return DataReaderObject;
    }
    
    public async Task<object?> ExecuteScalarAsync(SqlCommand cmd,
        CommandBehavior cmdBehavior = CommandBehavior.CloseConnection)
    {
        await cmd.Connection.OpenAsync();

        return await cmd.ExecuteScalarAsync();
    }
    
    public DatabaseContext(string connectString) 
    {
        ConnectionString = connectString;
        Init();
    }
    
    protected virtual void Init() 
    {
        ParameterPrefix = string.Empty;
    }
    
    public virtual IDbConnection CreateConnection() 
    {
        return CreateConnection(ConnectionString);
    }
    
    public virtual SqlCommand CreateCommand(string sql) 
    {
        return CreateCommand(CreateConnection(), sql);
    }
    
    public virtual SqlCommand CreateProcedureWithParameters(string sql, IDataParameter[] parameters) 
    {
        return CreateProcedureWithParameters(CreateConnection(), sql, parameters);
    }
    
    public virtual void Dispose() 
    {
        if (DataReaderObject != null) 
        {
            DataReaderObject.Close();
            DataReaderObject.Dispose();
        }
        
        if (CommandObject != null) 
        {
            if (CommandObject.Connection != null) 
            {
                if (CommandObject.Transaction != null) 
                {
                    CommandObject.Transaction.Dispose();
                }
                CommandObject.Connection.Close();
                CommandObject.Connection.Dispose();
            }
            CommandObject.Dispose();
        }
    }
}