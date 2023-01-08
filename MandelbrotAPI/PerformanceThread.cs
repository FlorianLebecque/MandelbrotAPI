using System.Diagnostics;

namespace MandelbrotAPI {
    public class PerformanceThread  {

        private Thread trd;

        private ThreadResult perf;

        private PerformanceCounter a;

        public PerformanceThread() {
            perf = new ThreadResult();
            a = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            trd  = new Thread(ComputePerformance);
            trd.Start();
        }

        private void ComputePerformance() {

            while (true) {
                 
                a.NextValue();

                Thread.Sleep(1000);
                lock (this.perf) {
                    this.perf.value = a.NextValue();
                }
            }
        }

        public float GetPerformance() {
            float value;
            lock (this.perf) {
                value = this.perf.value;
            }

            return value;
        }

    }

    class ThreadResult {
        public float value;
    }
}
