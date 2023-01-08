using System.Numerics;
using System.Text.Json.Serialization;

namespace MandelbrotAPI {
    public class MandelBrotPart {

        public int off_i { get; set; } 
        public int off_j { get;set; }
        public MandelbrotSet mbs { get; set; }

        public MandelBrotPart(int off_i,int off_j,Complex from, Complex to, double step, int iter) {

            this.off_i = off_i;
            this.off_j = off_j;

            mbs = new MandelbrotSet(from, to, step, iter);
        }


        [JsonConstructor]
        public MandelBrotPart(int off_i,int off_j,MandelbrotSet mbs) {
            this.off_i = off_i;
            this.off_j = off_j;
            this.mbs   = mbs;
        }

        public static int[,] Merge(List<MandelBrotPart> list, int width, int height, int split) {

            if (split == 1) {
                return list[0].mbs.pts;
            }

            int[,] results = new int[width, height];
            
            int h_span = width / split;
            int v_span = height / split;

            for (int r = 0; r < list.Count; r++) {
                MandelbrotSet mbs = list[r].mbs;

                int offset_i = list[r].off_i * h_span;
                int offset_j = list[r].off_j * v_span;

                for (int i = 0; i < mbs.pts.GetLength(0); i++) {
                    for (int j = 0; j < mbs.pts.GetLength(1); j++) {

                        results[offset_i + i, offset_j + j] = mbs.pts[i, j];

                    }
                }

            }

            return results;
        }

    }
}
