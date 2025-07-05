using SPbSTU.OPD.ASAP.Core.Domain.ValueObjects;

namespace SPbSTU.OPD.ASAP.Core.Domain.Models;

public class Student
{
    public long Id { get; init; }
    public string Name { get; init; } = null!;
    public Position? Position { get; init; }

    public Student()
    {
    }

    public Student(long id, string name)
    {
        Id = id;
        Name = name;
    }
}