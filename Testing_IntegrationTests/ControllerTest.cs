using System.Net;
using System.Net.Http.Json;
using System.Threading;
using AutoFixture.Xunit2;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using wdpr_project.Controllers_;
using wdpr_project.Data;
using wdpr_project.Models;
using Xunit.Abstractions;
using static wdpr_project.Controllers_.ResearchController;

namespace Testing_IntegrationTests;

public class ControllerTests : IntegrationTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ControllerTests(ApiWebApplicationFactory fixture, ITestOutputHelper testOutputHelper) : base(fixture)
    {
        _testOutputHelper = testOutputHelper;
    }



    [Fact]
     public async void TestResearch()
    {
        //Arrange

        // Assert
        var response = await _client.GetAsync("/Research");
        response.EnsureSuccessStatusCode(); // Ensure a successful HTTP status code (2xx)

        // Parse the response content to extract the list of Research objects
        var researchList = await response.Content.ReadFromJsonAsync<List<Research>>();
        
        // Assert that the researchList is not null (or empty) based on your expectations
        Assert.NotNull(researchList);

    }

}