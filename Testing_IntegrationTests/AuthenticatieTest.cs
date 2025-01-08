
using System.Net;
using System.Net.Http.Json;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.EntityFrameworkCore;
using wdpr_project.Data;
using wdpr_project.Models;
using Xunit.Abstractions;

namespace Testing_IntegrationTests;

public class AuthenticatieTests : IntegrationTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public AuthenticatieTests(ApiWebApplicationFactory fixture, ITestOutputHelper testOutputHelper) : base(fixture)
    {
        _testOutputHelper = testOutputHelper;
    }



    [Fact]
    public async void TestLoginSucces()
    {
        //Arrange
        var username = "Youneselbali1295";
        var password = "Youneselbali1295@";
        
        //Act

        var loginRequest = new { Username = username, Password = password };
        var response = await _client.PostAsJsonAsync("/Login", loginRequest);

        // Assert
       
        var token = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(token);
    }

     [Fact]
    public async void TestLoginFailed()
    {
        //Arrange
        var username = "Youneselbali129";
        var password = "Youneselbali1295@";
        
        //Act

        var loginRequest = new { Username = username, Password = password };
        var response = await _client.PostAsJsonAsync("/Login", loginRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        var errorMessage = await response.Content.ReadAsStringAsync();
        Assert.Contains("Invalide gebruikersnaam of wachtwoord", errorMessage);
    
    }

    

}


