namespace CRMScrap.Models;

public class RealEstateProperties
{
    public int Id { get; set; }
    public string Reference { get; set; }
    public string Nature { get; set; }
    public string Condition { get; set; }
    public string Typologi { get; set; }
    public string EnergiCertification { get; set; }
    public string YearConstraction { get; set; }
    public string Business { get; set; }
    public string Price { get; set; }
    public string Avaibility { get; set; }
    public string ContractNumber { get; set; }
    public string DateStart { get; set; }
    public string DateEnd { get; set; }
    public string CommisionAgenci { get; set; }
    public string Exlusiv { get; set; }
    public string AreaU { get; set; }
    public string AreaB { get; set; }
    public string AreaT { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public string Address { get; set; }
    public string DoorNumber { get; set; }
    public string Floor { get; set; }
    public string ZipCode { get; set; }
    public string Location { get; set; }
    public string State { get; set; }
    public string Town { get; set; }
    public string Neighborhood { get; set; }
    public string Description { get; set; }
    private readonly FeaturesRealEstate _featuresRealEstate = new FeaturesRealEstate();
}