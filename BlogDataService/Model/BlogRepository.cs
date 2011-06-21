using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MetaWeblogService.Model
{
    public class BlogRepository : MetaWeblogService.Model.IBlogRepository
    {
        private BlogDataDataContext db = new BlogDataDataContext();

        public BlogEntry GetPost(Int32 postid)
        {
            return db.BlogEntries.Where(e => e.BlogEntry_ID == postid).Single();
        }

        public IEnumerable<BlogCategory> FindAllBlogCategories(Int32 blogid)
        {
            return (from entries in db.BlogEntries
                    from catlink in db.BlogEntryCategories.Where(e => e.BlogEntry_ID == entries.BlogEntry_ID).DefaultIfEmpty()
                    from Category in db.BlogCategories.Where(c => c.BlogCategory_ID == catlink.BlogCategory_ID)
                    where entries.BlogFeed_ID == blogid
                    select Category);
        }

        public IQueryable<BlogEntry> GetRecentPosts(Int32 blogid, String username, String password, Int32 numberOfPosts)
        {
            return (from entry in db.BlogEntries 
                    where entry.BlogFeed_ID == blogid
                    orderby entry.BlogEntry_ID descending
                    select entry).Take(numberOfPosts);
        }

        public void DeletePost(Int32 postid)
        {
            BlogEntryCategory categories = db.BlogEntryCategories.Where(c => c.BlogEntry_ID == postid).SingleOrDefault();
            if (categories != null)
            {
                db.BlogEntryCategories.DeleteOnSubmit(categories);
            }           

            BlogEntryComment comments = db.BlogEntryComments.Where(c => c.BlogEntry_ID == postid).SingleOrDefault();
            if (comments != null)
            {
                db.BlogEntryComments.DeleteOnSubmit(comments);
            }            

            BlogEntry be = db.BlogEntries.Where(b => b.BlogEntry_ID == postid).Single();
            db.BlogEntries.DeleteOnSubmit(be);

            db.SubmitChanges();
        }

        public Int32 AddPost(Int32 blogid, String username, String password, String title, String content, String[] tags)
        {
            BlogEntry entry = new BlogEntry();
            entry.BlogFeed_ID = blogid;
            entry.Content = content;
            entry.Author = "Billy Coover";
            entry.Published = DateTime.Now;
            entry.Title = title;
            entry.Content = content;

            db.BlogEntries.InsertOnSubmit(entry);
            db.SubmitChanges();
            
            //BlogCategory cat;
            //foreach (var item in tags)
            //{
            //    cat = new BlogCategory { Category = item };
            //    db.BlogEntryCategories.InsertOnSubmit(
            //    db.SubmitChanges();
            //}

            return entry.BlogEntry_ID;
        }

        public void UpdatePost(Int32 postid, String title, String content, String[] tags)
        {
            BlogEntry entry = db.BlogEntries.Where(e => e.BlogEntry_ID == postid).Single();
            entry.Content = content;
            entry.Author = "Billy Coover";
            entry.Title = title;
            entry.Updated = DateTime.Now;
            db.SubmitChanges();

            //BlogCategory cat;
            //foreach (var item in tags)
            //{
            //    cat = new BlogCategory { Category = item };
            //    db.BlogEntryCategories.InsertOnSubmit(
            //    db.SubmitChanges();
            //}
        }

        public BlogUser UserProfile(String username, String password)
        {
            return db.BlogUsers.Where(u => u.UserName == username && u.Password == password).SingleOrDefault();
        }

        public IEnumerable<BlogFeed> FindAllUserFeeds(String username, String password)
        {
            return (from users in db.BlogUsers
                    join feedusers in db.BlogFeedUsers on users.BlogUser_ID equals feedusers.BlogUser_ID
                    join feeds in db.BlogFeeds on feedusers.BlogFeed_ID equals feeds.BlogFeed_ID
                    where users.UserName == username && users.Password == password
                    select feeds);
        }

        public Boolean ValidateUserFeed(Int32 blogid, String username, String password)
        {
            Boolean returnValue = false;
            var user = from bu in db.BlogUsers
                       join bfu in db.BlogFeedUsers on bu.BlogUser_ID equals bfu.BlogUser_ID
                       where bfu.BlogFeed_ID == blogid
                       select bu;
            if (user != null && (user.Count() > 0))
            {
                returnValue = true;
            }
            return returnValue;           
        }

        public Boolean ValidateUserEntry(Int32 postid, String username, String password)
        {
            Boolean returnValue = false;
            var user = from bu in db.BlogUsers
                       join bfu in db.BlogFeedUsers on bu.BlogUser_ID equals bfu.BlogUser_ID
                       join bf in db.BlogFeeds on bfu.BlogFeed_ID equals bf.BlogFeed_ID
                       join be in db.BlogEntries on bf.BlogFeed_ID equals be.BlogFeed_ID
                       where be.BlogEntry_ID == postid
                       select bu;
            if (user != null && (user.Count() > 0))
            {
                returnValue = true;
            }
            return returnValue;
        }

        public Boolean ValidateUser(String username, String password)
        {
            var user = db.BlogUsers.Where(u => u.UserName == username && u.Password == password).SingleOrDefault();
            if (user != null)
            {
                return true;
            }
            return false;
        }
    }
}