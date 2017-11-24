using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API.Models;
using Web_API.Models.TokenAuth;

namespace Web_API.Scheduler
{
    public class TokenCleaner
    {
        public async Task Run()
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                Console.WriteLine("Removing expired tokens...");
                List<AccessToken> accessTokens = db.AccessTokens.Where(x => x.expires_at < DateTime.Now).ToList();
                List<RefreshToken> refreshTokens = db.RefreshTokens.Where(x => x.access_token.expires_at < DateTime.Now).Include(x => x.access_token).ToList();
                Console.WriteLine("Removed {0} expired access tokens.", accessTokens.Count());
                Console.WriteLine("Removed {0} expired refresh tokens.", refreshTokens.Count());
                db.RemoveRange(refreshTokens);
                db.RemoveRange(accessTokens);
                await db.SaveChangesAsync();
            }
        }
    }
}
