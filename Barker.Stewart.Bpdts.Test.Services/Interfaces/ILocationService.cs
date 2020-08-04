namespace Barker.Stewart.Bpdts.Test.Services
{
    using Barker.Stewart.Bpdts.Test.Models;

    public interface ILocationService
    {
        bool IsWithinMilesOfLocation(ILocation sourceLocation, int miles, ILocation targetLocation);
    }
}
