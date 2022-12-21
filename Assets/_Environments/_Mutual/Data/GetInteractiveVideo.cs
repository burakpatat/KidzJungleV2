using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using Proyecto26;
using _Environments._Mutual.Connection;
using _Environments._Mutual.Data.State;

namespace _Environments._Mutual.Data
{
    public class GetInteractiveVideo : AbstractGetData
    {
        public static int _PositionCloumnCount = 0;
        public static InteractiveVideoClass InteractiveVideoClass;
        public static IEnumerator GetIVDatas()
        {
            string mainUrl = ConnectionManager.Instance.BaseUrl + Get_SubUrl(GetTarget.BASE, new string[1], "/items/InteractiveVideo?fields=*,questions.*,questions.pos.*");
            yield return GetResultResponse(GetTarget.BASE, new string[1], mainUrl);
            yield return new WaitUntil(() => _GETResponseResult != "");

            try
            {
                InteractiveVideoClass _datas = new InteractiveVideoClass();
                _datas = JsonUtility.FromJson<InteractiveVideoClass>(_GETResponseResult);
                InteractiveVideoClass = _datas;
            }
            catch (System.Exception ex)
            {
                Debug.Log("exception:" + ex.Message + ex.InnerException?.Message);
            }
        }
        public static string GetMedia()
        {
            return ConnectionManager.Instance.BaseUrl + Get_SubUrl(GetTarget.FILE, new string[1], "");
        }
    }
}
