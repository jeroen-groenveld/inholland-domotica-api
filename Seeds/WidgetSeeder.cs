using Domotica_API.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Domotica_API.Seeds
{
    public class WidgetSeeder : Seeder
    {
        protected override async Task Seed(DatabaseContext db)
        {

            if (db.Widgets.SingleOrDefault(x => x.id == 1) == null)
            {
                db.Add(new Widget
                {
                    name = "Window",
                    component_name = "windows",
                    description = "Open/Close the windows."
                });
            }

            if (db.Widgets.SingleOrDefault(x => x.id == 2) == null)
            {
                db.Add(new Widget
                {
                    name = "Lamp",
                    component_name = "lamps",
                    description = "Set the light On/Off."
                });
            }

            if (db.Widgets.SingleOrDefault(x => x.id == 3) == null)
            {
                db.Add(new Widget
                {
                    name = "Time & Date",
                    component_name = "time-date",
                    description = "Show the Time and Date."
                });
            }

            if (db.Widgets.SingleOrDefault(x => x.id == 4) == null)
            {
                db.Add(new Widget
                {
                    name = "Weather",
                    component_name = "weather",
                    description = "Shows the weather forecast for the upcomming days."
                });
            }

            if (db.Widgets.SingleOrDefault(x => x.id == 5) == null)
            {
                db.Add(new Widget
                {
                    name = "Heater",
                    component_name = "heater",
                    description = "Change the temperature of the heater."
                });
            }

	        if (db.Widgets.SingleOrDefault(x => x.id == 6) == null)
	        {
		        db.Add(new Widget
		        {
			        name = "Scoreboard",
			        component_name = "scoreboard",
			        description = "Scoreboard for Tic Tac Toe."
		        });
	        }

	        if (db.Widgets.SingleOrDefault(x => x.id == 7) == null)
	        {
		        db.Add(new Widget
		        {
			        name = "Tic Tac Toe",
			        component_name = "tic-tac-toe",
			        description = "Tic Tac Toe game."
		        });
			}

			if (db.Widgets.SingleOrDefault(x => x.id == 8) == null)
			{
				db.Add(new Widget
				{
					name = "Bookmarks",
					component_name = "bookmarks",
					description = "Bookmarks that point to an url."
				});
			}

			await db.SaveChangesAsync();
        }
    }
}
