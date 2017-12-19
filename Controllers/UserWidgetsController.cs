using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domotica_API.Middleware;
using Domotica_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Domotica_API.Controllers
{
    [Route(Config.App.API_ROOT_PATH + "/user/widgets")]
    [MiddlewareFilter(typeof(TokenAuthorize))]
    public class UserWidgetsController : ApiController
    {
        //Constructor
        public UserWidgetsController(DatabaseContext db) : base(db) { }

        [HttpGet]
        public IActionResult GetUserWidgets()
        {
            User user = (User)HttpContext.Items["user"];

            return Ok(new {widgets = this.WidgetList(), user_widgets = this.UserWidgetList(user)});
        }

        [HttpPut]
        public IActionResult PutUserWidgets([FromBody] List<Validators.UserWidget> user_widgets)
        {
            User user = (User)HttpContext.Items["user"];

            if (ModelState.IsValid == false)
            {
                return BadRequest("Incorrect post data");
            }

            List<UserWidget> userWidgets = new List<UserWidget>();
            Dictionary<char, List<int>> widgetColumnIndex = new Dictionary<char, List<int>>();
            foreach (Validators.UserWidget userWidget in user_widgets)
            {
                //Check if the widget exists.
                Widget widget = this.db.Widgets.SingleOrDefault(x => x.id == userWidget.widget_id);
                if (widget == null)
                {
                    return BadRequest("Incorrect post data, can't widget with id('" + userWidget.widget_id + "')");
                }

                //Check if the index in the column is already occupied.
                char column = userWidget.column[0];
                int column_index = userWidget.column_index;
                if (widgetColumnIndex.ContainsKey(column))
                {
                    List<int> indexes = widgetColumnIndex[column];
                    if (indexes.Contains(column_index))
                    {
                        return BadRequest("Incorrect post data, there is already a widget at this index(Column: '" +
                                          column + "', Index: '" + column_index + "').");
                    }
                }
                else
                {
                    widgetColumnIndex[column] = new List<int>();
                }
                widgetColumnIndex[column].Add(column_index);


                userWidgets.Add(new UserWidget
                {
                    column = userWidget.column,
                    column_index = userWidget.column_index,
                    user_id = user.id,
                    widget_id = widget.id
                });
            }

            //Remove old widgets.
            this.db.RemoveRange(this.UserWidgetList(user));

            //Add new widgets.
            user.Widgets = userWidgets;
            this.db.SaveChanges();

            return Ok(new { user.Widgets });
        }

        public List<Widget> WidgetList()
        {
            return this.db.Widgets.OrderBy(x => x.id).ToList();
        }

        public List<UserWidget> UserWidgetList(User user)
        {
            return this.db.UserWidgets.Where(x => x.user_id == user.id).ToList();
        }
    }
}