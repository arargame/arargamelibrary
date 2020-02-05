using ArarGameLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.UserManagement
{
    public class User : BaseObject
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsAuthorized { get; set; }

        public List<LogManagement.Log> Logs
        {
            get
            {
                return LogManagement.LogManager.GetLogsByEntity(Id.ToString());
            }
        }
    }
}
