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


        [HttpGet(Name ="MandelBrot?from={param1}&to={param2}&res={param3}&iter={param4}")]
        public MandelbrotSet Get(string from = "-10:-10", string to = "10:10", double res = 1,int iter = 30) {


            string[] from_arg = from.Split(':');
            string[] to_arg= to.Split(':');

            Complex v2_from = new Complex(Convert.ToDouble(from_arg[0]), Convert.ToDouble(from_arg[1]));
            Complex v2_to   = new Complex(Convert.ToDouble(  to_arg[0]), Convert.ToDouble(  to_arg[1]));

            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            return new MandelbrotSet(v2_from, v2_to, res,iter);
        }


    }
}
