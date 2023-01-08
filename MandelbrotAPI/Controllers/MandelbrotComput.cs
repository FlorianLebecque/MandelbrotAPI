using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace MandelbrotAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]


    public class MandelbrotComput : ControllerBase {

        [HttpGet(Name = "MandelBrot?off_i={param1}&off_j={param2}&from={param3}&to={param4}&step={param5}&iter={param6}")]
        public MandelBrotPart Get(int off_i, int off_j, string from = "-3:-2", string to = "2:2", double step = 500, int iter = 30) {

            string[] from_arg = from.Split(':');
            string[] to_arg = to.Split(':');

            Complex v2_from = new Complex(Convert.ToDouble(from_arg[0]), Convert.ToDouble(from_arg[1]));
            Complex v2_to = new Complex(Convert.ToDouble(to_arg[0]), Convert.ToDouble(to_arg[1]));

            return new(off_i,off_j, v2_from, v2_to, step,iter);
        }

    }
}
