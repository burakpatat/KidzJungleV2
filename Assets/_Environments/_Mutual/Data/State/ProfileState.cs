using Newtonsoft.Json;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace _Environments._Mutual.Data.State
{
    [Serializable]
    public class Profile_Child
    {
        public int id;
        public string childname;
        public List<ContentLimitation> content_limitation;
    }
    [Serializable]
    public class ContentLimitation
    {
        public int id;
        public int contentId;
        public List<Profile_Video> Videos;
        public List<Profile_Game> Games;
    }
    [Serializable]
    public class Profile_Data
    {
        public int id;
        public string status;
        public string KJId;
        public string name;
        public string mail;
        public List<Profile_Child> Child;
    }
    [Serializable]
    public class Profile_Game
    {
        public int id;
        public Profile_GamesId Games_id;
    }
    [Serializable]
    public class Profile_GamesId
    {
        public int id;
        public string status;
        public string name;
    }
    [Serializable]
    public class Post_Profile_Game
    {
        public int Content_Limitation_id;
        public GData Games_id;
    }
    [Serializable]
    public class Delete_Profile_Game
    {
        public int id;
    }
    [Serializable]
    public class Profile_InteractiveVideoId
    {
        public int id;
        public string videoname;
    }
    [Serializable]
    public class Profile_Video
    {
        public Profile_InteractiveVideoId InteractiveVideo_id;
    }
    [Serializable]
    public class ProfileClass
    {
        public List<Profile_Data> data;
        public override string ToString()
        {
            return UnityEngine.JsonUtility.ToJson(this, true);
        }
    }
}
