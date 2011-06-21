using System;
using System.Collections.Generic;

namespace MetaWeblogService.Model
{
    public interface IBlogRepository
    {
        Int32 AddPost(int blogid, string username, string password, string title, string content, string[] tags);
        void DeletePost(int postid);
        System.Collections.Generic.IEnumerable<BlogCategory> FindAllBlogCategories(Int32 blogid);
        BlogEntry GetPost(int postid);
        System.Linq.IQueryable<BlogEntry> GetRecentPosts(Int32 blogid, string username, string password, int numberOfPosts);
        void UpdatePost(Int32 postid, String title, String content, String[] tags);
        Boolean ValidateUserFeed(Int32 blogid, String username, String password);
        Boolean ValidateUserEntry(Int32 postid, String username, String password);
        BlogUser UserProfile(String username, String password);
        IEnumerable<BlogFeed> FindAllUserFeeds(String username, String password);
        Boolean ValidateUser(String username, String password);
    }
}
