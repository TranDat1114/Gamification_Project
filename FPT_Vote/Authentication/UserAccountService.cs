namespace FPT_Vote.Authentication;

public class UserAccountService
{
    private readonly List<UserAccount> _users;
    public UserAccountService()
    {
        _users = [
            new UserAccount { Email = "fptpoly_admin_17@demo.vn", Password = "admin", Role = "Admin" },
        ];
    }
    public async Task<UserAccount?> GetByUserName(string userName)
    {
        return _users.SingleOrDefault(x => x.Email == userName);

    }
}
