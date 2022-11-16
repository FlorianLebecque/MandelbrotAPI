using System.Numerics;

namespace MandelbrotAPI {
    public class QueueCall {

        private int off_i, off_j, iter;
        private Complex from, to;
        private double step;

        public QueueCall(int off_i, int off_j, Complex from, Complex to, double step, int iter) {
            this.off_i = off_i;
            this.off_j = off_j;
            this.from = from;
            this.to = to;
            this.step = step;
            this.iter = iter;
        }

        public MandelBrotPart Compute() {
            return new MandelBrotPart(off_i, off_j, from, to, step, iter);
        }
    }
}
