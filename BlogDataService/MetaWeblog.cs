using System;
using System.Collections.Generic;
using CookComputing.XmlRpc;
using MetaWeblogService.Model;
using System.Linq;

namespace MetaWeblogService
{
    public class MetaWeblog : XmlRpcService, IMetaWeblog
    {
        Model.IBlogRepository repository;

        public MetaWeblog()
            : this(new Model.BlogRepository()) {
        }

        public MetaWeblog(Model.IBlogRepository Repository) {
            repository = Repository;
        }
        
        string IMetaWeblog.AddPost(String blogid, String username, String password, Post post, Boolean publish)
        {
            if (ValidateUserFromFeed(blogid, username, password))
            {
                return repository.AddPost(Convert.ToInt32(blogid), username, password, post.title, post.description, post.categories).ToString();
            }
            throw new XmlRpcFaultException(0, "User is not valid!");
        }

        bool IMetaWeblog.UpdatePost(String postid, String username, String password, Post post, Boolean publish)
        {
            if (ValidateUserFromPost(postid, username, password))
            {
                //Boolean result = false;
                repository.UpdatePost(Convert.ToInt32(postid), post.title, post.description, post.categories);
                return true;
            }
            throw new XmlRpcFaultException(0, "User is not valid!");
        }

        Post IMetaWeblog.GetPost(String postid, String username, String password)
        {
            if (ValidateUserFromPost(postid, username, password)) {
                BlogEntry blogEntry = repository.GetPost(Convert.ToInt32(postid));
                Post post = new Post {
                    postid = blogEntry.BlogEntry_ID.ToString(),
                    dateCreated = blogEntry.Published,
                    description = blogEntry.Content,
                    permalink = blogEntry.URL,
                    title = blogEntry.Title,
                    userid = blogEntry.Author
                };
                return post;
            }
            throw new XmlRpcFaultException(0, "User is not valid!");
        }

        Category[] IMetaWeblog.GetCategories(String blogid, String username, String password)
        {
            if (ValidateUserFromFeed(blogid, username, password))
            {
                List<Category> categoryInfos = new List<Category>();
                var tags = repository.FindAllBlogCategories(Convert.ToInt32(blogid));
                Category info;
                foreach (var item in tags)
                {
                    info = new Category {
                        description = item.Category,
                        title = String.Empty
                    };
                    categoryInfos.Add(info);
                }
                return categoryInfos.ToArray();
            }
            throw new XmlRpcFaultException(0, "User is not valid!");
        }

        Post[] IMetaWeblog.GetRecentPosts(String blogid, String username, String password, Int32 numberOfPosts)
        {
            if (ValidateUserFromFeed(blogid, username, password))
            {
                List<Post> posts = new List<Post>();
                var entries = repository.GetRecentPosts(Convert.ToInt32(blogid), username, password, numberOfPosts);
                Post post;
                foreach (var item in entries)
                {
                    post = new Post {
                        postid = item.BlogEntry_ID.ToString(),
                        dateCreated = item.Published,
                        description = item.Content,
                        permalink = item.URL,
                        title = item.Title,
                        userid = item.Author
                    };
                    posts.Add(post);
                }
                return posts.ToArray();
            }
            throw new XmlRpcFaultException(0, "User is not valid!");
        }

        MediaObjectInfo IMetaWeblog.NewMediaObject(String blogid, String username, String password, MediaObject mediaObject)
        {
            if (ValidateUserFromFeed(blogid, username, password))
            {
                MediaObjectInfo objectInfo = new MediaObjectInfo();
                // TODO: Implement your own logic to add media object and set the objectInfo
                return objectInfo;
            }
            throw new XmlRpcFaultException(0, "User is not valid!");
        }

        bool IMetaWeblog.DeletePost(String key, String postid, String username, String password, Boolean publish)
        {
            if (ValidateUserFromPost(postid, username, password))
            {
                repository.DeletePost(Convert.ToInt32(postid));
                return true;
            }
            throw new XmlRpcFaultException(0, "User is not valid!");
        }

        BlogInfo[] IMetaWeblog.GetUsersBlogs(String key, String username, String password)
        {
            if (ValidateUser(username, password))
            {
                List<BlogInfo> infoList = new List<BlogInfo>();
                var feeds = repository.FindAllUserFeeds(username, password);
                if (feeds == null)
                {
                    throw new XmlRpcFaultException(0, "User does not have any blogs!");
                }
                BlogInfo info = new BlogInfo();
                foreach (var item in feeds)
                {
                    info = new BlogInfo { blogid = item.BlogFeed_ID.ToString(), blogName = item.Title, url = item.URL };
                    infoList.Add(info);
                }
                return infoList.ToArray();
            }            
            throw new XmlRpcFaultException(0, "User is not valid!");
        }

        UserInfo IMetaWeblog.GetUserInfo(String key, String username, String password)
        {
            UserInfo info = new UserInfo();
            var user = repository.UserProfile(username, password);
            if (user == null)
            {
                throw new XmlRpcFaultException(0, "User is not valid!");
            }
            info.email = user.Email;
            info.firstname = user.FirstName;
            info.lastname = user.LastName;
            info.nickname = user.NickName;
            info.url = user.Url;
            info.userid = user.UserName;
            return info;            
        }

        private Boolean ValidateUser(String username, String password)
        {
            return repository.ValidateUser(username, password);
        }

        private Boolean ValidateUserFromFeed(String blogid, String username, String password)
        {
            return repository.ValidateUserFeed(Convert.ToInt32(blogid), username, password);
        }

        private Boolean ValidateUserFromPost(String postid, String username, String password)
        {
            return repository.ValidateUserEntry(Convert.ToInt32(postid), username, password);
        }
    }
}
