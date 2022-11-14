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


            const int SPLIT = 2;
            double real_spacing = (v2_to.Real - v2_from.Real) / SPLIT;
            double imag_spacing = (v2_to.Imaginary - v2_from.Imaginary) / SPLIT;

            List<Task<MandelbrotSet>> taskList = new List<Task<MandelbrotSet>>();
            for (int i = 0; i < SPLIT; i++) {
                for (int j = 0; j < SPLIT; j++) {

                    Complex new_from = new Complex(v2_from.Real + (i * real_spacing), v2_from.Imaginary + (j * imag_spacing));
                    Complex new_to   = new Complex(new_from.Real+real_spacing,new_from.Imaginary + imag_spacing);

                    taskList.Add(new Task<MandelbrotSet>(() => GetSet(new_from, new_to, step / SPLIT, iter)));
                }
            }


            foreach(Task<MandelbrotSet> task in taskList) {
                task.Start();
            }

            bool is_not_completed = false;
            while (!is_not_completed) {
                foreach (Task<MandelbrotSet> task in taskList) { 
                    is_not_completed = true;
                    if (!task.IsCompleted) {
                        is_not_completed = false;
                    }
                }
            }

            List<MandelbrotSet> results = new List<MandelbrotSet>();
            foreach(Task<MandelbrotSet> task in taskList) {
                results.Add(task.Result);
            }

            double ratio = (v2_to.Imaginary - v2_from.Imaginary) / (v2_to.Real - v2_from.Real);

            return new MandelbrotSet(MandelbrotSet.Merge(results, (int)step, (int)(step * ratio),SPLIT));
        }

        private MandelbrotSet GetSet(Complex from, Complex to, double step, int iter) {
            return new MandelbrotSet(from, to, step, iter);
        }

    }
}
