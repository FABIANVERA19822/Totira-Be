namespace CrestApps.RetsSdk.Models
{
    public class PhotoIdAlpha
    {
        public string Id { get; set; }
        public int? ObjectId { get; set; }

        public PhotoIdAlpha()
        {

        }

        public PhotoIdAlpha(string id, int? objectId = null)
        {
            Id = id;
            ObjectId = objectId;
        }


        public override string ToString()
        {
            if (!ObjectId.HasValue)
            {
                return string.Format("{0}:*", Id);
            }

            return string.Format("{0}:{1}", Id, ObjectId.Value);
        }
    }
}