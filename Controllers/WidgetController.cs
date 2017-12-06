using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domotica_API.Controllers;
using Domotica_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Domotica_API.Controllers
{
    [Route(Config.App.API_ROOT_PATH + "/background")]
    public class WidgetController : ApiController
    {
        //Constructor
        public WidgetController(DatabaseContext db) : base(db) { }


        //public static bool UserWidgetValidPosition(ActiveWidget widget, List<ActiveWidget> widgets, int[,] position)
        //{
        //    Dictionary<int, bool> grid = new Dictionary<int, bool>();



        //}

        //public static int[,] grid()
        //{
        //   int[,] grid = new int[Config.App.GRID_SIZE_WIDTH * Config.App.GRID_SIZE_HEIGHT, 2];

        //}

        //public static List<ActiveWidget> GetUserWidgets(User user, DatabaseContext db)
        //{
        //    List <ActiveWidget> widgets = db.ActiveWidgets.Where(x => x.user_id == user.id).ToList();
        //    return widgets;
        //}

        //public static List<Widget> GetWidgetsForUserWidgets(List<ActiveWidget> userWidgets, DatabaseContext db)
        //{
        //    List<Widget> widgets = new List<Widget>();
        //    foreach (ActiveWidget userWidget in userWidgets)
        //    {
        //        Widget widget = db.Widgets.SingleOrDefault(x => x.id == userWidget.widget_id);
        //        if (widget != null)
        //        {
        //            widgets.Add(widget);
        //        }
        //    }
        //    return widgets;
        //}
    }
}