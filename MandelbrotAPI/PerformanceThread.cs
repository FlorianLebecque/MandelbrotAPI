using System.Diagnostics;

namespace MandelbrotAPI {
    public class PerformanceThread  {

        private Thread trd;

        private ThreadResult perf;

        private PerformanceCounter perf_counter;

        public PerformanceThread() {
            perf = new ThreadResult();
            perf_counter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            trd  = new Thread(ComputePerformance);
            trd.Start();
        }

        private void ComputePerformance() {

            while (true) {
                 
                perf_counter.NextValue();

                Thread.Sleep(1000);
                lock (this.perf) {
                    this.perf.value = perf_counter.NextValue();
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
