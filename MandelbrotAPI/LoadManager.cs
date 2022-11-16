using System.Diagnostics;
using System.Numerics;

namespace MandelbrotAPI {
    public class LoadManager {

        public bool jobs_done;
        private List<Task<MandelBrotPart>> jobs;
        private int split;
        public LoadManager(int split) {
        
            jobs = new List<Task<MandelBrotPart>>();
            jobs_done = false;
            this.split = split;

        }

        public void CreateJobs(Complex from,Complex to,double step, int iter) {

            double real_spacing = (to.Real - from.Real) / split;
            double imag_spacing = (to.Imaginary - from.Imaginary) / split;

            List<Task<MandelBrotPart>> taskList = new List<Task<MandelBrotPart>>();
            for (int i = 0; i < split; i++) {
                for (int j = 0; j < split; j++) {

                    Complex new_from = new Complex(from.Real + (i * real_spacing), from.Imaginary + (j * imag_spacing));
                    Complex new_to = new Complex(new_from.Real + real_spacing, new_from.Imaginary + imag_spacing);

                    int off_i = i;
                    int off_j = j;

                    QueueCall qc = new QueueCall(off_i, off_j, new_from, new_to, step / split, iter);

                    jobs.Add(new Task<MandelBrotPart>(qc.Compute));
                }
            }
        }

        public void Start() {
            foreach (Task<MandelBrotPart> task in jobs) {
                task.Start();
            }
        }

        public void Wait() {
            while (!jobs_done) {
                foreach (Task<MandelBrotPart> task in jobs) {
                    jobs_done = true;
                    if (!task.IsCompleted) {
                        jobs_done = false;
                    }
                }
            }
        }
        
        public MandelbrotSet GetResult(Complex from, Complex to, double step) {

            List<MandelBrotPart> results = new List<MandelBrotPart>();

            foreach (Task<MandelBrotPart> task in jobs) {
                results.Add(task.Result);
            }

            double ratio = Math.Abs((to.Imaginary - from.Imaginary) / (to.Real - from.Real));

            return new MandelbrotSet(MandelBrotPart.Merge(results, (int)step, (int)(step * ratio), split));
        }

    }
}
