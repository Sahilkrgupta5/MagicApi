using MagicApi.Models.Dto;

namespace MagicApi.DataStore
{
    public static class MagicStore
    {
        public static List<MagicDto> magicList = new List<MagicDto>
            {
                new MagicDto { Id=1, Name="Pool View", Occupancy=4, Sqft=100 },
                new MagicDto { Id=2, Name="Beach View", Occupancy=3, Sqft=300}
            };
    }
}
