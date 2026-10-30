using DE.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace DE
{
    public static class AppState
    {
        public static User? CurrentUser { get; set; }
    }
}
