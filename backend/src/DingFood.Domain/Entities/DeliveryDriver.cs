using DingFood.Domain.Primitives;

namespace DingFood.Domain.Entities;

public sealed class DeliveryDriver : AggregateRoot
{
    private DeliveryDriver() : base(0) { }
    public long BranchId { get; private set; }
    public string Name { get; private set; } = "";
    public string Phone { get; private set; } = "";
    public string VehiclePlate { get; private set; } = "";
    public string EmploymentType { get; private set; } = "";
    public bool IsActive { get; private set; } = true;
    public static DeliveryDriver Create(long branchId, string name, string phone, string plate, string employmentType)
    {
        var driver = new DeliveryDriver { BranchId = branchId };
        driver.Update(name, phone, plate, employmentType, true);
        return driver;
    }
    public void Update(string name, string phone, string plate, string employmentType, bool active)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > 150 || string.IsNullOrWhiteSpace(phone) || phone.Length > 20 ||
            string.IsNullOrWhiteSpace(plate) || plate.Length > 10 || employmentType is not ("Fixo" or "Freelancer"))
            throw new ArgumentException("Informe nome, telefone, placa e vínculo (Fixo/Freelancer) válidos.");
        Name = name.Trim(); Phone = phone.Trim(); VehiclePlate = plate.Trim().ToUpperInvariant(); EmploymentType = employmentType; IsActive = active;
    }
}
