using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using TechTalk.SpecFlow;
using TestProject.Hooks;

namespace TestProject.Steps
{
    [Binding]
    public class MainStepDefinitions
    {
        private readonly MainContext mainContext;
        private readonly CustomHttpClient _httpClient;

        public MainStepDefinitions(MainContext mainContext)
        {
            this.mainContext = mainContext;
            this._httpClient = new CustomHttpClient();
        }

        [When(@"I send request to ""(.*)"" with ""(.*)"" type and:")]
        public void WhenISendRequestToWithTypeAnd(string url, string type, Table parameters)
        {
            switch (type.ToUpper())
            {
                case "GET":
                    this.mainContext.Response = _httpClient.Get(url, createHeaders(parameters)).Result;
                    break;
                case "POST":
                    this.mainContext.Response = _httpClient.Post(url, createHeaders(parameters), null).Result;
                    break;
                default:
                    throw new ArgumentException($"Unable to send request with {type} type");
            }
        }

        [Then(@"I validate that response has ""(\d+)"" code")]
        public void ThenIValidateThatResponseHasCode(int responseCode)
        {
            int expectedStatusCode = (int)this.mainContext.Response.StatusCode;
            Assert.AreEqual(responseCode, expectedStatusCode,
                $"Expected to have \"{responseCode}\", but \"{expectedStatusCode}\" was found");
        }

        [Then(@"I validate that response has ""(.*)"" for ""(.*)""")]
        public void ThenIValidateThatResponseHasFor(string value, string key)
        {
            var contentString = this.mainContext.Response.Content.ReadAsStringAsync().Result;
            var content = new JObject(contentString);
            Assert.IsTrue(content.TryGetValue(key, out JToken responseValue), $"Expected response to conatin \"{key}\"");

            string responseValueString = responseValue.ToString();
            Assert.AreEqual(value, responseValue, $"Expected to have \"{value}\" for \"{key}\", but \"{responseValue}\" was found");
        }

        [Then(@"I validate that response has ""(.*)"" returned")]
        public void ThenIValidateThatResponseHasReturned(string key)
        {
            var contentString = this.mainContext.Response.Content.ReadAsStringAsync().Result;
            Assert.IsTrue(contentString.Contains(key), $"Expected to have \"{key}\" in the response, but it wasn't found");
        }

        [Given(@"I save uuid from response")]
        public void GivenISaveUuidFromResponse()
        {
            var response = this.mainContext.Response.Content.ReadAsStringAsync().Result;
            int uuidKeyIndex = response.IndexOf("uuid");
            string uuidResponse = response.Substring(uuidKeyIndex);
            uuidResponse = uuidResponse.Substring(uuidResponse.IndexOf(": "));
            uuidResponse = uuidResponse.Substring(uuidResponse.IndexOf("\"") + 1);
            uuidResponse = uuidResponse.Substring(0, uuidResponse.IndexOf("\""));
            this.mainContext.UUID = uuidResponse;
        }


        private Dictionary<string, string> createHeaders(Table parameters)
        {
            var headers = new Dictionary<string, string>();
            int headerIndex = parameters.Header.ToList().FindIndex(header => header == "Headers");
            for (int i = 0; i < parameters.Rows.Count; i++)
            {
                var headerValue = parameters.Rows[i][headerIndex];
                var headerValueSplit = headerValue.Split(": ");
                if (headerValueSplit[1].Contains("@"))
                {
                    switch (headerValueSplit[1].Substring(1))
                    {
                        case "uuid":
                            headerValueSplit[1] = this.mainContext.UUID;
                            break;
                        default:
                            throw new ArgumentException($"Unable to find variable \"{headerValueSplit[1].Substring(1)}\"");
                    }
                }
                headers.Add(headerValueSplit[0], headerValueSplit[1]);
            }
            return headers;
        }
    }
}
