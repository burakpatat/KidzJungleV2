using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using _Environments._Mutual;
using _Environments._Mutual.Data;
using _Environments._Mutual.Data.State;
using System;

namespace _Environments._Mutual.Connection
{
    public class Login : MonoBehaviour
    {
        public static string LoginToken;

        public static bool TOKENCreated = false;

        public static bool FirstOpen = false;
        private void Awake()
        {
            TOKENCreated = false;
            FirstOpen = false;
        }
        public static IEnumerator GuestUserLogin()
        {
            User_Login _datas = new User_Login();

            _datas.email = "guest@mtekbilisim.com";
            _datas.password = "guest@mtek";

            yield return GetUser.Login(_datas);

            LoginToken = GetUser.UserTokenClass.data.access_token;

            ConnectionManager.Instance.ActiveToken = LoginToken;
            ConnectionManager.Instance.GuestLoginTokenCreated = true;

            TOKENCreated = true;

        }
    }
}
