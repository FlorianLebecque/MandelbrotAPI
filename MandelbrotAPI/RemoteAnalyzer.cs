﻿using System.Diagnostics;

namespace MandelbrotAPI {
    public class RemoteAnalyzer {

        private List<string> remotes;
        private Thread trd;
        private Dictionary<string, RemoteResult> remoteResult;
        private static HttpClient client;
        public RemoteAnalyzer(List<string> remotes){
            if(client== null) {
                client = new HttpClient();
            }

            this.remotes = remotes;
            this.remoteResult = new();

            foreach(string s in remotes) {
                remoteResult.Add(s, new());
            }


            trd = new(ThreadFunction);
        }

        private void ThreadFunction() {

            Stopwatch timer = new Stopwatch();

            while (true) {
                foreach(string s in remotes) {

                    string path = string.Format("{0}/api/Diagnostic",s);

                    timer.Start();

                    Task<HttpResponseMessage> resp = client.GetAsync(path);

                    resp.Wait();

                    timer.Stop();


                    var cpu = resp.Result.Content;
                    var ping = timer.ElapsedMilliseconds;

                    lock (remoteResult[s]) {
                        remoteResult[s].cpu = (float)Convert.ToDouble(cpu);
                        remoteResult[s].ping = ping;
                    }
                }
            }
        }
    
        public RemoteResult GetResult(string remote) {
            RemoteResult res;
            lock (remoteResult[remote]) { 
                res = remoteResult[remote];
            }

            return res;
        }
    
    }

    public class RemoteResult {
        public float ping;
        public float cpu;
    }
}