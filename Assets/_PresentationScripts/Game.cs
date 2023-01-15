using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Environments._Mutual.Data;
using _Environments._Mutual.Data.State;
using _Environments._Mutual.Connection;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

using UnityEngine.Networking;
using System.Threading.Tasks;
using System.IO;
using System;
using System.Linq;

public class Game : MonoBehaviour
{
    private void Awake()
    {
        _cachePath = Application.persistentDataPath + "/Temp/Covers";
    }
    public GameObject PosterButton;
    Texture2D _texture;

    List<GData> _datas = new List<GData>();
    public Image BundleProgressbar;
    public float DownloadAssetDataProgress;

    private string _cachePath;

    public TMP_InputField LoginInput;
    bool bnull = false;
    int firstcount = 0;
    int datacount = 0;

    public TMP_Text UserNameText;
    public Transform Posters;
    public Transform MainPanel;
    public Transform MenuPanel;

    public Transform PassPanel;
    public TMP_Text PassPanelText;
    public Button PassButton;
    public Button PassCloseButton;

    [SerializeField] int _ContentLimitationIDforhiddenGamesTable;
    [SerializeField] int _GamesIDforhiddenGamesTable;
    bool _passed, _notpassed = false;
    private void Start()
    {
        //Invoke("GetCloumn", 1f);
    }
    void GetCloumn()
    {
        _datas = GetGame.GameClass.data;

        foreach (var row in _datas)
        {
            var _intanceObj = Instantiate(PosterButton, Vector3.zero, Quaternion.identity);
            _intanceObj.transform.SetParent(Posters);
            
            _intanceObj.transform.localScale = Vector3.one;
           
            if (datacount > firstcount)
            {
                _intanceObj.transform.localPosition = new Vector3(transform.localPosition.x + 550f, transform.localPosition.y, transform.localPosition.z);
                //firstcount += 1;
            }
            else if(datacount <= firstcount)
            {
                _intanceObj.transform.localPosition = Vector3.zero;
                datacount += 1;
            }

            //image
            StartCoroutine(SetPoster(GetGame.GetMedia() + row.icon, row.name, _intanceObj.transform.GetChild(0)));
            _intanceObj.transform.GetChild(1).gameObject.SetActive(true);
            //_intanceObj.GetComponent<Button>().interactable = false;

            foreach (var prow in ConnectionManager.Instance._profiledatas)
            {
                for (int i = 0; i < prow.Child.Count; i++)
                {
                    if (prow.Child[i].childname == LoginInput.text)
                    {
                        if (prow.Child[i].content_limitation[0].Games != null)
                        {
                            for (int a = 0; a < prow.Child[i].content_limitation[0].Games.Count; a++)
                            {
                                if (prow.Child[i].content_limitation[0].Games[a].Games_id.name.Contains(row.name))
                                {
                                    _intanceObj.transform.GetChild(1).gameObject.SetActive(false);
                                    _intanceObj.GetComponent<Button>().interactable = true;

                                    _intanceObj.transform.GetComponent<Button>().onClick.AddListener(() => GETClick(
                                    int.Parse(_intanceObj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text)));

                                    
                                }
                            }
                        }
                    }
                }
            }
            _intanceObj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = row.id.ToString();
            _intanceObj.transform.GetComponent<Button>().onClick.AddListener(() => PassClick(
            int.Parse(_intanceObj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text), row.name, true));
        }

        
    }
    public void OnGameLoadProgress(int progress)
    {
        Debug.Log("MtekGames " + progress);

        BundleProgressbar.fillAmount = progress / 100.0f;
    }
    public void PassClick(int id, string gameName, bool _passsit)
    {
        Color p, np;
        ColorUtility.TryParseHtmlString("#44FF00", out p);
        ColorUtility.TryParseHtmlString("#FF002B", out np);

        PassPanel.gameObject.SetActive(true);
        if (_passsit)
        {
            PassPanel.transform.GetChild(0).GetComponent<Image>().color = p;
            PassPanelText.text = gameName + " oyunu için izin verilsin mi?".ToLower();
        }
        else
        {
            PassPanel.transform.GetChild(0).GetComponent<Image>().color = np;
            PassPanelText.text = gameName + " oyunu için izin kaldýrýlsýn mý?".ToLower();
        }

        PassButton.onClick.AddListener(() => ControlGame(id, LoginInput.text));
        PassCloseButton.onClick.AddListener(() => PassPanelCloseButtonClick());
    }
    void PassPanelCloseButtonClick()
    {
        PassPanel.gameObject.SetActive(false);
    }
    public void ControlGame(int gameId, string childName)
    {
        foreach (var row in _datas)
        {
            foreach (var prow in ConnectionManager.Instance._profiledatas)
            {
                for (int i = 0; i < prow.Child.Count; i++)
                {
                    if (prow.Child[i].childname == LoginInput.text)
                    {
                        _ContentLimitationIDforhiddenGamesTable = prow.Child[i].content_limitation[0].id;

                        for (int a = 0; a < prow.Child[i].content_limitation[0].Games.Count; a++)
                        {
                            if (prow.Child[i].content_limitation[0].Games != null)
                            {
                                if (prow.Child[i].content_limitation[0].Games[a].Games_id.id == gameId && _notpassed == false)
                                {
                                    _notpassed = true;
                                    _GamesIDforhiddenGamesTable = prow.Child[i].content_limitation[0].Games[a].id;
                                    StartCoroutine(DeleteContentLimitationGames(_GamesIDforhiddenGamesTable));
                                }
                            }
                        }
                    }
                }
            }
            if (row.id == gameId && _passed == false && _notpassed == false)
            {
                _passed = true;
                StartCoroutine(PostContentLimitationGames(_ContentLimitationIDforhiddenGamesTable, row));
            }

        }

    }
    IEnumerator PostContentLimitationGames(int contentId, GData game)
    {
        AllStateCRUDModel.Post_Profile_Game _datas = new AllStateCRUDModel.Post_Profile_Game();

        _datas.Content_Limitation_id = contentId;
        _datas.Games_id = game;

        yield return GetProfile.PostProfileDatas(_datas);
    }
    IEnumerator DeleteContentLimitationGames(int gamesId)
    {
        AllStateCRUDModel.Delete_Profile_Game _datas = new AllStateCRUDModel.Delete_Profile_Game();
        _datas.id = gamesId;
        yield return GetProfile.DeleteProfileDatas(_datas, gamesId.ToString());
    }
    public void GETClick(int id)
    {
        foreach (var row in _datas)
        {
            if (row.id == id)
            {
                foreach (var prow in ConnectionManager.Instance._profiledatas)
                {
                    for (int i = 0; i < prow.Child.Count; i++)
                    {
                        if (prow.Child[i].childname == LoginInput.text)
                        {
                            if (prow.Child[i].content_limitation[0].Games != null)
                            {
                                for (int a = 0; a < prow.Child[i].content_limitation[0].Games.Count; a++)
                                {
                                    if (prow.Child[i].content_limitation[0].Games[a].Games_id.name.Contains(row.name))
                                    {
                                        StartCoroutine(MtekAssetBundle.DownloadAssetBundle(GetGame.GetMedia() + row.MainBundle[0].files[0].directus_files_id, row.name, OnGameLoadProgress, row.MainBundle[0].scenename));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    public void LoginClick()
    {
        GetCloumn();

        MenuPanel.gameObject.SetActive(false);
        MainPanel.gameObject.SetActive(true);
        UserNameText.text = LoginInput.text;
    }
    public IEnumerator SetPoster(string url, string name,Transform _intanceObj)
    {
        if (!Directory.Exists(_cachePath))
        {
            Debug.Log("No directory found for temporary files. Creating one.");
            Directory.CreateDirectory(_cachePath);
        }

        string posterPath = _cachePath + "/" + name + ".poster";
        bool valid = true;
        _texture = new Texture2D(1080, 1920);

        if (System.IO.File.Exists(posterPath))
        {
            Debug.Log("Game visual exist :  " + posterPath);
            System.TimeSpan since = System.DateTime.Now.Subtract(System.IO.File.GetLastWriteTime(posterPath));
            if (since.Days >= 1)
            {
                valid = false;
                Debug.Log("But visual is old :  " + since.Days + " days");
            }
        }
        else
        {
            Debug.Log("Game visual does not exist :  " + posterPath);
            valid = false;
        }

        if (!valid)
        {
            Debug.Log("Renewing cached visual for game " + name);
            UnityWebRequest trq = UnityWebRequest.Get(url);
            yield return trq.SendWebRequest();
            if (trq.result == UnityWebRequest.Result.Success)
            {
                byte[] result = trq.downloadHandler.data;
                System.IO.File.WriteAllBytes(posterPath, result);
                valid = true;
            }

        }

        if (valid)
        {
            _texture.LoadImage(System.IO.File.ReadAllBytes(posterPath));

            Rect rec = new Rect(0, 0, _texture.width, _texture.height);
            _intanceObj.transform.GetComponent<Image>().sprite = Sprite.Create(_texture, rec, new Vector2(0, 0), 1);

        }


        yield return null;
    }

    void OnDestroy() => Dispose();
    public void Dispose() => UnityEngine.Object.Destroy(_texture);    
}
