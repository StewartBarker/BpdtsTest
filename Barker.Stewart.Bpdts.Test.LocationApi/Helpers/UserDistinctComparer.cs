namespace Barker.Stewart.Bpdts.Test.LocationApi.Helpers
{
    using Barker.Stewart.Bpdts.Test.Models;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public class UserDistinctComparer : IEqualityComparer<User>
    {
        public bool Equals([AllowNull] User x, [AllowNull] User y)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (x == null || y == null)
                return false;
            return (x.Id.Equals(y.Id));
        }

        public int GetHashCode([DisallowNull] User obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
