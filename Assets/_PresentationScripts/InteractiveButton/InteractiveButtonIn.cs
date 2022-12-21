using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveButtonIn : MonoBehaviour
{

    [SerializeField] private bool _buttonState;
    public bool ButtonState { get { return _buttonState; }set { _buttonState = value; } }

}

