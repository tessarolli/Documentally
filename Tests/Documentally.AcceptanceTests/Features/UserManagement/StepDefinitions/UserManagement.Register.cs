using Documentally.Contracts.Authentication;
using FluentAssertions;
using System.Net.Http.Json;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace Documentally.AcceptanceTests.Features.UserManagement.StepDefinitions
{
    public partial class UserManagement // Register
    {
        [Given(@"I have entered the following registration details:")]
        public void GivenIHaveEnteredTheFollowingRegistrationDetails(Table table)
        {
            // Retrieve the registration details from the table and store them for later use
            var registrationData = table.CreateInstance<RegisterRequest>();
           
            // Store the registrationData in a shared context 
            scenarioContext.Set(registrationData, "RequestData");
        }

        [Then(@"the register response should contain the following data and I should save the newly registered user info into the context:")]
        public async Task ThenTheRegisterResponseShouldContainTheFollowingDataAndIShouldSaveTheNewlyRegisteredUserInfoIntoTheContext(Table table)
        {
            var expectedResponseData = table.CreateInstance<AuthenticationResponse>();

            var actualResponseData = await response?.Content.ReadFromJsonAsync<AuthenticationResponse>()!;

            actualResponseData.Should().NotBeNull();
            actualResponseData!.Id.Should().BeGreaterThan(expectedResponseData.Id);
            actualResponseData!.FirstName.Should().BeEquivalentTo(expectedResponseData.FirstName);
            actualResponseData!.LastName.Should().BeEquivalentTo(expectedResponseData.LastName);
            actualResponseData!.Email.Should().BeEquivalentTo(expectedResponseData.Email);
            actualResponseData!.Token.Should().NotBeNullOrWhiteSpace(expectedResponseData.Token);

            scenarioContext.Set(actualResponseData, "NewlyRegisteredUser");
        }

    }
}
