using System.Collections.Generic;
using UnityEngine;
using MEC;
using UnityEngine.UI;

namespace NORK
{
    public class Manager : MonoBehaviour
    {
        public static Manager instance;
        public Text txt_Version;

        private void Awake()
        {
            if (!instance) instance = this;
            else Destroy(gameObject);

            DontDestroyOnLoad(gameObject);

            //page_Start.gameObject.SetActive(true);

            Timing.RunCoroutine(Init());

            txt_Version.text = $"v{Application.version}";
        }

        /// <summary>
        /// 초기화
        /// </summary>
        private IEnumerator<float> Init()
        {
           yield return Timing.WaitUntilDone(manager_Setting.Start_Setting());

#if !UNITY_EDITOR
            manager_Common.Check_Version(() =>
            {
                page_Start.gameObject.SetActive(true);
            });
#else
            page_Start.gameObject.SetActive(true);
#endif
        }

        [Header("매니저 스크립트")]
        public Manager_Member manager_Member;
        public Manager_Language manager_Language;
        public Manager_UI manager_Ui;
        public Manager_Common manager_Common;
        public Manager_Webrequest manager_Web;
        public Manager_Data manager_Data;
        public Manager_Gallery manager_Gallery;
        public Manager_Firebase manager_Firebase;
        public Manager_Email manager_Email;
        public Manager_Setting manager_Setting;

        [Header("페이지 스크립트")]
        public Page_Start page_Start;
        public Page_Set_Profile page_Set_Profile;
        public Page_Calender page_Calender;
        public Page_Todo page_Todo;
        public Page_Record page_Record;
        public Page_Setting page_Setting;

        //[Header("각종 스크립트")]

        [Header("현재 색상")]
        [Tooltip("1")] public Color col_Main;
        [Tooltip("2")] public Color col_Main_Light;
        [Tooltip("3")] public Color col_Main_Light_Sub;
        [Tooltip("4")] public Color col_F7F7F7;
        [Tooltip("5")] public Color col_F5F5F5;
        [Tooltip("6")] public Color col_C7C7C7;
        [Tooltip("7")] public Color col_707070;
        [Tooltip("8")] public Color col_000000;

        [System.Serializable]
        public struct Data_Color
        {
            public Color col_Main;
            public Color col_Main_Light;
            public Color col_Main_Light_Sub;
        }

        [Header("색상 리스트")]
        public List<Data_Color> data_Colors;

        [Header("폰트")]
        public Font font_Bold;
        public Font font_Regular;
        public Font font_Midium;
        public Font font_Light;
        public Font font_ExtraLight;
    }
}