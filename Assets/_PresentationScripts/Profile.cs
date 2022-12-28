using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using _Environments._Mutual.Connection;
using _Environments._Mutual.Data;
using _Environments._Mutual.Data.State;
using _Environments._Local;

using UnityEngine.SceneManagement;

public class Profile : MonoBehaviour
{
    #region Singleton

    public static Profile Instance;
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

    public bool guestProfileRegister;
    public string guestUUID;

    public Transform FirstPanel;
    public Transform RegisterPanel;

    bool entry = false;
    private void Awake()
    {
        Singleton();
    }
    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        GuestProfileSave.LoadFile();
    }
    private void Update()
    {
        if (GuestProfileSave.FILEFOUND == true && entry == false)
        {
            entry = true;
            guestUUID = GuestProfileSave.GUESTUUID;

            //Guest Login
            StartCoroutine(Login.GuestUserLogin());

        }
        if (ConnectionManager.Instance.GuestLoginTokenCreated == true)
        {
            SceneManager.LoadScene(1);
        }
    }
    //splash
    public void ContinueButton()
    {
        GuestProfileSave.SaveFile();

        guestProfileRegister = false;
        SceneManager.LoadScene(1);
    }
    //splash
    public void RegisterButton()
    {
        FirstPanel.gameObject.SetActive(false);
        RegisterPanel.gameObject.SetActive(true);
    }
    public void BackButton()
    {
        RegisterPanel.gameObject.SetActive(false);
        FirstPanel.gameObject.SetActive(true);
    }
}
