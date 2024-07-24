using DotnetAPI.Data;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PostsController(IConfiguration config) : ControllerBase, IPostsController
    {
        private readonly DataContextDapper _dapper = new(config);

        [HttpGet("Posts")]
        public IEnumerable<Post> GetPosts()
        {
            string postsSql = @"SELECT [PostId],
                    [UserId],
                    [PostTitle],
                    [PostContent],
                    [PostCreated],
                    [PostUpdated] 
                FROM TutorialAppSchema.Posts";

            return _dapper.LoadData<Post>(postsSql);
        }

        [HttpGet("Post/{postId}")]
        public Post GetPostSingle(int postId)
        {
            string postSql = @"SELECT [PostId],
                    [UserId],
                    [PostTitle],
                    [PostContent],
                    [PostCreated],
                    [PostUpdated] 
                FROM TutorialAppSchema.Posts
                WHERE PostId = " + postId.ToString();

            return _dapper.LoadDataSingle<Post>(postSql);
        }

        [HttpGet("PostsByUser/{userId}")]
        public IEnumerable<Post> GetPostsByUser(int userId)
        {
            string postsSql = @"SELECT [PostId],
                    [UserId],
                    [PostTitle],
                    [PostContent],
                    [PostCreated],
                    [PostUpdated] 
                FROM TutorialAppSchema.Posts
                WHERE UserId = " + userId.ToString();

            return _dapper.LoadData<Post>(postsSql);
        }

        [HttpGet("MyPosts")]
        public IEnumerable<Post> GetMyPosts()
        {
            string postsSql = @"SELECT [PostId],
                    [UserId],
                    [PostTitle],
                    [PostContent],
                    [PostCreated],
                    [PostUpdated] 
                FROM TutorialAppSchema.Posts
                WHERE UserId = " + this.User.FindFirst("userId")?.Value;

            return _dapper.LoadData<Post>(postsSql);
        }

        [HttpPost("Post")]
        public IActionResult AddPost(PostToAddDTO post)
        {
            string addPostSql = @"
            INSERT INTO TutorialAppSchema.Posts(
                [UserId],
                [PostTitle],
                [PostContent],
                [PostCreated],
                [PostUpdated]
                ) VALUES (
                   " + this.User.FindFirst("userId")?.Value
                   + ",'" + post.PostTitle
                   + "','" + post.PostContent
                   + "', GETDATE(), GETDATE() )";

            if (_dapper.ExecuteSql(addPostSql))
            {
                return Ok();
            }

            throw new Exception("Failed to create new post");
        }

        [HttpPut("Post")]
        public IActionResult EditPost(PostToEditDTO post)
        {
            string editPostSql = @"
            UPDATE TutorialAppSchema.Posts 
                SET PostContent = '" + post.PostContent
                + "', PostTitle = '" + post.PostTitle
                + @"', PostUpdated = GETDATE()
                WHERE PostId = " + post.PostId.ToString() +
                "AND UserId = " + this.User.FindFirst("userId")?.Value;

            if (_dapper.ExecuteSql(editPostSql))
            {
                return Ok();
            }

            throw new Exception("Failed to edit post");
        }

        [HttpDelete("Post/{postId}")]
        public IActionResult DeletePost(int postId)
        {
            string deletePostSql = @"
                DELETE FROM TutorialAppSchema.Posts
                    WHERE PostId = " + postId.ToString() +
                    "AND UserId = " + this.User.FindFirst("userId")?.Value;

            if (_dapper.ExecuteSql(deletePostSql))
            {
                return Ok();
            }

            throw new Exception("Failed to delete post");
        }

        [HttpGet("Search/{searchText}")]
        public IEnumerable<Post> GetPostsBySearch(string searchText)
        {
            string postsBySearchSql = @"SELECT [PostId],
                    [UserId],
                    [PostTitle],
                    [PostContent],
                    [PostCreated],
                    [PostUpdated] 
                FROM TutorialAppSchema.Posts
                WHERE PostTitle LIKE '%" + searchText + @"%'
                    OR PostContent LIKE '%" + searchText + "%'"
                ;

            return _dapper.LoadData<Post>(postsBySearchSql);
        }
    }
}