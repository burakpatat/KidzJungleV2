using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Environments._Mutual.Data;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

using UnityEngine.Networking;
using System.Threading.Tasks;

using RenderHeads.Media.AVProVideo;
using RenderHeads.Media.AVProVideo.Demos;

public class TestObject : MonoBehaviour
{
    public List<Transform> ObjectTransform;
    public GameObject Banner;

    public TextMeshProUGUI clickText;

    public TMP_InputField InputMessage;
    public TMP_InputField InputUpdateMessage;
    int clickID;

    public RawImage dataImage;
    public MediaPlayer mediaPlayer;
    Texture2D _texture;
    public VCR vCR;

    public List<float> videoSecondTime;
    int timeListCount = 0;
    bool b_rePlay = false;
    void GetCloumn()
    {
        
        int cloumnCount = GetData._CloumnCount;
        var _datas = GetData.TESTCLASS.data;

        for (int i = 0; i < cloumnCount; i++)
        {
            ObjectTransform[i].gameObject.SetActive(true);
            var _intanceObj = Instantiate(Banner, ObjectTransform[i].position, Quaternion.identity);
            _intanceObj.transform.SetParent(ObjectTransform[i]);
            _intanceObj.transform.localPosition = Vector3.zero;
            _intanceObj.transform.localScale = Vector3.one;

            _intanceObj.transform.GetChild(0).GetComponent<Text>().text = _datas[i].id.ToString();
            _intanceObj.transform.GetComponent<Button>().onClick.AddListener(
                () => GETClick(int.Parse(_intanceObj.transform.GetChild(0).GetComponent<Text>().text)));
        }
    }
    public async void GETClick(int id)
    {
        var _datas = GetData.TESTCLASS.data;

        foreach (var row in _datas)
        {
            if (row.id == id)
            {
                
                clickID = row.id;

                //message
                clickText.text = row.message;

                //openVideo
                vCR._videoFiles.Add(GetData.GetMedia() + row.video);
                vCR.OnOpenVideoFile();
                

                //image
                _texture = await GetRemoteTexture(GetData.GetMedia() + row.image);
                dataImage.texture = _texture;
            }
        }
    }
    private void Start()
    {
        Invoke("GetCloumn", .5f);
    }
    public void SendDATAButton()
    {
        StartCoroutine(PostThis());
    }
    IEnumerator PostThis()
    {
        _Environments._Mutual.Data.State.InsertData _datas = new _Environments._Mutual.Data.State.InsertData();

        _datas.message = InputMessage.text;

        yield return GetData.PostDatas(_datas);
    }
    public void UpdateDATAButton()
    {
        StartCoroutine(UpdateThis());
    }
    IEnumerator UpdateThis()
    {
        _Environments._Mutual.Data.State.UpdateData _datas = new _Environments._Mutual.Data.State.UpdateData();

        _datas.id = clickID;
        _datas.message = InputUpdateMessage.text;

        yield return GetData.UpdateDatas(_datas, clickID.ToString());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(mediaPlayer.Info.GetVideoFrameRate());
        }

        float time = vCR.PlayingPlayer.Control.GetCurrentTimeMs();
        Debug.Log(time);

        if (time >= videoSecondTime[timeListCount])
        {
            vCR.OnPauseButton();
            timeListCount += 1;
            b_rePlay = true;
        }
        if (b_rePlay == true)
        {
            b_rePlay = false;
            StartCoroutine(RePlayVideo());
        }
    }
    public static async Task<Texture2D> GetRemoteTexture(string url)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            // begin request:
            var asyncOp = www.SendWebRequest();

            // await until it's done: 
            while (asyncOp.isDone == false)
                await Task.Delay(1000 / 30);//30 hertz

            // read results:
            if (www.isNetworkError || www.isHttpError)
            // if( www.result!=UnityWebRequest.Result.Success )// for Unity >= 2020.1
            {
                // log error:

                Debug.Log($"{www.error}, URL:{www.url}");

                // nothing to return on error:
                return null;
            }
            else
            {
                // return valid results:
                return DownloadHandlerTexture.GetContent(www);
            }
        }
    }
    IEnumerator RePlayVideo()
    {
        yield return new WaitForSeconds(10f);
        vCR.OnPlayButton();

    }
    void OnDestroy() => Dispose();
    public void Dispose() => Object.Destroy(_texture);
}
