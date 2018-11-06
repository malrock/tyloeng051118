using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace windowsservice
{
    interface IConfiguration
    {
        string SubscriptionKey { get; }
        string UriBase { get; }
        string ImagesPath { get; }
    }
}
