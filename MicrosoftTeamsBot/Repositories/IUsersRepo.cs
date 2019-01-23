using System.Collections.Generic;
using MicrosoftTeamsBot.Models;

namespace MicrosoftTeamsBot.Repositories
{
    public interface IUsersRepo
    {
        IEnumerable<Users> GetAllUsers();
        Users GetUserById(string UserId);
    }
}