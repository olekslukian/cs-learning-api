using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
    public interface IPostsController
    {
        public IEnumerable<Post> GetPosts();
        public Post GetPostSingle(int postId);
        public IEnumerable<Post> GetPostsByUser(int userId);
        public IEnumerable<Post> GetMyPosts();
        public IActionResult AddPost(PostToAddDTO post);
        public IActionResult EditPost(PostToEditDTO post);
        public IActionResult DeletePost(int postId);
        public IEnumerable<Post> GetPostsBySearch(string searchText);
    }
}