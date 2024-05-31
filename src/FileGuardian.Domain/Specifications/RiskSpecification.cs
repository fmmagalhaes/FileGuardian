using System.Linq.Expressions;
using File = FileGuardian.Domain.Entities.File;

namespace FileGuardian.Domain.Specifications;

public class RiskSpecification(int risk) : Specification<File>
{
    public override Expression<Func<File, bool>> ToExpression()
    {
        return file => file.Risk >= risk;
    }
}
