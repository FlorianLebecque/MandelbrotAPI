using Microsoft.AspNetCore.Server.IIS.Core;
using System.Numerics;

namespace MandelbrotAPI {
    public class MandelbrotSet {
        private Complex from { get; set; }
        private Complex to { get; set; }

        private double resolution { get; set; }

        private int max_iter { get; set; }

        public List<List<int>> points { get; set; }
            


        public MandelbrotSet(Complex from, Complex to, double res,int iter) {

            this.from   = from;
            this.to     = to;
            this.resolution = res;
            this.max_iter   = iter;

            points = this.compute();
        }

        private List<List<int>> compute() {

            Complex a;
            Complex c;

            List<List<int>> points_ = new List<List<int>>();

            for(double r = from.Real; r < to.Real; r+= this.resolution) {
                List<int> temp = new List<int>();
                for(double i = from.Imaginary; i < to.Imaginary; i += this.resolution) {

                    c = new Complex(r,i);

                    a = this.IsInMandel(c, this.max_iter);

                    temp.Add((a.Magnitude < 2) ? 1: 0);

                }
                points_.Add(temp);
            }

            return points_;
        }

        private Complex IsInMandel(Complex z,int n) {

            if(n == 1) {
                return 0;
            } else {
                return  Complex.Pow(IsInMandel(z, n - 1),2) + z;
            }

        }

    }
}
