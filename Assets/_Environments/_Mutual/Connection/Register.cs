using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using _Environments._Mutual;
using _Environments._Mutual.Data;
using _Environments._Mutual.Data.State;

namespace _Environments._Mutual.Connection
{
    public class Register : MonoBehaviour
    {
        public static IEnumerator GuestUserRegister()
        {
            User_Register _datas = new User_Register();
            _datas.email = "guest@mtekbilisim.com";
            _datas.password = "guest@mtek";

            yield return GetUser.Register(_datas);
        }
    }
}
