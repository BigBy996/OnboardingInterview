using System.Net.Http;
using TechTalk.SpecFlow;

namespace TestProject.Hooks
{
    public class MainContext
    {
        private const string ResponseKey = "ResponseKey ";
        public HttpResponseMessage Response
        {
            get => ScenarioContext.Current.Get<HttpResponseMessage>(ResponseKey);
            set => ScenarioContext.Current.Set(value, ResponseKey);
        }

        private const string UUIDKey = "UUIDKey";
        public string UUID
        {
            get => ScenarioContext.Current.Get<string>(UUIDKey);
            set => ScenarioContext.Current.Set(value, UUIDKey);
        }
    }
}
