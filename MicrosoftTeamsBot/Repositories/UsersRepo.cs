using MicrosoftTeamsBot.Data;
using MicrosoftTeamsBot.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace MicrosoftTeamsBot.Repositories
{
    public class UsersRepo : IUsersRepo
    {
        HttpClient client = new HttpClient();

        private MicrosoftBotDbContext _applicationDbContext;
        public UsersRepo(MicrosoftBotDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IEnumerable<Users> GetAllUsers()
        {
            try
            {
                return _applicationDbContext.users.AsNoTracking().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Users GetUserById(string UserId)
        {
            try
            {
                var bird = _applicationDbContext.users.Where(u => u.Id == UserId).AsNoTracking();
                return bird.SingleOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}