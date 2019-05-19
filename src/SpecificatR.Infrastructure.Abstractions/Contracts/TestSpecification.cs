using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SpecificatR.Infrastructure.Abstractions.Contracts
{
    public class TestSpecification: BaseSpecification<int>
    {
        public TestSpecification()
            :base(BuildCriteria())
        {
        }

        private static Expression<Func<int, bool>> BuildCriteria()
        {
            throw new NotImplementedException();
        }
    }
}
