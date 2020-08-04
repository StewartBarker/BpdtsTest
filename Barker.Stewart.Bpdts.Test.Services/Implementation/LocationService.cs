namespace Barker.Stewart.Bpdts.Test.Services
{
    using System;
    using Barker.Stewart.Bpdts.Test.Models;
    using GeoCoordinatePortable;

    public class LocationService : ILocationService
    {
        private double ConvertMilesToMetres(double miles) => miles * 1609.344;

        public bool IsWithinMilesOfLocation(ILocation source, int miles, ILocation target)
        {
            if (source == null || target == null)
                throw new ArgumentNullException();

            var sourceCoordinate = new GeoCoordinate(source.Latitude, source.Longitude);
            var targetCoordinate = new GeoCoordinate(target.Latitude, target.Longitude);

            return sourceCoordinate.GetDistanceTo(targetCoordinate) < ConvertMilesToMetres(miles);
        }
    }
}
