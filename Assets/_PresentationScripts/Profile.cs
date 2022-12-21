using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using _Environments._Mutual.Connection;
using _Environments._Mutual.Data;
using _Environments._Mutual.Data.State;
using _Environments._Local;

public class Profile : MonoBehaviour
{
    private void Start()
    {
        GuestProfileSave.LoadFile();
    }
    public void ContinueButton()
    {
        GuestProfileSave.SaveFile();
    }
}
