using Newtonsoft.Json;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;

namespace _Environments._Mutual.Data.State
{
	[Serializable]
	public class TESTData
	{
		public List<MainData> data;
		public override string ToString()
		{
			return UnityEngine.JsonUtility.ToJson(this, true);
		}
	}
	[Serializable]
	public class MainData
	{
		public int id;
		public int sort;
		public string message;
		public string image;
		public string video;
	}
	[Serializable]
	public class InsertData
	{
		public string message;
	}
	[Serializable]
	public class UpdateData
	{
		public int id;
		public string message;
	}
}

