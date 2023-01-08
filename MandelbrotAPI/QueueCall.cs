using System.Net.Http;
using System.Net.Http.Headers;
using System.Numerics;

namespace MandelbrotAPI {
    public class QueueCall {

        private int off_i, off_j, iter;
        private Complex from, to;
        private double step;

        private static HttpClient client;

        public QueueCall(int off_i, int off_j, Complex from, Complex to, double step, int iter) {
            this.off_i = off_i;
            this.off_j = off_j;
            this.from = from;
            this.to = to;
            this.step = step;
            this.iter = iter;

            if( client == null) {
                client = new HttpClient();
            } 
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public MandelBrotPart Compute() {
            return new MandelBrotPart(off_i, off_j, from, to, step, iter);
        }

        public Task<MandelBrotPart> Request(string ip) {

            string from_str = string.Format("{0}:{1}",from.Real,from.Imaginary);
            string to_str   = string.Format("{0}:{1}",to.Real, to.Imaginary);


            string path = string.Format("{0}/api/MandelbrotComput?off_i={1}&off_j={2}&from={3}&to={4}&step={5}&iter={6}", ip,off_i,off_j, from_str, to_str, step,iter);

            Task<HttpResponseMessage> resp = client.GetAsync(path);

            resp.Wait();

            var rep = resp.Result;

            return rep.Content.ReadFromJsonAsync<MandelBrotPart>();
        }
    }
}
