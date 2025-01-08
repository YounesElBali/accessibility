using System.Net;
using System.Net.Http.Json;
using AutoFixture.Xunit2;
using Microsoft.EntityFrameworkCore;
using wdpr_project.Models;
using Xunit.Abstractions;

namespace Testing_IntegrationTests;

public class UserControllerTests : IntegrationTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public UserControllerTests(ApiWebApplicationFactory fixture, ITestOutputHelper testOutputHelper) : base(fixture)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    //Expert
    
    [Trait ("User", "expert")]
    [Theory, AutoDataNoId]
    public async Task GET_retrieves_experts(ExpertFullDTO[] experts)
    {
        //Arrange
        foreach (ExpertFullDTO expertdto in experts)
        {
            Expert expert = new Expert();
            await expert.UpdateFields(expertdto, _context);
            _context.Experts.Add(expert);
        }
        await _context.SaveChangesAsync();

        //Act
        var response = await _client.GetAsync("/Expert");
        
        //Assert
        response.EnsureSuccessStatusCode();

        var expertsResponse = await response.Content.ReadFromJsonAsync<List<ExpertBaseDTO>>();
        expertsResponse?.Count.Should().Be(experts.Length);
    }

    [Trait ("User", "expert")]
    [Theory, AutoDataNoId]
    public async Task GET_retrieves_specific_expert(ExpertFullDTO expertdto, DisabilityFullDTO[] disabilitydtos, DisabilityAidFullDTO[] aiddtos)
    {
        //Arrange
        List<Disability> disabilities = new List<Disability>();
        foreach (DisabilityFullDTO dto in disabilitydtos)
        {
            Disability disability = new Disability();
            disability.UpdateFields(dto);
            _context.Disabilities.Add(disability);
            disabilities.Add(disability);
        }
        List<DisabilityAid> aids = new List<DisabilityAid>();
        foreach (DisabilityAidFullDTO dto in aiddtos)
        {
            DisabilityAid disabilityAid = new DisabilityAid();
            disabilityAid.UpdateFields(dto);
            _context.DisabilityAids.Add(disabilityAid);
            aids.Add(disabilityAid);
        }

        await _context.SaveChangesAsync();

        expertdto.DisabilityIds = disabilities.Select(d => d.Id).ToList();
        expertdto.DisabilityAidIds = aids.Select(a => a.Id).ToList();

        Expert expert = new Expert();
        await expert.UpdateFields(expertdto, _context);
        
        _context.Experts.Add(expert);
        await _context.SaveChangesAsync();

        //Act
        var response = await _client.GetAsync($"/Expert/{expert.Id}");

        //Assert
        response.EnsureSuccessStatusCode();

        var expertResponse = await response.Content.ReadFromJsonAsync<ExpertDetailDTO>();
        expertResponse.Should().BeEquivalentTo(_mapper.Map<ExpertDetailDTO>(expert));
    }

    [Trait ("User", "expert")]
    [Theory, AutoData]
    public async Task GET_fails_when_expert_doesnt_exist(ExpertFullDTO expert)
    {
        //Arrange

        //Act
        var response = await _client.GetAsync($"/Expert/{expert.Id}");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Trait ("User", "expert")]
    [Theory, AutoDataNoId]
    public async Task POST_expert_persists_in_database(ExpertFullDTO expertdto, DisabilityFullDTO[] disabilitydtos, DisabilityAidFullDTO[] aiddtos)
    {
        //Arrange
        List<Disability> disabilities = new List<Disability>();
        foreach (DisabilityFullDTO dto in disabilitydtos)
        {
            Disability disability = new Disability();
            disability.UpdateFields(dto);
            _context.Disabilities.Add(disability);
            disabilities.Add(disability);
        }
        List<DisabilityAid> aids = new List<DisabilityAid>();
        foreach (DisabilityAidFullDTO dto in aiddtos)
        {
            DisabilityAid disabilityAid = new DisabilityAid();
            disabilityAid.UpdateFields(dto);
            _context.DisabilityAids.Add(disabilityAid);
            aids.Add(disabilityAid);
        }

        await _context.SaveChangesAsync();

        expertdto.DisabilityIds = disabilities.Select(d => d.Id).ToList();
        expertdto.DisabilityAidIds = aids.Select(a => a.Id).ToList();

        //Act
        var response = await _client.PostAsJsonAsync("/Expert", expertdto);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var expertResponse = await response.Content.ReadFromJsonAsync<ExpertFullDTO>();
        expertResponse.Should().NotBeNull();
        
        _testOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());

        var expertInDb = _context.Experts
            .Include(e => e.PersonalData)
            .ThenInclude(p => p.Address)
            .Include(e => e.Caretaker)
            .ThenInclude(p => p.Address)
            .Include(e => e.Disabilities).AsSplitQuery()
            .Include(e => e.Aids).AsSplitQuery()
            .FirstOrDefault(a => a.Id == expertResponse!.Id);
        expertInDb.Should().NotBeNull();

        expertResponse.Should().BeEquivalentTo(_mapper.Map<ExpertFullDTO>(expertInDb));
    }

    [Trait ("User", "expert")]
    [Theory, AutoData]
    public async Task PUT_fails_when_expert_doesnt_exist(ExpertFullDTO expert)
    {
        //Arrange
        
        //Act
        var response = await _client.PutAsJsonAsync("/Expert", expert);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Trait ("User", "expert")]
    [Theory, AutoDataNoId]
    public async Task PUT_expert_persists_in_database(string newUsername, ExpertFullDTO expertdto, DisabilityFullDTO[] disabilitydtos, DisabilityAidFullDTO[] aiddtos)
    {
        //Arrange
        List<Disability> disabilities = new List<Disability>();
        foreach (DisabilityFullDTO dto in disabilitydtos)
        {
            Disability disability = new Disability();
            disability.UpdateFields(dto);
            _context.Disabilities.Add(disability);
            disabilities.Add(disability);
        }
        List<DisabilityAid> aids = new List<DisabilityAid>();
        foreach (DisabilityAidFullDTO dto in aiddtos)
        {
            DisabilityAid disabilityAid = new DisabilityAid();
            disabilityAid.UpdateFields(dto);
            _context.DisabilityAids.Add(disabilityAid);
            aids.Add(disabilityAid);
        }

        await _context.SaveChangesAsync();

        expertdto.DisabilityIds = disabilities.Select(d => d.Id).ToList();
        expertdto.DisabilityAidIds = aids.Select(a => a.Id).ToList();

        Expert expert = new Expert();
        await expert.UpdateFields(expertdto, _context);
        
        _context.Experts.Add(expert);
        await _context.SaveChangesAsync();
        
        expertdto.Id = expert.Id;
        expertdto.Username = newUsername;

        //Act
        var response = await _client.PutAsJsonAsync($"/Expert/{expertdto.Id}", expertdto);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var expertInDb = _context.Experts.FirstOrDefault(a => a.Id == expert.Id);
        expertInDb.Should().NotBeNull();

        expertInDb.Should().BeEquivalentTo(expert);

    }

    [Trait ("User", "expert")]
    [Theory, AutoDataNoId]
    public async Task DELETE_expert_persists_in_database(ExpertFullDTO expertdto, DisabilityFullDTO[] disabilitydtos, DisabilityAidFullDTO[] aiddtos)
    {
        //Arrange
        List<Disability> disabilities = new List<Disability>();
        foreach (DisabilityFullDTO dto in disabilitydtos)
        {
            Disability disability = new Disability();
            disability.UpdateFields(dto);
            _context.Disabilities.Add(disability);
            disabilities.Add(disability);
        }
        List<DisabilityAid> aids = new List<DisabilityAid>();
        foreach (DisabilityAidFullDTO dto in aiddtos)
        {
            DisabilityAid disabilityAid = new DisabilityAid();
            disabilityAid.UpdateFields(dto);
            _context.DisabilityAids.Add(disabilityAid);
            aids.Add(disabilityAid);
        }

        await _context.SaveChangesAsync();

        expertdto.DisabilityIds = disabilities.Select(d => d.Id).ToList();
        expertdto.DisabilityAidIds = aids.Select(a => a.Id).ToList();

        Expert expert = new Expert();
        await expert.UpdateFields(expertdto, _context);
        
        _context.Experts.Add(expert);
        await _context.SaveChangesAsync();

        //Act
        var response = await _client.DeleteAsync($"/Expert/{expert.Id}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var expertInDb = _context.Experts.FirstOrDefault(a => a.Id == expert.Id);
        expertInDb.Should().BeNull();

        int aidsWithExpert = _context.DisabilityAids.Count(a => a.AidUsers.Contains(expert));
        aidsWithExpert.Should().Be(0);

        int disabilitiesWithExpert = _context.Disabilities.Count(d => d.DisabledExperts.Contains(expert));
        disabilitiesWithExpert.Should().Be(0);

    }
    
    //Business
    
    [Trait ("User", "business")]
    [Theory, AutoDataNoId]
    public async Task GET_retrieves_businesses(Business[] businesses)
    {
        //Arrange
        foreach (Business business in businesses)
        {
            _context.Businesses.Add(business);
        }
        await _context.SaveChangesAsync();

        //Act
        var response = await _client.GetAsync("/Business");
        
        //Assert
        response.EnsureSuccessStatusCode();

        var businessesResponse = await response.Content.ReadFromJsonAsync<List<BusinessDTO>>();
        businessesResponse?.Count.Should().Be(businesses.Length);
    }

    [Trait ("User", "business")]
    [Theory, AutoDataNoId]
    public async Task GET_retrieves_specific_business(Business business)
    {
        //Arrange
        _context.Businesses.Add(business);
        await _context.SaveChangesAsync();

        //Act
        var response = await _client.GetAsync($"/Business/{business.Id}");

        //Assert
        response.EnsureSuccessStatusCode();

        var businessResponse = await response.Content.ReadFromJsonAsync<BusinessDTO>();
        businessResponse.Should().BeEquivalentTo(_mapper.Map<BusinessDTO>(business));
    }

    [Trait ("User", "business")]
    [Theory, AutoData]
    public async Task GET_fails_when_business_doesnt_exist(Business business)
    {
        //Arrange

        //Act
        var response = await _client.GetAsync($"/Business/{business.Id}");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Trait ("User", "business")]
    [Theory, AutoDataNoId]
    public async Task POST_business_persists_in_database(Business business)
    {
        //Arrange

        //Act
        var response = await _client.PostAsJsonAsync("/Business", business);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var businessResponse = await response.Content.ReadFromJsonAsync<Business>();
        businessResponse.Should().NotBeNull();

        var businessInDb = _context.Businesses
            .Include(b => b.Address)
            .FirstOrDefault(a => a.Id == businessResponse!.Id);
        businessInDb.Should().NotBeNull();

        businessResponse.Should().BeEquivalentTo(businessInDb);
    }

    [Trait ("User", "business")]
    [Theory, AutoData]
    public async Task PUT_fails_when_business_doesnt_exist(Business business)
    {
        //Arrange
        
        //Act
        var response = await _client.PutAsJsonAsync("/Business", business);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Trait ("User", "business")]
    [Theory, AutoDataNoId]
    public async Task PUT_business_persists_in_database(Business business, string newUsername)
    {
        //Arrange
        _context.Businesses.Add(business);
        await _context.SaveChangesAsync();
        

        //Act
        var response = await _client.PutAsJsonAsync($"/Business/{business.Id}", business);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var businessInDb = _context.Businesses.FirstOrDefault(a => a.Id == business.Id);
        businessInDb.Should().NotBeNull();

        businessInDb.Should().BeEquivalentTo(business);

    }

    [Trait ("User", "business")]
    [Theory, AutoDataNoId]
    public async Task DELETE_business_persists_in_database(Business business)
    {
        //Arrange
        _context.Businesses.Add(business);
        await _context.SaveChangesAsync();

        //Act
        var response = await _client.DeleteAsync($"/Business/{business.Id}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var businessInDb = _context.Businesses.FirstOrDefault(a => a.Id == business.Id);
        businessInDb.Should().BeNull();

    }
    
    //Admin
    
    [Trait ("User", "admin")]
    [Theory, AutoDataNoId]
    public async Task GET_retrieves_admins(Admin[] admins)
    {
        //Arrange
        foreach (Admin admin in admins)
        {
            _context.Admins.Add(admin);
        }
        await _context.SaveChangesAsync();

        //Act
        var response = await _client.GetAsync("/Admin");
        
        //Assert
        response.EnsureSuccessStatusCode();

        var adminsResponse = await response.Content.ReadFromJsonAsync<List<AdminDTO>>();
        adminsResponse?.Count.Should().Be(admins.Length);
    }

    [Trait ("User", "admin")]
    [Theory, AutoDataNoId]
    public async Task GET_retrieves_specific_admin(Admin admin)
    {
        //Arrange
        _context.Admins.Add(admin);
        await _context.SaveChangesAsync();

        //Act
        var response = await _client.GetAsync($"/Admin/{admin.Id}");

        //Assert
        response.EnsureSuccessStatusCode();

        var adminResponse = await response.Content.ReadFromJsonAsync<AdminDTO>();
        adminResponse.Should().BeEquivalentTo(_mapper.Map<AdminDTO>(admin));
    }

    [Trait ("User", "admin")]
    [Theory, AutoData]
    public async Task GET_fails_when_admin_doesnt_exist(Admin admin)
    {
        //Arrange

        //Act
        var response = await _client.GetAsync($"/Admin/{admin.Id}");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Trait ("User", "admin")]
    [Theory, AutoDataNoId]
    public async Task POST_admin_persists_in_database(Admin admin)
    {
        //Arrange

        //Act
        var response = await _client.PostAsJsonAsync("/Admin", admin);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var adminResponse = await response.Content.ReadFromJsonAsync<Admin>();
        adminResponse.Should().NotBeNull();

        var adminInDb = _context.Admins.FirstOrDefault(a => a.Id == adminResponse!.Id);
        adminInDb.Should().NotBeNull();

        adminResponse.Should().BeEquivalentTo(adminInDb);
    }

    [Trait ("User", "admin")]
    [Theory, AutoData]
    public async Task PUT_fails_when_admin_doesnt_exist(Admin admin)
    {
        //Arrange
        
        //Act
        var response = await _client.PutAsJsonAsync("/Admin", admin);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Trait ("User", "admin")]
    [Theory, AutoDataNoId]
    public async Task PUT_admin_persists_in_database(Admin admin, string newUsername)
    {
        //Arrange
        _context.Admins.Add(admin);
        await _context.SaveChangesAsync();
        

        //Act
        var response = await _client.PutAsJsonAsync($"/Admin/{admin.Id}", admin);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var adminInDb = _context.Admins.FirstOrDefault(a => a.Id == admin.Id);
        adminInDb.Should().NotBeNull();

        adminInDb.Should().BeEquivalentTo(admin);

    }

    [Trait ("User", "admin")]
    [Theory, AutoDataNoId]
    public async Task DELETE_admin_persists_in_database(Admin admin)
    {
        //Arrange
        _context.Admins.Add(admin);
        await _context.SaveChangesAsync();

        //Act
        var response = await _client.DeleteAsync($"/Admin/{admin.Id}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var adminInDb = _context.Admins.FirstOrDefault(a => a.Id == admin.Id);
        adminInDb.Should().BeNull();

    }
}