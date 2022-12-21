using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using Proyecto26;
using _Environments._Mutual.Connection;
using _Environments._Mutual.Data.State;
using UnityEngine.Networking;

namespace _Environments._Mutual.Data
{
    public class GetData : AbstractGetData
    {
        public static int _CloumnCount = 0;
        public static TESTData TESTCLASS;

        public static IEnumerator GetDatas()
        {
            string mainUrl = ConnectionManager.Instance.BaseUrl + Get_SubUrl(GetTarget.BASE, new string[1], "/items/UnityTest/");
            yield return GetResultResponse(GetTarget.BASE, new string[1], mainUrl);
            yield return new WaitUntil(() => _GETResponseResult != "");

            try
            {
                TESTData _datas = new TESTData();
                _datas = JsonUtility.FromJson<TESTData>(_GETResponseResult);
                foreach (var row in _datas.data)
                {
                    Debug.Log(row.id + " | " + row.message);
                }

                _CloumnCount = _datas.data.Count;
                TESTCLASS = _datas;
            }
            catch (System.Exception ex)
            {
                Debug.Log("exception bak:" + ex.Message + ex.InnerException?.Message);
            }
        }
        public static IEnumerator PostDatas(InsertData _datas)
        {
            string mainUrl = ConnectionManager.Instance.BaseUrl + Post_SubUrl(PostTarget.NEWBASE, new string[1], "/items/UnityTest/");
            yield return PostData(mainUrl, PostTarget.NEWBASE, _datas);
            yield return new WaitUntil(() => _POSTResponseResult != "");

        }
        public static IEnumerator UpdateDatas(UpdateData _datas, string UpdateID)
        {
            Debug.Log(Newtonsoft.Json.JsonConvert.SerializeObject(_datas));
            string mainUrl = ConnectionManager.Instance.BaseUrl + Update_SubUrl(UpdateTarget.UPDATEBASE, "/items/UnityTest/" + UpdateID );
            yield return UpdateData(mainUrl, UpdateTarget.UPDATEBASE, _datas);
        }

        public static string GetMedia()
        {
            return ConnectionManager.Instance.BaseUrl + Get_SubUrl(GetTarget.FILE, new string[1], "");
        }
    }

}
