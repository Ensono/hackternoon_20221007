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
                new ()
                {
                    Email = "alasdair.stark@ensono.com",
                    Competency = "Connected Services"
                },
                new ()
                {
                    Email = "andy.durkan@ensono.com",
                    Competency = "Data Engineering"
                },
                new ()
                {
                    Email = "alan.walsh@ensono.com",
                    Competency = "Snowsports Division"
                }
            };
        }
    }
}
