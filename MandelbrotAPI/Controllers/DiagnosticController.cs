using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MandelbrotAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosticController : ControllerBase {

        private static PerformanceThread perfThread;

        public DiagnosticController() {
            if(perfThread == null) {
                perfThread = new PerformanceThread();
            }
        }

        [HttpGet(Name = "Diagnosis")]
        public float Get() {
            return perfThread.GetPerformance();
        }
    }
}
