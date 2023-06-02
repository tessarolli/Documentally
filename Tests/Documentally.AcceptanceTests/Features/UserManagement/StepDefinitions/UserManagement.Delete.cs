using Documentally.Contracts.Authentication;
using Documentally.Contracts.User.Responses;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Documentally.AcceptanceTests.Features.UserManagement.StepDefinitions
{
    public partial class UserManagement // Delete
    {

        [When(@"I want to delete the newly registered user")]
        public void WhenIWantToDeleteTheNewlyRegisteredUser()
        {
            var currentUser = scenarioContext.Get<UserResponse>("UserResponse");

            scenarioContext.Set(currentUser, "RequestData");
        }

        [When(@"I send a DELETE request to ""([^""]*)""")]
        public async Task WhenISendADELETERequestTo(string url)
        {
            var requestData = scenarioContext.Get<UserResponse>("RequestData");

            url = url.Replace("{id}", requestData.Id.ToString());

            if (featureContext.TryGetValue("AuthenticatedUser", out AuthenticationResponse user))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
            }

            response = await client.DeleteAsync(url);
        }
    }
}
