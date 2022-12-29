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

public class MagaraList : MonoBehaviour
{
    #region Singleton

    public static MagaraList Instance;
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
    private string _cachePath;
    private void Awake()
    {
        Singleton();
        _cachePath = Application.persistentDataPath + "/Temp/Covers";
    }

    public GameObject PosterButton;
    public List<Transform> PosterTransform;
    Texture2D _texture;
    public Transform Posters;

    List<GData> _datas = new List<GData>();
    public int ClickPosterID;
    void Start()
    {
        Invoke("InitData", 3f);
    }

    void InitData()
    {
        _datas = GetGame.GameClass.data;
        GetList();
    }
    void GetList()
    {
        int cloumnCount = GetGame._CloumnCount;
        for (int i = 0; i < cloumnCount; i++)
        {
            var _intanceObj = Instantiate(PosterButton, PosterTransform[i].transform.position, Quaternion.identity);
            _intanceObj.transform.SetParent(Posters);

            _intanceObj.transform.localScale = Vector3.one;

            //image
            StartCoroutine(SetPoster(GetGame.GetMedia() + _datas[i].icon, _datas[i].name, _intanceObj.transform.GetChild(0)));
            _intanceObj.transform.GetChild(1).gameObject.SetActive(true);

            foreach (var prow in ConnectionManager.Instance._profiledatas)
            {
                for (int p = 0; p < prow.Child.Count; p++)
                {
                    Debug.Log(p);
                    if (prow.Child[p].childname == ConnectionManager.Instance.ChildsName[0])
                    {
                        if (prow.Child[p].content_limitation[0].Games != null)
                        {
                            for (int a = 0; a < prow.Child[p].content_limitation[0].Games.Count; a++)
                            {
                                if (prow.Child[p].content_limitation[0].Games[a].Games_id.name.Contains(_datas[i].name))
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
            _intanceObj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = _datas[i].id.ToString();
        }
    }
    public void GETClick(int id)
    {
        ClickPosterID = id;
        SceneManager.LoadScene(5);
    }
    public IEnumerator SetPoster(string url, string name, Transform _intanceObj)
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
