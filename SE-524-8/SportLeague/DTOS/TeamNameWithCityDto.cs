namespace SportLeague.DTOS
{
    public class TeamNameWithCityDto
    {
        public string TeamName { get; set; }
        public string City { get; set; }

        public override string ToString()
        {
            return $"Team: {TeamName}, City: {City}";
        }
    }
}
