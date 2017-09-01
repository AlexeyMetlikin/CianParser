using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteParser.Infrastructure.Abstract
{
    public interface IRequestToServer
    {
        bool IsRequestByProxy { get; set; }

        void SetWebProxy(string address, int port);

        string GetPageContent(string url);
    }
}
