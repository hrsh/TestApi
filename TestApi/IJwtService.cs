using Microsoft.AspNetCore.Identity;

namespace TestApi
{
    public interface IJwtService
    {
        string Generate(IdentityUser user);
    }
}