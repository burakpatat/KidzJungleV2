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
            User_Register _Userdatas = new User_Register();
            User_RegisterForPostProfile _Profiledatas = new User_RegisterForPostProfile();

            _Userdatas.email = "guest@mtekbilisim.com";
            _Userdatas.password = "guest@mtek";

            yield return GetUser.Register(_Userdatas, _Profiledatas);
        }
    }
}
