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

        
        [HttpGet(Name ="MandelBrot?from={param1}&to={param2}&step={param3}&iter={param4}")]
        public MandelbrotSet Get(string from = "-3:-2", string to = "2:2", double step = 500,int iter = 30) {


            string[] from_arg = from.Split(':');
            string[] to_arg= to.Split(':');

            Complex v2_from = new Complex(Convert.ToDouble(from_arg[0]), Convert.ToDouble(from_arg[1]));
            Complex v2_to   = new Complex(Convert.ToDouble(  to_arg[0]), Convert.ToDouble(  to_arg[1]));

            return new MandelbrotSet(v2_from, v2_to, step,iter);
        }
        
        


    }
}
