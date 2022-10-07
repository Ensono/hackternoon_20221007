using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsonoDigital.Slack.Hackternoon.Application
{
    public class BobClientMock : IBobClient
    {
        public IList<BobEmployee> GetOutOfOfficeEmployees()
        {
            return new List<BobEmployee>
            {
                new BobEmployee
                {
                    Email = "alasdair.stark@ensono.com"
                },
                new BobEmployee()
                {
                    Email = "vishal.sharma@ensono.com"
                },
                new BobEmployee()
                {
                    Email = "alan@ensono.com"
                }
            };
        }
    }
}
