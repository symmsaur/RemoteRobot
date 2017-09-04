using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RemoteRobotLib
{
    public static class RobotClientProvider
    {
        static Dictionary<string, HttpClient> _clients = new Dictionary<string, HttpClient>();

        public static HttpClient GetHttpClient(string url)
        {
            HttpClient client;
            if (!_clients.TryGetValue(url, out client))
            {
                var credentialCache = new CredentialCache();
                credentialCache.Add(new Uri($"http://{url}/rw"), "Digest",
                    new NetworkCredential("Default User", "robotics"));
                var httpClientHandler = new HttpClientHandler();
                client = new HttpClient(new HttpClientHandler { Credentials = credentialCache });
                _clients.Add(url, client);
            }
            return client;
        }
    }
}
