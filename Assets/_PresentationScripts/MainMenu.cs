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

    [Header("Register")]
    public Transform RegisterTransform;

    public TMP_InputField UsernameIF;
    public TMP_InputField MailIF;
    public TMP_InputField PasswordIF;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        starcount = PlayerPrefs.GetInt("starVal");
        diacount = PlayerPrefs.GetInt("diaVal");
        ScoreText.text = "star : " + starcount.ToString() + "\n" + "diamond : " + diacount.ToString();
    }
    void guestQuery()
    {
        if (ConnectionManager.Instance.BaseLoadedOK && ConnectionManager.Instance.AuthName == "Guest")
        {
            ConnectionManager.Instance.BaseLoadedOK = false;

            UUIDText.text = Profile.Instance.guestUUID;
            usernameText.text = "childName : " + ConnectionManager.Instance.ChildsName[0] + "\n" + "parentName : " + ConnectionManager.Instance.AuthName;

            DataLoading.Instance.HideLoading();

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
        DataLoading.Instance.ReOp();
        StartCoroutine(Load(3));
    }
    public void PlayButton()
    {
        SceneManager.LoadScene(4);
        DataLoading.Instance.loadingScreenManager.RevealLoadingScreen();
    }
    public void RegisterPanelButton()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        RegisterTransform.gameObject.SetActive(true);
    }
    public void RegisterButton()
    {

    }
    public void RegisterBackButton()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        RegisterTransform.gameObject.SetActive(false);
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
