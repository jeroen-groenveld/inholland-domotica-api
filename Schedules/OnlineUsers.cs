using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domotica_API.Models;
using Domotica_API.Models.TokenAuth;
using Microsoft.EntityFrameworkCore;
using PusherServer;
using Newtonsoft.Json;

namespace Domotica_API.Schedules
{
    public class OnlineUsers
    {
        public async Task Run()
        {
            var pusher = Controllers.Pusher.Pusher.Create();
            IGetResult<object> result = await pusher.GetAsync<object>("/channels/global-channel", new {info = "subscription_count"});

            SubCount sub = Newtonsoft.Json.JsonConvert.DeserializeObject<SubCount>(result.Body);
            if (sub.subscription_count == 0)
            {
                return;
            }

            await pusher.TriggerAsync(channelName: "global-channel", eventName: "user_count", data: sub.subscription_count);
        }

        public class SubCount
        {
            public bool occupied { get; set; }
            public int subscription_count { get; set; }
        }
    }
}
