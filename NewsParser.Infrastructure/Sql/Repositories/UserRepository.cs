

using NewsParser.Domain.Authentication.Models;
using NewsParser.Infrastructure.Extensions;

namespace NewsParser.Infrastructure.Sql.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SqlSettings _sqlSettings;
    private readonly SqlServerDatabaseContext _context;

    public UserRepository(IOptions<SqlSettings> sqlSettings, SqlServerDatabaseContext context)
    {
        _sqlSettings = sqlSettings.Value;
        _context = context;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var emailParameter = new SqlParameter("@email", email);

        var procedureName = _sqlSettings.Procedures.GetUserByEmail;

        _context.CreateProcedureWithParameters(procedureName, new[] {emailParameter});

        var rdr = await _context.CreateDataReaderAsync();

        if (!rdr.HasRows)
            return null;

        User? user = null;
        while (await rdr.ReadAsync())
        {
            user = new User
            {
                Id = rdr.GetData<int>("Id"),
                FirstName = rdr.GetData<string>("FirstName"),
                LastName = rdr.GetData<string>("LastName"),
                Email = rdr.GetData<string>("Email"),
                Password = rdr.GetData<string>("Password")
            };
        }

        return user;
    }

    public async Task<int> AddAsync(User user)
    {
        var (firstName, lastName, email, password) = user;

        var firstNameParameter = new SqlParameter("@firstname", firstName);
        var lastNameParameter = new SqlParameter("@lastname", lastName);
        var emailParameter = new SqlParameter("@email", email);
        var passwordParameter = new SqlParameter("@password", password);

        var procedureName = _sqlSettings.Procedures.AddUser;

        _context.CreateProcedureWithParameters(procedureName, new[]
        {
            firstNameParameter, lastNameParameter,
            emailParameter, passwordParameter
        });

        var id = await _context.ExecuteScalarAsync();

        return Convert.ToInt32(id);
    }
}