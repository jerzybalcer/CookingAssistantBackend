namespace CookingAssistantBackend.Models.DTOs
{
    public class TagDto
    {
        public TagDto(int tagId, string name)
        {
            TagId = tagId;
            Name = name;
        }
        public TagDto()
        {

        }

        public int TagId { get; set; }
        public string Name { get; set; }
    }
}
