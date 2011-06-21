using System;
using CookComputing.XmlRpc;

namespace MetaWeblogService
{
    public interface IMetaWeblog
    {
        [XmlRpcMethod("metaWeblog.newPost")]
        string AddPost(String blogid, String username, String password, Post post, Boolean publish);

        [XmlRpcMethod("metaWeblog.editPost")]
        bool UpdatePost(String postid, String username, String password, Post post, Boolean publish);

        [XmlRpcMethod("metaWeblog.getPost")]
        Post GetPost(String postid, String username, String password);

        [XmlRpcMethod("metaWeblog.getCategories")]
        Category[] GetCategories(String blogid, String username, String password);

        [XmlRpcMethod("metaWeblog.getRecentPosts")]
        Post[] GetRecentPosts(String blogid, String username, String password, Int32 numberOfPosts);

        [XmlRpcMethod("metaWeblog.newMediaObject")]
        MediaObjectInfo NewMediaObject(String blogid, String username, String password, MediaObject mediaObject);

        [XmlRpcMethod("blogger.deletePost")]
        [return: XmlRpcReturnValue(Description = "Returns true.")]
        bool DeletePost(String key, String postid, String username, String password, Boolean publish);

        [XmlRpcMethod("blogger.getUsersBlogs")]
        BlogInfo[] GetUsersBlogs(String key, String username, String password);

        [XmlRpcMethod("blogger.getUserInfo")]
        UserInfo GetUserInfo(String key, String username, String password);
    }
}
