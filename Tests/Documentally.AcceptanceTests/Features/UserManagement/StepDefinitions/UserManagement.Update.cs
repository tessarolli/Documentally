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
    public partial class UserManagement // Update
    {

        [When(@"I update the newly registered user details to:")]
        public void WhenIUpdateTheNewlyRegisteredUserDetailsTo(Table table)
        {
            var updatedUser = table.CreateInstance<UserResponse>();

            var currentUser = scenarioContext.Get<UserResponse>("UserResponse");

            var newUser = new UserResponse(
                currentUser.Id,
                updatedUser.FirstName,
                updatedUser.LastName,
                updatedUser.Email,
                currentUser.Password,
                updatedUser.Role,
                currentUser.CreatedAtUtc
            );


            scenarioContext.Set(newUser, "RequestData");
        }

        [When(@"I send a PUT request to ""([^""]*)""")]
        public async Task WhenISendAPUTRequestTo(string url)
        {
            var updatedUser = scenarioContext.Get<UserResponse>("RequestData");

            if (featureContext.TryGetValue("AuthenticatedUser", out AuthenticationResponse user))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
            }

            response = await client.PutAsJsonAsync(url, updatedUser);
        }


    }
}
