using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webserver
{
    interface HTTPObject
    {
        Dictionary<string,string> Headers { get; set; }
        string Body { get; set; }
    }
}
