using DIRS21.Mapping.Core.Base;
using DIRS21.Mapping.Models.Internal;
using DIRS21.Mapping.Models.External.Google;

namespace DIRS21.Mapping.Mappers.Google
{
    /// <summary>
    /// Maps DIRS21 Room to Google format
    /// </summary>
    public class RoomToGoogleMapper : MapperBase<Room, GoogleRoom>
    {
        public override string SourceType => "Google.Room";
        public override string TargetType => "Model.Room";

        protected override GoogleRoom MapInternal(Room source)
        {
            return new GoogleRoom
            {
                Id = source.RoomId,
                RoomType = source.Type,
                Rate = (double)source.Price
            };
        }
    }
}