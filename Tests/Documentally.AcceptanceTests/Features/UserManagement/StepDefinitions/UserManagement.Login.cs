using Documentally.Contracts.Authentication;
using FluentAssertions;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace Documentally.AcceptanceTests.Features.UserManagement.StepDefinitions
{
    public partial class UserManagement // Login
    {

        [Given(@"I have the following admin credentials:")]
        public void GivenIHaveTheFollowingAdminCredentials(Table table)
        {
            var loginData = table.CreateInstance<LoginRequest>();

            scenarioContext.Set(loginData, "RequestData");
        }

        [Then(@"the response should contain the following authenticated user data:")]
        public async Task ThenTheResponseShouldContainTheFollowingAuthenticatedUserData(Table table)
        {
            var expectedResponseData = table.CreateInstance<AuthenticationResponse>();

            var actualResponseData = await response?.Content.ReadFromJsonAsync<AuthenticationResponse>()!;

            actualResponseData.Should().NotBeNull();
            actualResponseData!.Id.Should().BeGreaterThan(expectedResponseData.Id);
            actualResponseData!.FirstName.Should().BeEquivalentTo(expectedResponseData.FirstName);
            actualResponseData!.LastName.Should().BeEquivalentTo(expectedResponseData.LastName);
            actualResponseData!.Email.Should().BeEquivalentTo(expectedResponseData.Email);
            actualResponseData!.Token.Should().NotBeNullOrWhiteSpace(expectedResponseData.Token);
          
            featureContext.Set(actualResponseData, "AuthenticatedUser");
        }

    }
}
