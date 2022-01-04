using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using LitJson;

namespace NORK
{
    public class Manager_Member : MonoBehaviour
    {
        /// <summary>
        /// ȸ������
        /// </summary>
        [Serializable]
        public struct Data_Member
        {
            public string id;
#if USE_LOGIN
            public string email;
#else
            public string devicemodel;
            public string devicename;
            public string deviceuniqueIdentifier;
#endif
            public string nickname;
            public string nickname_code;
            public string character;
            public string promise;
            public string platform;
            public string token;
            public string special_code;

            public Data_Member(string _id, string _devicemodel, string _devicename, string _deviceuniqueIdentifier, string _nickname, string _nickname_code, string _character, string _promise, string _platform, string _token, string _special_code)
            {
                id = _id;
#if USE_LOGIN
                email = "";
#else
                devicemodel = _devicemodel;
                devicename = _devicename;
                deviceuniqueIdentifier = _deviceuniqueIdentifier;
#endif

                nickname = _nickname;
                nickname_code = _nickname_code;
                character = _character;
                promise = _promise;
                platform = _platform;
                token = _token;
                special_code = _special_code;
            }

            public Data_Member(string _id, string _email, string _nickname, string _nickname_code, string _character, string _promise, string _platform, string _token, string _special_code)
            {
                id = _id;
#if USE_LOGIN
                email = _email;
#else
                devicemodel = "";
                devicename = "";
                deviceuniqueIdentifier = "";
#endif

                nickname = _nickname;
                nickname_code = _nickname_code;
                character = _character;
                promise = _promise;
                platform = _platform;
                token = _token;
                special_code = _special_code;
            }
        }

        /// <summary>
        /// ���� ȸ������
        /// </summary>
        [SerializeField] private Data_Member data_Member;
        public Data_Member Get_Data_Member { get { return data_Member; } }

        /// <summary>
        /// ȸ������ ����
        /// </summary>
        /// <param name="_json">ȸ������ json������ string</param>
        public void Set_Data_Member(string _json)
        {
            data_Member = JsonUtility.FromJson<Data_Member>(_json);
            Set_Data_Member();
        }

        /// <summary>
        /// ȸ������ ����
        /// </summary>
        /// <param name="_json">ȸ������ json������ string</param>
        public void Set_Data_Member(Data_Member _member)
        {
            data_Member = _member;
            Set_Data_Member();
        }

        /// <summary>
        /// ȸ������ ����
        /// </summary>
        private void Set_Data_Member()
        {
            //Log_Login();
            Manager.instance.manager_Data.Todo_List();// �� �� ����Ʈ �ҷ�����
        }

        /// <summary>
        /// �α��� �α� �����
        /// </summary>
        public void Log_Login()
        {
            Timing.RunCoroutine(Manager.instance.manager_Web.Cor_Log_Login_Insert());
        }
    }
}