using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminLteMvc.Controllers
{
    public class BaseController : Controller
    {
        // GET: Sweet
           public string Alert(string message, Utilities.NotificationType notificationType)
            {
                string msg = "<script language='javascript'>swal('" + notificationType.ToString().ToUpper() + "', '" + message.Replace("'", " ").Replace("`"," ") + "','" + notificationType + "')" + "</script>";
                ViewBag.notification = msg;
                return msg;
            }
    }
}