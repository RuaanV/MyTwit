using System.Device.Location;

namespace MyTwit.Services
{
    /// <summary>
    /// Device Location Service
    /// </summary>
    public interface ILocationService
    {
        /// <summary>
        /// Tries to get current location.
        /// </summary>
        /// <returns></returns>
        GeoCoordinate TryToGetCurrentLocation();
    }
}
