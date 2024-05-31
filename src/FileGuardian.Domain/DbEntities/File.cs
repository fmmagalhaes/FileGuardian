namespace FileGuardian.Domain.Entities;

public class File
{
    public int Id { get; set; }
    public required string Name { get; set; }

    private int _risk;
    public int Risk
    {
        get { return _risk; }
        set
        {
            // Design validations in the domain model layer:
            // https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/domain-model-layer-validations
            if (value < 0 || value > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(Risk), "Risk must be between 0 and 100");
            }
            _risk = value;
        }
    }

    // Navigation properties
    public List<FileUser> FileUsers { get; set; } = [];
    public List<FileGroup> FileGroups { get; set; } = [];
}
