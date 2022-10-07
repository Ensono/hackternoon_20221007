using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsonoDigital.Slack.Hackternoon.Application
{
    public interface IBobClient
    {
        IList<BobEmployee> GetOutOfOfficeEmployees();
    }
}
