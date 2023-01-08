using System.Diagnostics;

namespace MandelbrotAPI {
    public class RemoteAnalyzer {

        private List<RemoteResult> remotes;
        private Thread trd;
        private static HttpClient client;
        public RemoteAnalyzer(List<string> remotes){
            if(client== null) {
                client = new HttpClient();
            }

            this.remotes = new();

            foreach(string s in remotes) {
                this.remotes.Add(new(s));
            }


            trd = new(ThreadFunction);
            trd.Start();
        }

        private void ThreadFunction() {

            Stopwatch timer = new Stopwatch();

            while (true) {
                foreach(RemoteResult r in remotes) {

                    string path = string.Format("{0}/api/Diagnostic",r.host);

                    timer.Start();

                    Task<HttpResponseMessage> resp = client.GetAsync(path);

                    resp.Wait();

                    timer.Stop();


                    var cpu = resp.Result.Content.ReadAsStringAsync();
                    cpu.Wait();
                    var ping = timer.ElapsedMilliseconds;

                    lock (r) {
                        r.cpu = (float)Convert.ToDouble(cpu.Result);
                        r.ping = ping;
                    }
                }

                Thread.Sleep(5000); //actualize result every five seconds
            }
        }
        public Dictionary<string, RemoteResult> GetRemoteResult() {
            Dictionary<string, RemoteResult> a = new();
            foreach(RemoteResult r in remotes) {
                lock (r) {
                    a.Add(r.host, r);
                }
            }
            return a;
        }
    
    }

    public class RemoteResult {
        public float ping;
        public float cpu;
        public readonly string host;
        public RemoteResult(string host) {
            this.host = host;
        }
    }
}
