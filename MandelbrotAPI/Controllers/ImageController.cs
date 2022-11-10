using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace MandelbrotAPI.Controllers {
    [Route("[controller]")]
    [ApiController]
    public class ImageController : ControllerBase {


        [HttpGet(Name = "MBImg?from={param1}&to={param2}&step={param3}&iter={param4}")]
        public IActionResult GetImg(string from = "-3:-2", string to = "2:2", double step = 1000, int iter = 30) {

            string[] from_arg = from.Split(':');
            string[] to_arg = to.Split(':');

            Complex v2_from = new Complex(Convert.ToDouble(from_arg[0]), Convert.ToDouble(from_arg[1]));
            Complex v2_to = new Complex(Convert.ToDouble(to_arg[0]), Convert.ToDouble(to_arg[1]));

            MandelbrotSet mbs = new MandelbrotSet(v2_from, v2_to, step, iter);

            var img = mbs.SaveImg();

            return File(img, "image/jpeg");
        }
    }
}
