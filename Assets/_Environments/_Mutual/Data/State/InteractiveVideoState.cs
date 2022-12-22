using Newtonsoft.Json;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;

namespace _Environments._Mutual.Data.State
{
    [Serializable]
    public class IVData
    {
        public int id;
        public string videofile;
        public string videoname;
        public List<string> isInteractive;
        public string videoThumbnail;
        public List<Category> category;
        public List<Question> questions;
    }
    [Serializable]
    public class CategoriesId
    {
        public List<string> base_categories;
        public List<string> sub_categories;
        public string category_title;
    }
    [Serializable]
    public class Category
    {
        public CategoriesId Categories_id;
    }
    [Serializable]
    public class Pos
    {
        public int id;
        public string caseName;
        public string x;
        public string y;
        public string size;
        public bool isAnswer;
        public int vid;
    }
    [Serializable]
    public class Question
    {
        public int id;
        public string name;
        public string intime;
        public string outtime;
        public string satime;
        public string firstwrongtime;
        public string secondwrongtime;
        public string truetime;
        public int qid;
        public List<Pos> pos;
    }
    [Serializable]
    public class InteractiveVideoClass
    {
        public List<IVData> data;
        public override string ToString()
        {
            return UnityEngine.JsonUtility.ToJson(this, true);
        }
    }
}
