using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

using _Environments._Mutual.Connection;
using _Environments._Mutual.Data;
using _Environments._Mutual.Data.State;
using System.Threading.Tasks;

using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    #region Singleton

    public static MainMenu Instance;
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
    public TMP_Text UUIDText;

    public TMP_Text usernameText;
    int starcount, diacount;

    public TMP_Text ScoreText;

    public LoadingScreenManager _loadingScreenManager;

    public Transform fakeLoadImage;
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        starcount = PlayerPrefs.GetInt("starVal");
        diacount = PlayerPrefs.GetInt("diaVal");
        ScoreText.text = "star : " + starcount.ToString() + "\n" + "diamond : " + diacount.ToString();

        fakeLoadImage.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, 180), .6f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
    }
    void guestQuery()
    {
        if (ConnectionManager.Instance.BaseLoadedOK && ConnectionManager.Instance.AuthName == "Guest")
        {
            ConnectionManager.Instance.BaseLoadedOK = false;

            UUIDText.text = Profile.Instance.guestUUID;
            usernameText.text = "childName : " + ConnectionManager.Instance.ChildsName[0] + "\n" + "parentName : " + ConnectionManager.Instance.AuthName;

            _loadingScreenManager.HideLoadingScreen();
            fakeLoadImage.GetComponent<RectTransform>().DOScale(Vector3.zero, .6f).SetEase(Ease.Linear);

        }
    }
    private void Awake()
    {
        Singleton();
    }
    void Update()
    {
        guestQuery();
    }
    public void StarButton()
    {
        starcount += 10;
        ScoreText.text = "star : " + starcount.ToString() + "\n" + "diamond : " + diacount.ToString();

        PlayerPrefs.SetInt("starVal", starcount);
    }
    public void DiaButton()
    {
        diacount += 1;
        ScoreText.text = "star : " + starcount.ToString() + "\n" + "diamond : " + diacount.ToString();

        PlayerPrefs.SetInt("diaVal", diacount);
    }
    public void WatchButton()
    {
        StartCoroutine(Load(2));
    }
    public void PlayButton()
    {
        SceneManager.LoadScene(3);
    }
    IEnumerator Load(int index)
    {
        Application.backgroundLoadingPriority = ThreadPriority.BelowNormal;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            int p = (int)(asyncOperation.progress * 100f);
            Debug.Log(p.ToString() + "%");
            
            if(p >= 89)
            {
                p = 100;
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
        
    }
}
