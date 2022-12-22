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
    private void Awake()
    {
        Singleton();
    }
    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        GuestProfileSave.LoadFile();

        if (GuestProfileSave.FILEFOUND)
        {
            guestProfileRegister = false;
            guestUUID = GuestProfileSave.GUESTUUID;
            SceneManager.LoadScene(1);
        }
    }
    public void ContinueButton()
    {
        GuestProfileSave.SaveFile();

        guestProfileRegister = false;
        SceneManager.LoadScene(1);
    }
}
