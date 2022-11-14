using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;

namespace MandelbrotAPI {
    public class MandelbrotSet {
        public Complex from { get; set; }
        public Complex to { get; set; }

        private double step { get; set; }

        private int max_iter { get; set; }

        private double ratio { get; set; }

        public List<List<int>> points { get {

                var finalPoints = new List<List<int>>();

                for (int i = 0; i < pts.GetLength(0); i++) {
                    var temp = new List<int>();
                    for(int j = 0; j < pts.GetLength(1); j++) {
                        temp.Add(pts[i, j]);
                    }
                    finalPoints.Add(temp);
                }

                return finalPoints;
            }
        }

        public int[,] pts;

        public MandelbrotSet(Complex from, Complex to, double step,int iter) {

            this.from   = from;
            this.to     = to;
            this.step = step;
            this.max_iter   = iter;

            this.ratio = (this.to.Imaginary - this.from.Imaginary) / (this.to.Real - this.from.Real);

            pts = this.compute();
        }

        public MandelbrotSet(int[,] pts) {
            this.pts = pts;
        }

        private int[,] compute() {

            Complex c;

            int[,] points_ = new int[(int)this.step,(int)(this.step*this.ratio)];

            double r = 0;
            double i = 0;

            for (int x = 0; x < (int)this.step; x++) {
                for(int y = 0; y < (int)(this.step * this.ratio); y++) {

                    r = this.from.Real + (((x) / (this.step)) * (this.to.Real - this.from.Real));
                    i = this.from.Imaginary + (((y) / (this.step*this.ratio)) * (this.to.Imaginary - this.from.Imaginary));

                    c = new Complex(r, i);

                    points_[x, y] = this.IsInMandel(c, this.max_iter);
                }
            }

            return points_;
        }

        private int IsInMandel(Complex z,int max_iter) {

            Complex a = new Complex(z.Real,z.Imaginary);
            int n = 0;
            while(n < max_iter) {

                a = Complex.Pow(a, 2) + z;

                if(a.Magnitude > 2) {
                    return n;
                }

                n++;
            }

            return n;
        }

        public byte[]? SaveImg() {

            // Create 2D array of integers
            int width = (int)this.step;
            int height = (int)(this.step*this.ratio);

            int stride = width * 4;
            int[,] integers = new int[width, height];

           
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {

                    byte colors = (byte)(16 * pts[x,y]);

                    byte[] bgra = new byte[] { colors, colors, colors, 255 };
                    
                    integers[x, y] = BitConverter.ToInt32(bgra, 0);
                }
            }
            

            // Copy into bitmap
            Bitmap bitmap;
            unsafe {
                fixed (int* intPtr = &integers[0, 0]) {
                    bitmap = new Bitmap(width, height, stride, PixelFormat.Format32bppRgb, new IntPtr(intPtr));
                }
            }


            ImageConverter converter = new ImageConverter();
            return converter.ConvertTo(bitmap, typeof(byte[])) as byte[];

        }


        public static int[,] Merge(List<MandelbrotSet> list, int width,int height,int split) {

            if (split == 1) {
                return list[0].pts;
            }

            int[,] results = new int[width,height];
            int[] arr = new int[0];

            list.OrderBy(x => x.from.Real).ThenBy(x => x.from.Imaginary);


            int h_span = width / split;
            int v_span = height / split;

            for(int r = 0; r < list.Count; r++) {
                MandelbrotSet mbs = list[r];

                int[] baData = new int[mbs.pts.Length];
                Buffer.BlockCopy(mbs.pts, 0, baData, 0, mbs.pts.Length*sizeof(int));
                arr = arr.Concat(baData).ToArray();
            }

            return MandelbrotSet.Make2DArray(arr,width,height,split);
        }

        private static T[,] Make2DArray<T>(T[] input, int width, int height, int split) {
            T[,] output = new T[width, height];

            int h_span = width / split;
            int v_span = height / split;

            int region_size = h_span * v_span;
            int row_size = h_span;

            for (int i = 0; i < input.Length; i++) {

                int region = i / (region_size);
                int row = ((i - (region*region_size)) / h_span);

                int index = (i - (row*h_span) - (region * region_size));

                output[index,row] = input[i];
            }

            return output;
        }
    }
}
