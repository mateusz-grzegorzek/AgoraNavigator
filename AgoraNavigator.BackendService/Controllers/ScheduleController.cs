using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AgoraNavigator.Domain.Schedule;

namespace AgoraNavigator.BackendService.Controllers
{
    [Route("api/[controller]")]
    public class ScheduleController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<ScheduleItem> Get()
        {
            return new ScheduleItem[] 
            {
                new ScheduleItem()
                {
                    Title = "Opening Ceremony",
                    Presenter = "Chuck Norris",
                    StartTime = new DateTime(2017, 4, 23, 12, 00, 00)
                },
                new ScheduleItem()
                {
                    Title = "The Pierogi Workshop",
                    Presenter = "Andrzej Duda",
                    StartTime = new DateTime(2017, 4, 24, 13, 00, 00)
                },
                new ScheduleItem()
                {
                    Title = "Melanż & Drinking Presentation",
                    Presenter = "Owca",
                    StartTime = new DateTime(2017, 4, 24, 14, 15, 00)
                }
            };
        }
    }
}
