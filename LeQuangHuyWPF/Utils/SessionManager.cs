using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeQuangHuyWPF.Utils
{
    public static class SessionManager
    {
        public static Customer CurrentUser { get; set; }
        public static bool IsAdmin => CurrentUser?.EmailAddress == "admin@FUMiniHotelSystem.com";
    }
}
