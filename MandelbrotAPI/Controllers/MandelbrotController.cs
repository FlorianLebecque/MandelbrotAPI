using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace MandelbrotAPI.Controllers {

    [ApiController]
    [Route("[controller]")]
    public class MandelbrotController : ControllerBase {

        LoadManager lm;

        public MandelbrotController() {
            lm = new();
        }

        
        [HttpGet(Name ="MandelBrot?from={param1}&to={param2}&step={param3}&iter={param4}&split={param5}")]
        public MandelbrotSet Get(string from = "-3:-2", string to = "2:2", double step = 500,int iter = 30,int split = 16) {

            string[] from_arg = from.Split(':');
            string[] to_arg= to.Split(':');

            Complex v2_from = new Complex(Convert.ToDouble(from_arg[0]), Convert.ToDouble(from_arg[1]));
            Complex v2_to   = new Complex(Convert.ToDouble(  to_arg[0]), Convert.ToDouble(  to_arg[1]));

            lm.CreateJobs(split,v2_from, v2_to, step, iter);
            lm.Start();
            lm.Wait();

            return lm.GetResult(split,v2_from,v2_to,step);
        }


    }
}
