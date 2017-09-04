using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RemoteRobotLib
{
    public class RemoteRobot
    {
        readonly string _url;
        readonly HttpClient _client;
        readonly string _taskName;

        const string ModuleName = "Remote";

        public RemoteRobot(string url, string taskName, HttpClient client)
        {
            _url = url;
            _taskName = taskName;
            _client = client;
        }

        public async Task RunProcedure(string procedureName)
        {
            await SetStringVariable("name", $"\"{procedureName}\"");
            //await Task.Delay(1000);
            await SetBoolVariable("start", true);
            await WaitForBoolValue("start", false);
            await WaitForBoolValue("running", false);
        }

        async Task WaitForBoolValue(string name, bool value)
        {
            // Setting up subscriptions requires a websocket connection.
            // Let's use polling for now
            Debug.Write("Waiting");
            while (await GetBoolVariable(name) != value)
            {
                await Task.Delay(100);
                Debug.Write(".");
            }
            Debug.WriteLine("");
        }

        async Task<bool> GetBoolVariable(string name)
        {
            string urlString = $"http://{_url}/rw/rapid/symbol/data/RAPID/{_taskName}/{ModuleName}/{name}?json=1";
            var response = await _client.GetAsync(urlString);
            var content = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(content);
            string value= json["_embedded"]["_state"][0]["value"].ToString();
            return ParseBoolString(value);
        }

        async Task SetBoolVariable(string name, bool value)
        {
            await SetStringVariable(name, GetBoolString(value));
        }

        async Task SetStringVariable(string name, string value)
        {
            var values = new Dictionary<string, string>
            {
                {"value", value },
            };
            var content = new FormUrlEncodedContent(values);
            string urlString = $"http://{_url}/rw/rapid/symbol/data/RAPID/{_taskName}/{ModuleName}/{name}?action=set";
            var response = await _client.PostAsync(urlString, content);
            var responseString = await response.Content.ReadAsStringAsync();

            Debug.Print($"Status: {response.StatusCode}");
            Debug.Print(responseString);
        }

        string GetBoolString(bool value)
        {
            return value ? "TRUE" : "FALSE";
        }

        bool ParseBoolString(string value)
        {
            if (value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            else if (value.Equals("false", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }
            else
            {
                throw new Exception("Unexpected bool value");
            }
        }

    }
}
