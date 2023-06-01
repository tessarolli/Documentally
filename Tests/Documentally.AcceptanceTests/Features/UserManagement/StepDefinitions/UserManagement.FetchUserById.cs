using Documentally.Contracts.Authentication;
using Documentally.Contracts.User.Responses;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Documentally.AcceptanceTests.Features.UserManagement.StepDefinitions
{
    public partial class UserManagement
    {

        [Given(@"I have stored the newly registered user in the Context Scenario")]
        public void GivenIHaveStoredTheNewlyRegisteredUserInTheContextScenario()
        {
            var hadUserStoredInContext = scenarioContext.TryGetValue("NewlyRegisteredUser", out AuthenticationResponse user);

            hadUserStoredInContext.Should().BeTrue();

            scenarioContext.Set(user.Id, "RequestData");
        }

        [When(@"I send a GET request to ""([^""]*)""")]
        public async Task WhenISendAGETRequestTo(string url)
        {
            var id = scenarioContext.Get<long>("RequestData");

            url = url.Replace("{id}", id.ToString());

            // Send a GET request to the specified URL
            response = await client.GetAsync(url);
        }

        [Then(@"the response should contain the following user data:")]
        public async Task ThenTheResponseShouldContainTheFollowingUserData(Table table)
        {
            var expectedResponseData = table.CreateInstance<UserResponse>();

            var actualResponseData = await response?.Content.ReadFromJsonAsync<UserResponse>()!;

            actualResponseData.Should().NotBeNull();
            actualResponseData!.Id.Should().BeGreaterThan(expectedResponseData.Id);
            actualResponseData!.FirstName.Should().BeEquivalentTo(expectedResponseData.FirstName);
            actualResponseData!.LastName.Should().BeEquivalentTo(expectedResponseData.LastName);
            actualResponseData!.Email.Should().BeEquivalentTo(expectedResponseData.Email);
            actualResponseData!.Password.Should().NotBeNullOrWhiteSpace();
            actualResponseData!.Role.Should().Be(0);
            actualResponseData!.CreatedAtUtc.Should().BeBefore(DateTime.UtcNow);
        }

    }
}
