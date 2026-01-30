namespace WebApplication1.Entities;

public sealed class Customer
{
  public Guid Id { get; set; } = Guid.CreateVersion7();
  public required string Name { get; set; }
  public required string Email { get; set; }
  public required string PhoneNumber { get; set; }
  public required string Address { get; set; }
  public required string City { get; set; }
  public required string Region { get; set; }
  public required string PostalCode { get; set; }
  public required string Country { get; set; }
}
