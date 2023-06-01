using BoDi;
using Documentally.Contracts.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using TechTalk.SpecFlow;

namespace Documentally.AcceptanceTests.Hooks
{
    [Binding]
    public class SpecFlowHooks
    {
        private readonly IObjectContainer _objectContainer;

        public SpecFlowHooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:5001")
            };

            _objectContainer.RegisterInstanceAs(httpClient);
        }

        [AfterScenario]
        public void AfterScenario()
        {
        }
    }
}
