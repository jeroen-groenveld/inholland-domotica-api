using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API.Models;
using Web_API.Models.TokenAuth;
using System.Threading;

namespace Web_API.Scheduler
{
    public class TokenCleaner
    {
        protected DatabaseContext _db;

        public TokenCleaner(DatabaseContext db)
        {
            _db = db;
        }

        public async Task Run()
        {
            Console.WriteLine("Removing expired tokens...");
            List<AccessToken> accessTokens = this._db.AccessTokens.Where(x => x.expires_at < DateTime.Now).ToList();
            List<RefreshToken> refreshTokens = this._db.RefreshTokens.Where(x => x.expires_at < DateTime.Now).ToList();
            Console.WriteLine("Removed {0} expired access tokens.", accessTokens.Count());
            Console.WriteLine("Removed {0} expired refresh tokens.", refreshTokens.Count());
            this._db.RemoveRange(refreshTokens);
            this._db.RemoveRange(accessTokens);
            await this._db.SaveChangesAsync();
        }
    }
}
