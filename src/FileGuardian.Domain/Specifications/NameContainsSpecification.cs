using System.Linq.Expressions;
using File = FileGuardian.Domain.Entities.File;

namespace FileGuardian.Domain.Specifications;

public class NameContainsSpecification(string substring) : Specification<File>
{
    public override Expression<Func<File, bool>> ToExpression()
    {
        return file => file.Name.Contains(substring);
    }
}
