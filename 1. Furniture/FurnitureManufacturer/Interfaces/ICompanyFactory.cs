namespace FurnitureManufacturer.Interfaces
{
    public interface ICompanyFactory
    {
        ICompany CreateCompany(string name, string registrationNumber);
    }
}
