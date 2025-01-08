using AutoFixture;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;

namespace Testing_IntegrationTests;

public class AutoDataNoIdAttribute : AutoDataAttribute
{
    public AutoDataNoIdAttribute() : base(() =>
    {
        var fixture = new Fixture();
        fixture.Customizations.Add(new NoIdSpecimenBuilder());
        
        return fixture;
    })
    {
    }
}

public class NoIdSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        var propertyInfo = request as System.Reflection.PropertyInfo;
        if (propertyInfo != null &&
            propertyInfo.Name.Equals("Id"))
        {
            return new OmitSpecimen();
        }

        var fieldInfo = request as System.Reflection.FieldInfo;
        if (fieldInfo != null &&
            fieldInfo.Name.Equals("Id"))
        {
            return new OmitSpecimen();
        }

        return new NoSpecimen();
    }
}