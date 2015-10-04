using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.ModelBinding;
using Newtonsoft.Json;
using RestSharp;

namespace DevChallenge
{
    public class ChallengeModule : NancyModule
    {

        string guid;
        private string apiUrl = "http://internal-devchallenge-2-dev.apphb.com";
        string webhookUrl = "http://7088ea79.ngrok.io";
        string repoUrl = "https://github.com/WilliamAvila/Dev-Challenge-A-A";
        private Avengers avenger;
        private List<string> results; 
        public ChallengeModule()
        {
            Avengers av;
            Get["/"] = _ =>
            {
                avenger = new Avengers();
                results = new List<string>();
                string response = "";
                for (int i = 0; i < 20; i++)
                {
                    var obj = HttpGet();
                    var respo = JsonConvert.DeserializeObject<ResponseObject>(obj);

                    var encodedValue = ApplyAlgorithm(respo.words, respo.startingFibonacciNumber, respo.algorithm);

                    var responseP = HttpPost(encodedValue, respo.algorithm);
                    var responsePost = JsonConvert.DeserializeObject<ResponsePost>(responseP);
                    results.Add(responseP);

                }

                
                return response;
            };

            Post["/"] = _ =>
            {
                var response = this.Bind<ResponsePayload>();
                return response.secret;
            };

        }

       

        public string GetGuid()
        {
            Guid g;
            g = Guid.NewGuid();
            return g.ToString();
        }



        public string HttpGet()
        {
            guid = GetGuid();
            var client = new RestClient(apiUrl);
            var request = new RestRequest("values/" + guid, Method.GET);

            var response =  client.Execute(request);
            var content = response.Content;
            return content;
        }


        public string HttpPost(string encodedValue, string algorithm)
        {
            var client = new RestClient(apiUrl);
            var request = new RestRequest("/values/" + guid + "/" + algorithm, Method.POST);

            request.AddParameter("encodedValue", encodedValue);
            request.AddParameter("emailAddress", "william@acklenavenue.com");
            request.AddParameter("name", "William Avila");
            request.AddParameter("webhookUrl", webhookUrl);
            request.AddParameter("repoUrl", repoUrl);

            var response = client.Execute(request);
            var content = response.Content;

            return content;
            
        }

        public string ApplyAlgorithm(string[] words, double startingFibonacciNumber, string algorithm)
        {
            var result = "";
            int fib = (int)startingFibonacciNumber;
            if (algorithm == "IronMan")
                result = avenger.IronMan(words);
            else if (algorithm == "TheIncredibleHulk")
                result = avenger.TheIncredibleHulk(words);
            else if (algorithm == "Thor")
                result = avenger.Thor(words, fib);
            else if (algorithm == "CaptainAmerica")
                result = avenger.CaptainAmerica(words,fib);

            return result;
        }
    }

    public class ResponsePayload
    {
        public string secret { get; set; }
    }

    public class ResponseObject
    {
        public string[] words { get; set; }
        public double startingFibonacciNumber { get; set; }
        public string algorithm { get; set; }
    }

    internal class ResponsePost
    {
        public string status { get; set; }
        public string message { get; set; }
    }
}