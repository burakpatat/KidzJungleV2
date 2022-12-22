using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using System;
using System.Linq;
using _Environments._Mutual;
using _Environments._Mutual.Data;
using _Environments._Mutual.Data.State;

namespace _Environments._Mutual.Connection
{
    public class ConnectionManager : MonoBehaviour
    {
        #region Singleton

        public static ConnectionManager Instance;
        void Singleton()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        #endregion

        [SerializeField]
        private string _BaseUrl = "";

        [SerializeField]
        private string _ActiveToken;

        [SerializeField]
        private bool _PremiumStatus;

        [SerializeField]
        private string _Language;

        [Header("Auth Info")]
        [SerializeField]
        private string _AuthID;
        [SerializeField]
        private string _AuthName;
        [SerializeField]
        private string _ParentName;
        [SerializeField]
        private List<string> _ChildsName;

        [HideInInspector] public List<Profile_Data> _profiledatas = new List<Profile_Data>();

        public string BaseUrl { get => _BaseUrl; set => _BaseUrl = value; }
        public string ActiveToken { get => _ActiveToken; set => _ActiveToken = value; }
        public bool PremiumStatus { get => _PremiumStatus; set => _PremiumStatus = value; }
        public string Language { get => _Language; set => _Language = value; }

        public string AuthID { get => _AuthID; set => _AuthID = value; }
        public string AuthName { get => _AuthName; set => _AuthName = value; }
        public string ParentName { get => _ParentName; set => _ParentName = value; }
        public List<string> ChildsName { get => _ChildsName; set => _ChildsName = value; }
        public void setTokenAndBaseUrl()
        {
            String mainUrl = String.Empty;
            String token = String.Empty;
            String premiumStatus = String.Empty;
            String language = String.Empty;
#if TEST
            mainUrl = "https://kidzjungle.directus.app";
            token = "CrxfNKnyvKrYkFTo1vrYIZMcL81VCnAI";
            premiumStatus = "0";
            language = "tr-TR";
#elif PROD
            mainUrl = "https://api.kidzjungle.com/kidsvid";
            token = "01fcd8d0-ae90-4740-9928-59051e9ec067";
            premiumStatus = "0";
            language = "tr-TR";
#endif
#if UNITY_EDITOR
            Debug.Log("EDITOR VERSION STARTED");
            Debug.Log("Caching size : " + Caching.cacheCount + " - " + Caching.ready);
            AssetBundle.UnloadAllAssetBundles(false);
            bool cleared = Caching.ClearCache();
            Debug.Log("Cache cleared : " + cleared);
            BaseUrl = mainUrl;
            ActiveToken = token;
            if (premiumStatus.Equals(0))
            {
                PremiumStatus = false;
            }
            Language = language;
#elif UNITY_ANDROID
            Debug.Log("Android PLATFORM VERSION STARTED");
            Debug.Log("Caching size : " + Caching.cacheCount + " - " + Caching.ready);
            AssetBundle.UnloadAllAssetBundles(false);
            bool cleared = Caching.ClearCache();
            Debug.Log("Cache cleared : " + cleared);
            BaseUrl = mainUrl;
            ActiveToken = token;
            if (premiumStatus.Equals(0))
            {
                PremiumStatus = false;
            }
            Language = language;

#elif STANDALONE
            Debug.Log("STANDALONE VERSION STARTED");
            Debug.Log("Caching size : "+Caching.cacheCount+" - "+Caching.ready);
            AssetBundle.UnloadAllAssetBundles(false);
            bool cleared = Caching.ClearCache();
            Debug.Log("Cache cleared : "+cleared);
            SetConnectionStats(mainUrl + token + premiumStatus + language);
#endif
        }
        private void Awake()
        {
            Singleton();

            StartCoroutine(WebServices());
        }
        IEnumerator WebServices()
        {
            setTokenAndBaseUrl();

            yield return authInfo();

            yield return GetGame.GetGameDatas();
            yield return GetInteractiveVideo.GetIVDatas();
            

            Debug.Log("Base Loaded");
        }
        private void LogMessage(string message)
        {
            Debug.Log(message + "\n");
        }

        IEnumerator authInfo()
        {
            yield return GetProfile.GetProfileDatas();

            _profiledatas = GetProfile.ProfileClass.data;
            foreach (var row in _profiledatas)
            {
                if (Profile.Instance.guestProfileRegister == false)
                {
                    if(row.name == "Guest")
                    {
                        AuthID = row.KJId;
                        AuthName = row.name;
                        ParentName = row.name;

                        for (int i = 0; i < row.Child.Count; i++)
                        {
                            ChildsName.Add(row.Child[i].childname);
                        }
                    }
                }
            }
        }
    }
}


