using System;
using CookComputing.XmlRpc;

namespace MetaWeblogService
{
    #region structs
    public struct BlogInfo
    {
        public String blogid;
        public String url;
        public String blogName;
    }

    public struct Category
    {
        //public String categoryId;
        //public String categoryName;
        public String description;
        public String title;
    }

    [Serializable]
    public struct CategoryInfo
    {
        public String description;
        public String htmlUrl;
        public String rssUrl;
        public String title;
        public String categoryid;
    }

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct Enclosure
    {
        public Int32 length;
        public String type;
        public String url;
    }

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct Post
    {
        public DateTime dateCreated;
        public String description;
        public String title;
        public String[] categories;
        public String permalink;
        public Object postid;
        public String userid;
        public String wp_slug;
    }

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct Source
    {
        public String name;
        public String url;
    }

    public struct UserInfo
    {
        public String userid;
        public String firstname;
        public String lastname;
        public String nickname;
        public String email;
        public String url;
    }

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct MediaObject
    {
        public String name;
        public String type;
        public Byte[] bits;
    }

    [Serializable]
    public struct MediaObjectInfo
    {
        public string url;
    }
    #endregion
}
