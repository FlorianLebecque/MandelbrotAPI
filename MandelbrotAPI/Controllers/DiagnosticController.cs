using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MandelbrotAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosticController : ControllerBase {



        [HttpGet(Name = "Diagnosis")]
        public float Get() {

            PerformanceCounter a = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            a.NextValue();

            Thread.Sleep(1000);

            return a.NextValue();
        }
    }
}
