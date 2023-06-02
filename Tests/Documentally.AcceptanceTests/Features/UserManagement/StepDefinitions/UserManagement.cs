using Documentally.Contracts.Authentication;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Documentally.AcceptanceTests.Features.UserManagement.StepDefinitions
{
    [Binding]
    public partial class UserManagement
    {
        private readonly HttpClient client;
        private readonly FeatureContext featureContext;
        private readonly ScenarioContext scenarioContext;
        private HttpResponseMessage? response = null;

        public UserManagement(HttpClient client, ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            this.client = client;
            this.scenarioContext = scenarioContext;
            this.featureContext = featureContext;
        }



        [When(@"I send a POST request to ""(.*)""")]
        public async Task WhenISendAPOSTRequestTo(string url)
        {
            // Retrieve the request data from the shared context
            var requestData = scenarioContext.Get<object>("RequestData");

            if (featureContext.TryGetValue("AuthenticatedUser", out AuthenticationResponse user))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
            }

            // Send a POST request to the specified URL with the requestData as the payload
            response = await client.PostAsJsonAsync(url, requestData);
        }

        [Then(@"the response status code should be (.*)")]
        public void ThenTheResponseStatusCodeShouldBe(int expectedStatusCode)
        {
            response?.StatusCode.Should().Be((System.Net.HttpStatusCode)expectedStatusCode);
        }
    }
}
