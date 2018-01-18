using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PusherServer;

namespace Domotica_API.Controllers.Pusher
{
    public class Pusher
    {
        public static PusherServer.Pusher Create()
        {
            //Creates a new instance of the Pusher Object.
            return new PusherServer.Pusher(
                env.PUSHER_APPID,
                env.PUSHER_KEY,
                env.PUSHER_SECRET,
                new PusherOptions
                {
                    Cluster = env.PUSHER_CLUSTER
                });
        }
    }
}
