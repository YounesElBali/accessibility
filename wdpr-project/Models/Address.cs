using AutoMapper;

namespace wdpr_project.Models;

public class Address
{
    public int Id { get; set; } //TODO add to UML | added as a foreign key, because a composite key is not guaranteed to be unique
    public string Postcode { get; set; }
    public string Adress { get; set; }
    

    public Address(){}

    public Address(string postcode, string adress)
    {
        Postcode = postcode;
        Adress = adress;
    }

    public void UpdateFields(Address updated)
    {
        if (updated.Id != Id)
        {
            return; //TODO: Throw error?
        }
        Postcode = updated.Postcode;
        Adress = updated.Adress;
    }
}

public class AddressDTO
{
    public int Id { get; set; }
    public string Postcode { get; set; }
    public string Adress { get; set; }
}

public class AddressProfile : Profile
{
    public AddressProfile()
    {
        CreateMap<Address, AddressDTO>();
    }
}