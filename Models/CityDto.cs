namespace CityInfo.API.Models
{
    /// <summary>
    /// DTO for a city with points of interest
    /// </summary>
    public class CityDto
    {
        /// <summary>
        /// The id of the city.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The name of the city.
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// The description of the city.
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// The number of points of interest for the city.
        /// </summary>
        public int NumberOfPointsOfInterest
        {
            get
            {
                return PointsOfInterest.Count;
            }
        }
        /// <summary>
        /// The points of interest for the city.
        /// </summary>
        public ICollection<PointOfInterestDto> PointsOfInterest { get; set; } = new List<PointOfInterestDto>();

    }
}
