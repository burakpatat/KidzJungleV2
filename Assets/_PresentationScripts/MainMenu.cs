using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

using _Environments._Mutual.Connection;
using _Environments._Mutual.Data;
using _Environments._Mutual.Data.State;
using System.Threading.Tasks;

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
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        starcount = PlayerPrefs.GetInt("starVal");
        diacount = PlayerPrefs.GetInt("diaVal");
        ScoreText.text = "star : " + starcount.ToString() + "\n" + "diamond : " + diacount.ToString();

        Invoke("guestQuery", 1f);
    }
    void guestQuery()
    {
        if (ConnectionManager.Instance.AuthName == "Guest")
        {
            UUIDText.text = Profile.Instance.guestUUID;
            usernameText.text = "childName : " + ConnectionManager.Instance.ChildsName[0] + "\n" + "parentName : " + ConnectionManager.Instance.AuthName;
        }
    }
    private void Awake()
    {
        Singleton();
    }
    void Update()
    {
        
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
        SceneManager.LoadScene(2);
    }
    public void PlayButton()
    {
        SceneManager.LoadScene(3);
    }
}
