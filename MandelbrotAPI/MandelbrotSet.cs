using Microsoft.AspNetCore.Server.IIS.Core;
using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;

namespace MandelbrotAPI {
    public class MandelbrotSet {
        private Complex from { get; set; }
        private Complex to { get; set; }

        private double step { get; set; }

        private int max_iter { get; set; }

        private double ratio { get; set; }

        public List<List<int>> points { get; set; }
            


        public MandelbrotSet(Complex from, Complex to, double step,int iter) {

            this.from   = from;
            this.to     = to;
            this.step = step;
            this.max_iter   = iter;

            this.ratio = (this.to.Imaginary - this.from.Imaginary) / (this.to.Real - this.from.Real);

            points = this.compute();
        }

        private List<List<int>> compute() {

            Complex a;
            Complex c;

            List<List<int>> points_ = new List<List<int>>();

            double r_inc = (to.Real - from.Real) / this.step;
            double i_inc = (to.Imaginary - from.Imaginary) / this.step;


            for (double r = from.Real; r < to.Real; r+= r_inc) {
                List<int> temp = new List<int>();
                for(double i = from.Imaginary; i < to.Imaginary; i += i_inc) {

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

        public byte[]? SaveImg() {

            //create image from list of points

            // Create 2D array of integers
            int width = points.Count();
            int height = points[0].Count();
            int stride = width * 4;
            int[,] integers = new int[width, height];

           
            for (int x = 0; x < points.Count(); x++) {
                for (int y = 0; y < points[x].Count(); y++) {

                    byte colors = (byte)(255 * points[x][y]);

                    byte[] bgra = new byte[] { colors, colors, colors, 255 };
                    
                    integers[x, y] = BitConverter.ToInt32(bgra, 0);
                }
            }

            

            // Copy into bitmap
            Bitmap bitmap;
            unsafe {
                fixed (int* intPtr = &integers[0, 0]) {
                    bitmap = new Bitmap(width, (int)(width * this.ratio), stride, PixelFormat.Format32bppRgb, new IntPtr(intPtr));
                }
            }


            ImageConverter converter = new ImageConverter();
            return converter.ConvertTo(bitmap, typeof(byte[])) as byte[];

        }
    }
}
