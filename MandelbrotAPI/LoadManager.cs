using System.Diagnostics;
using System.Numerics;

namespace MandelbrotAPI {
    public class LoadManager {

        private bool jobs_done;
        private List<Task<MandelBrotPart>> jobs;
        private List<QueueCall> queues;
        private RemoteAnalyzer ra;
        public LoadManager(List<string> remotes) {

            ra = new RemoteAnalyzer(remotes);

            queues = new();
            jobs = new();
            jobs_done = false;
        }

        public void CreateJobs(int split,Complex from,Complex to,double step, int iter) {
            double real_spacing = (to.Real - from.Real) / split;
            double imag_spacing = (to.Imaginary - from.Imaginary) / split;

            List<Task<MandelBrotPart>> taskList = new List<Task<MandelBrotPart>>();
            for (int i = 0; i < split; i++) {
                for (int j = 0; j < split; j++) {
                    Complex new_from = new Complex(from.Real + (i * real_spacing), from.Imaginary + (j * imag_spacing));
                    Complex new_to = new Complex(new_from.Real + real_spacing, new_from.Imaginary + imag_spacing);

                    int off_i = i;
                    int off_j = j;

                    queues.Add(new QueueCall(off_i, off_j, new_from, new_to, step / split, iter));
                }
            }
        }

        public void ManageLoad() {

            Dictionary<string,RemoteResult> remotes = ra.GetRemoteResult();
            var sortedDict = from result in remotes orderby result.Value.ping ascending select result;

            int host = 0;
            int tries = 0;
            foreach(QueueCall qc in queues) {

                tries = 0;
                string key = remotes.Keys.ToArray()[host % remotes.Count];

                while ((remotes[key].cpu > 70)&&(tries < remotes.Count)){
                    key = remotes.Keys.ToArray()[host % remotes.Count];
                    
                    tries++;
                    host++;
                }

                jobs.Add(qc.Request(key));

                host++;
            }

        }

        public void Start() {
            foreach (Task<MandelBrotPart> task in jobs) {
                if(task.Status == TaskStatus.Created) {
                    task.Start();
                }
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
        
        public MandelbrotSet GetResult(int split,Complex from, Complex to, double step) {

            List<MandelBrotPart> results = new List<MandelBrotPart>();

            foreach (Task<MandelBrotPart> task in jobs) {
                results.Add(task.Result);
            }

            double ratio = Math.Abs((to.Imaginary - from.Imaginary) / (to.Real - from.Real));

            return new MandelbrotSet(MandelBrotPart.Merge(results, (int)step, (int)(step * ratio), split));
        }

    }
}
