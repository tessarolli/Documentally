using Documentally.Domain.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documentally.Domain.ValueObjects
{
    public sealed class UserId : ValueObject
    {
        public Guid Value { get; }

        public UserId(Guid? guid = null)
        {
            if (guid is null)
                Value = Guid.NewGuid();
            else
                Value = guid.Value;
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
