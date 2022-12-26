using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using Proyecto26;
using _Environments._Mutual.Connection;
using _Environments._Mutual.Data.State;

namespace _Environments._Mutual.Data
{
    public class GetUser : AbstractGetData
    {
        public static UserClass UserClass;
        public static IEnumerator GetUserDatas()
        {
            string mainUrl = ConnectionManager.Instance.BaseUrl + Get_SubUrl(GetTarget.BASE, new string[1], "/users?fields=*,profile.*,*,profile.Child.*," +
                "*profile.Child.content_limitation.Videos.InteractiveVideo_id.*,*,profile.Child.content_limitation.Games.Games_id.*");

            yield return GetResultResponse(GetTarget.BASE, new string[1], mainUrl);
            yield return new WaitUntil(() => _GETResponseResult != "");

            try
            {
                UserClass _datas = new UserClass();
                _datas = JsonUtility.FromJson<UserClass>(_GETResponseResult);
                UserClass = _datas;
            }
            catch (System.Exception ex)
            {
                Debug.Log("exception:" + ex.Message + ex.InnerException?.Message);
            }
        }
        /*public static IEnumerator PostProfileDatas(Post_Profile_Game _datas)
        {
            string mainUrl = ConnectionManager.Instance.BaseUrl + Post_SubUrl(PostTarget.NEWBASE, new string[1], "/items/Content_Limitation_Games?fields=*,Games_id.*");
            yield return PostData(mainUrl, PostTarget.NEWBASE, _datas);
            yield return new WaitUntil(() => _POSTResponseResult != "");

        }
        public static IEnumerator DeleteProfileDatas(Delete_Profile_Game _datas, string DeleteID)
        {
            Debug.Log(Newtonsoft.Json.JsonConvert.SerializeObject(_datas));
            string mainUrl = ConnectionManager.Instance.BaseUrl + Delete_SubUrl(DeleteTarget.DELETEBASE, "/items/Content_Limitation_Games/" + DeleteID);
            yield return DeleteData(mainUrl, DeleteTarget.DELETEBASE, _datas);
        }*/
    }
}
