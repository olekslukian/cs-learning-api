
namespace DotnetAPI.DTOs
{
    public class PostToAddDTO
    {
        public string PostTitle { get; set; }
        public string PostContent { get; set; }

        public PostToAddDTO()
        {
            PostTitle ??= "";
            PostContent ??= "";
        }
    }
}