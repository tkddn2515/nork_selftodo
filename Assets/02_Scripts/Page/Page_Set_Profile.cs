using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using UnityEngine.UI;
using LitJson;
using System;

namespace NORK
{
    public class Page_Set_Profile : Page
    {
        internal override void OnEnable()
        {
            base.OnEnable();
        }

        internal override void Start()
        {
            base.Start();
            Init();
        }

        private void Update()
        {
            if(Manager_Common.isBack)
            {
                switch (page_Type)
                {
                    case Page_Type.Character:
                        Manager.instance.manager_Common.Active_Page(Manager.instance.page_Start);
                        break;
                    case Page_Type.Name:
                        OnClickUp_Btn(false);
                        break;
                    case Page_Type.Promise:
                        OnClickUp_Btn(false);
                        break;
                    case Page_Type.End:
                        OnClickButton_End_Back();
                        break;
                }
            }
        }

        private void Init()
        {
            Init_Common();
            Init_Character();
            Init_Name();
            Init_Promise();
        }

        #region 0. Subpage 공통
        public enum Page_Type { Character, Name, Promise, End }
        [Header("Subpage_공통")]
        [SerializeField] private Page_Type page_Type;
        [Tooltip("타이틀 Title_1")]
        [SerializeField] private CanvasGroup cvg_Title1;
        [Tooltip("타이틀 Title_2")]
        [SerializeField] private CanvasGroup cvg_Title2;
        [Tooltip("타이틀 Title_3")]
        [SerializeField] private CanvasGroup cvg_Title3;

        [Tooltip("버튼 화살표 - Next")]
        [SerializeField] private Image img_Btn_Arrow_Next;
        [Tooltip("버튼 화살표 - Prev")]
        [SerializeField] private Image img_Btn_Arrow_Prev;
        [Tooltip("버튼 그라데이션 - Next")]
        [SerializeField] private CanvasGroup cvg_Btn_Gradation_Next;
        [Tooltip("버튼 그라데이션 - Prev")]
        [SerializeField] private CanvasGroup cvg_Btn_Gradation_Prev;
        [Tooltip("이미지 그라데이션 - Next")]
        [SerializeField] private Image img_Btn_Gradation_Next;
        [Tooltip("이미지 그라데이션 - Prev")]
        [SerializeField] private Image img_Btn_Gradation_Prev;
        [Tooltip("버튼 Fill_2")]
        [SerializeField] private CanvasGroup cvg_Fill2;
        [Tooltip("버튼 Fill_3")]
        [SerializeField] private CanvasGroup cvg_Fill3;
        [Tooltip("버튼 Step_1")]
        [SerializeField] private CanvasGroup cvg_Step1;
        [Tooltip("버튼 Step_2")]
        [SerializeField] private CanvasGroup cvg_Step2;
        [Tooltip("버튼 Step_3")]
        [SerializeField] private CanvasGroup cvg_Step3;

        CoroutineHandle cor_Title1;
        CoroutineHandle cor_Title2;
        CoroutineHandle cor_Title3;
        CoroutineHandle cor_Btn_Gra_Next;
        CoroutineHandle cor_Btn_Arrow_Next;
        CoroutineHandle cor_Btn_Gra_Prev;
        CoroutineHandle cor_Btn_Arrow_Prev;
        CoroutineHandle cor_Fill2;
        CoroutineHandle cor_Fill3;
        CoroutineHandle cor_Step1;
        CoroutineHandle cor_Step2;
        CoroutineHandle cor_Step3;

        /// <summary>
        /// 공통 초기화
        /// </summary>
        private void Init_Common()
        {
            page_Type = Page_Type.Character;
            img_Btn_Arrow_Prev.gameObject.SetActive(false);
            img_Btn_Arrow_Next.color = Manager.instance.col_C7C7C7;
            cvg_Title1.alpha = cvg_Step1.alpha = 1;
            cvg_Title2.alpha = cvg_Title3.alpha = cvg_Step2.alpha = cvg_Step3.alpha = 0;
        }

        /// <summary>
        /// 이름 버튼 누를 시
        /// </summary>
        public void OnClickDown_Btn(bool _next)
        {
            switch (page_Type)
            {
                case Page_Type.Character:
                    OnClickDown_Btn(ref cor_Btn_Gra_Next, ref cor_Btn_Arrow_Next, cvg_Btn_Gradation_Next, img_Btn_Gradation_Next, img_Btn_Arrow_Next, false);
                    break;
                default:
                    if (_next)
                        OnClickDown_Btn(ref cor_Btn_Gra_Next, ref cor_Btn_Arrow_Next, cvg_Btn_Gradation_Next, img_Btn_Gradation_Next, img_Btn_Arrow_Next, page_Type.Equals(Page_Type.Promise) ? true : false);
                    else
                        OnClickDown_Btn(ref cor_Btn_Gra_Prev, ref cor_Btn_Arrow_Prev, cvg_Btn_Gradation_Prev, img_Btn_Gradation_Prev, img_Btn_Arrow_Prev, true);
                    break;
            }
            
            Manager.instance.manager_Common.PointDown();
        }

        /// <summary>
        /// 이름 버튼 누른 후 뗄 시
        /// </summary>
        public void OnClickUp_Btn(bool _next)
        {
            bool click = Manager.instance.manager_Common.PointUp_OneClick();
            MovePage(click, _next);
        }

        /// <summary>
        /// 다음 페이지로 이동
        /// </summary>
        public void Move_NextPage()
        {
            MovePage(true, true);
        }

        /// <summary>
        /// 페이지 이동
        /// </summary>
        /// <param name="click"></param>
        /// <param name="_next"></param>
        private void MovePage(bool click, bool _next)
        {
            switch (page_Type)
            {
                case Page_Type.Character:
                    OnClickUp_Btn(ref cor_Btn_Gra_Next, ref cor_Btn_Arrow_Next, cvg_Btn_Gradation_Next, img_Btn_Arrow_Next, false);
                    break;
                default:
                    if (_next)
                        OnClickUp_Btn(ref cor_Btn_Gra_Next, ref cor_Btn_Arrow_Next, cvg_Btn_Gradation_Next, img_Btn_Arrow_Next, page_Type.Equals(Page_Type.Name) ? click : true);
                    else
                        OnClickUp_Btn(ref cor_Btn_Gra_Prev, ref cor_Btn_Arrow_Prev, cvg_Btn_Gradation_Prev, img_Btn_Arrow_Prev, true);
                    break;
            }

            if (click)
            {
                switch (page_Type)
                {
                    case Page_Type.Character:
                        if (!cur_Character)
                        {
                            Manager.instance.manager_Common.Show_Alert0("캐릭터를 선택해 주세요.");
                            return;
                        }
                        ui_Name.Active(true);
                        Manager_Common.StartCoroutine(ref cor_Fill2, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Fill2, true));
                        Manager_Common.StartCoroutine(ref cor_Step1, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Step1, false));
                        Manager_Common.StartCoroutine(ref cor_Step2, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Step2, true));
                        Manager_Common.StartCoroutine(ref cor_Title1, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Title1, false));
                        Manager_Common.StartCoroutine(ref cor_Title2, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Title2, true));
                        img_Btn_Arrow_Prev.gameObject.SetActive(true);
                        page_Type = Page_Type.Name;
                        break;
                    case Page_Type.Name:
                        if (_next)
                        {
                            if (string.IsNullOrWhiteSpace(ipf_Name.text))
                            {
                                Manager.instance.manager_Common.Show_Alert0("이름을 입력해 주세요.");
                                ipf_Name.ActivateInputField();
                                OnClickUp_Btn(ref cor_Btn_Gra_Next, ref cor_Btn_Arrow_Next, cvg_Btn_Gradation_Next, img_Btn_Arrow_Next, false);
                                return;
                            }
                            ui_Promise.Active(true);
                            Manager_Common.StartCoroutine(ref cor_Fill3, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Fill3, true));
                            Manager_Common.StartCoroutine(ref cor_Step2, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Step2, false));
                            Manager_Common.StartCoroutine(ref cor_Step3, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Step3, true));
                            Manager_Common.StartCoroutine(ref cor_Title2, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Title2, false));
                            Manager_Common.StartCoroutine(ref cor_Title3, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Title3, true));
                            img_Btn_Arrow_Next.color = Manager.instance.col_F7F7F7;
                            page_Type = Page_Type.Promise;
                        }
                        else
                        {
                            img_Btn_Arrow_Prev.gameObject.SetActive(false);
                            ui_Name.Active(false);
                            Manager_Common.StartCoroutine(ref cor_Fill2, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Fill2, false));
                            Manager_Common.StartCoroutine(ref cor_Step1, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Step1, true));
                            Manager_Common.StartCoroutine(ref cor_Step2, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Step2, false));
                            Manager_Common.StartCoroutine(ref cor_Title1, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Title1, true));
                            Manager_Common.StartCoroutine(ref cor_Title2, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Title2, false));
                            page_Type = Page_Type.Character;
                        }
                        break;
                    case Page_Type.Promise:
                        if (_next)
                        {
                            Set_Text_End_Main();
                            ui_End.Active(true);
                            page_Type = Page_Type.End;
                        }
                        else
                        {
                            ui_Promise.Active(false);
                            Manager_Common.StartCoroutine(ref cor_Fill3, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Fill3, false));
                            Manager_Common.StartCoroutine(ref cor_Step2, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Step2, true));
                            Manager_Common.StartCoroutine(ref cor_Step3, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Step3, false));
                            Manager_Common.StartCoroutine(ref cor_Title2, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Title2, true));
                            Manager_Common.StartCoroutine(ref cor_Title3, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Title3, false));
                            img_Btn_Arrow_Next.color = Manager.instance.col_C7C7C7;
                            page_Type = Page_Type.Name;
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// 버튼 누를 시
        /// </summary>
        private void OnClickDown_Btn(ref CoroutineHandle cor_Gra, ref CoroutineHandle cor_Arrow, CanvasGroup cvg_gra, Image img_gra, Image img_Arrow, bool isFilled)
        {
            img_gra.color = isFilled ? Color.white : Manager.instance.col_Main;
            Manager_Common.StartCoroutine(ref cor_Gra, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_gra, 0.5f));
            Manager_Common.StartCoroutine(ref cor_Arrow, Manager.instance.manager_Ui.Cor_Color(img_Arrow, isFilled ? Manager.instance.col_Main : Manager.instance.col_F7F7F7));
        }

        /// <summary>
        /// 버튼 누른 후 뗄 시
        /// </summary>
        private void OnClickUp_Btn(ref CoroutineHandle cor_Gra, ref CoroutineHandle cor_Arrow, CanvasGroup cvg_gra, Image img_Arrow, bool isFilled)
        {
            Manager_Common.StartCoroutine(ref cor_Gra, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_gra, 0));
            Manager_Common.StartCoroutine(ref cor_Arrow, Manager.instance.manager_Ui.Cor_Color(img_Arrow, isFilled ? Manager.instance.col_F7F7F7 : Manager.instance.col_C7C7C7));
        }
        #endregion

        #region 1. Subpage_Character
        [Header("Subpage_Character")]
        [Tooltip("Subpage_Character")]
        [SerializeField] private UI ui_Character;
        [Tooltip("현재 선택된 캐릭터")]
        [SerializeField] private Prefab_Box_Character cur_Character;

        /// <summary>
        /// 캐릭터 선택 페이지 초기화
        /// </summary>
        private void Init_Character()
        {
            cur_Character = null;
        }

        /// <summary>
        /// 캐릭터 선택 시
        /// </summary>
        public void OnSelect_Character(Prefab_Box_Character prefab_Box_Character)
        {
            if(cur_Character)
                cur_Character.DeSelect();
            cur_Character = prefab_Box_Character;
            cur_Character.Select();
        }
        #endregion

        #region 2. Subpage_Name
        [Header("Subpage_Name")]
        [Tooltip("Subpage_Name")]
        [SerializeField] private UI ui_Name;
        [Tooltip("이름 인풋필드")]
        [SerializeField] private InputField ipf_Name;
        [Tooltip("이름 인풋플드 언더라인")]
        [SerializeField] private RectTransform rect_Name_UnderLine;
        

        /// <summary>
        /// 이름 페이지 초기화
        /// </summary>
        private void Init_Name()
        {
            ipf_Name.text = string.Empty;
        }

        CoroutineHandle cor_Ipf_Name;
        /// <summary>
        /// 이름 인풋필드 클릭시
        /// </summary>
        public void OnSelect_Ipf_Name()
        {
            Manager_Common.StartCoroutine(ref cor_Ipf_Name, Manager.instance.manager_Ui.Cor_Scale_Anchored(rect_Name_UnderLine, Vector3.one, 10));
        }

        /// <summary>
        /// 이름 인풋필드 클릭 해제시
        /// </summary>
        public void OnDeselect_Ipf_Name()
        {
            if (string.IsNullOrWhiteSpace(ipf_Name.text))
            {
                Manager_Common.StartCoroutine(ref cor_Ipf_Name, Manager.instance.manager_Ui.Cor_Scale_Anchored(rect_Name_UnderLine, new Vector3(0, 1, 1), 10));
            }
        }

        
        #endregion

        #region 3. Subpage_Promise
        [Header("Subpage_Promise")]
        [SerializeField] private UI ui_Promise;
        [Tooltip("약속 인풋필드")]
        [SerializeField] private InputField ipf_Promise;
        [Tooltip("약속 인풋플드 언더라인")]
        [SerializeField] private RectTransform rect_Promise_UnderLine;

        /// <summary>
        /// 약속 페이지 초기화
        /// </summary>
        private void Init_Promise()
        {
            ipf_Promise.text = string.Empty;
        }

        CoroutineHandle cor_Ipf_Promise;
        /// <summary>
        /// 약속 인풋필드 클릭시
        /// </summary>
        public void OnSelect_Ipf_Promise()
        {
            float width = rect_Name_UnderLine.transform.parent.GetComponent<RectTransform>().rect.width;
            Manager_Common.StartCoroutine(ref cor_Ipf_Promise, Manager.instance.manager_Ui.Cor_Scale_Anchored(rect_Promise_UnderLine, Vector3.one, 10));
        }

        /// <summary>
        /// 약속 인풋필드 클릭 해제시
        /// </summary>
        public void OnDeselect_Ipf_Promise()
        {
            if (string.IsNullOrWhiteSpace(ipf_Promise.text))
            {
                Manager_Common.StartCoroutine(ref cor_Ipf_Promise, Manager.instance.manager_Ui.Cor_Scale_Anchored(rect_Promise_UnderLine, new Vector3(0, 1, 1), 10));
            }
        }
        #endregion

        #region 4. Subpage_End
        [Header("Subpage_End")]
        [Tooltip("Subpage_Character")]
        [SerializeField] private UI ui_End;
        [SerializeField] private Text txt_End_Main;
        [SerializeField] private Text text_Do;
        [SerializeField] private Text text_Dont;
        [SerializeField] private Image img_Character;
        [SerializeField] private RectTransform rect_End_Do_Back;
        [SerializeField] private RectTransform rect_End_Dont_Back;

        /// <summary>
        /// End 메인텍스트 입력
        /// </summary>
        private void Set_Text_End_Main()
        {
            txt_End_Main.text = Manager_Language.Get_Text($"{{0}} 님,\n#Selftodo를\n시작해볼까요?", ipf_Name.text);
            img_Character.sprite = cur_Character.get_Img_Char.sprite;
        }

        CoroutineHandle cor_End_Do_Back_Scale;
        CoroutineHandle cor_End_Do_Back_Cvg;
        CoroutineHandle cor_End_Do_Text;
        /// <summary>
        /// 완료 버튼 누를 시
        /// </summary>
        public void OnClickDown_End_Do()
        {
            Debug.Log("OnClickDown_End_Do : " + rect_End_Do_Back.name, rect_End_Do_Back);
            Manager_Common.StartCoroutine(ref cor_End_Do_Back_Scale, Manager.instance.manager_Ui.Cor_Scale_Anchored(rect_End_Do_Back, Vector3.one, 8));
            Manager_Common.StartCoroutine(ref cor_End_Do_Back_Cvg, Manager.instance.manager_Ui.Cor_CanvasAlpha(rect_End_Do_Back.GetComponent<CanvasGroup>(), true));
            Manager_Common.StartCoroutine(ref cor_End_Do_Text, Manager.instance.manager_Ui.Cor_Color(text_Do, Manager.instance.col_F7F7F7));
            Manager.instance.manager_Common.PointDown();
        }

        /// <summary>
        /// 완료 버튼 누른 후 뗄 시
        /// </summary>
        public void OnClickUp_End_Do()
        {
            Manager_Common.StartCoroutine(ref cor_End_Do_Back_Scale, Manager.instance.manager_Ui.Cor_Scale_Anchored(rect_End_Do_Back, Vector3.zero));
            Manager_Common.StartCoroutine(ref cor_End_Do_Back_Cvg, Manager.instance.manager_Ui.Cor_CanvasAlpha(rect_End_Do_Back.GetComponent<CanvasGroup>(), false));
            Manager_Common.StartCoroutine(ref cor_End_Do_Text, Manager.instance.manager_Ui.Cor_Color(text_Do, Manager.instance.col_707070));
            if (Manager.instance.manager_Common.PointUp_OneClick())
            {
                Manager.instance.manager_Common.Start_Loading();
#if USE_LOGIN
                Timing.RunCoroutine(Manager.instance.manager_Web.Cor_Member_Update_Nickname_Character_Promise(Manager.instance.manager_Member.Get_Data_Member.id, ipf_Name.text, cur_Character.get_Char_Name.ToString(), ipf_Promise.text, (string _return) =>
                {
                    JsonData _json = JsonMapper.ToObject(_return);
                    bool nickname_changed = bool.Parse(_json["nickname_changed"].ToString());
                    Manager_Member.Data_Member member = Manager.instance.manager_Member.Get_Data_Member;
                    member.nickname = ipf_Name.text;
                    if (nickname_changed)
                        member.nickname_code = _json["nickname_code"].ToString();
                    Manager.instance.manager_Member.Set_Data_Member(member);
                    Manager.instance.manager_Common.Active_Page(Manager.instance.page_Calender); // 캘린더 켜기
                    Manager.instance.manager_Common.Stop_Loading();
                }));
#else
                Timing.RunCoroutine(Cor_Join());
                IEnumerator<float> Cor_Join()
                {
                    string platform = Application.platform.ToString();
#if UNITY_ANDROID
                    platform = "Android";
#elif UNITY_IOS
                    platform = "iOS";
#endif
                    string devicemodel = SystemInfo.deviceModel;
                    string devicename = SystemInfo.deviceName;
                    string deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
#if UNITY_EDITOR || UNITY_STANDALONE
                    Manager.instance.manager_Firebase.token = "StubToken";
                    yield return Timing.WaitForSeconds(0);
#elif UNITY_ANDROID || UNITY_IOS
                    if (string.IsNullOrWhiteSpace(Manager.instance.manager_Firebase.token))
                    {
                        Manager.instance.manager_Firebase.GetToken();
                        yield return Timing.WaitUntilTrue(() => !string.IsNullOrWhiteSpace(Manager.instance.manager_Firebase.token));
                    }
#endif
                    Timing.RunCoroutine(Manager.instance.manager_Web.Cor_Member_Insert(devicemodel, devicename, deviceUniqueIdentifier, ipf_Name.text, cur_Character.get_Char_Name.ToString(), ipf_Promise.text, platform, Manager.instance.manager_Firebase.token, (_return) =>
                    {
                        string id = "";
                        string nickname_code = "";
                        string special_code = "";
                        if (_return == null)
                        {
                            id = Manager.instance.manager_Common.DateTimeToYMDHmS2(DateTime.Now);
                        }
                        else
                        {
                            JsonData _json = JsonMapper.ToObject(_return);
                            id = _json["id"].ToString();
                            nickname_code = _json["nickname_code"].ToString();
                            special_code = _json["special_code"].ToString();
                        }
                        
                        Manager_Member.Data_Member _member = new Manager_Member.Data_Member(id, devicemodel, devicename, deviceUniqueIdentifier, ipf_Name.text, nickname_code, cur_Character.get_Char_Name.ToString(), ipf_Promise.text, platform, Manager.instance.manager_Firebase.token, special_code);
                        Manager.instance.manager_Member.Set_Data_Member(_member);
                        Manager.instance.manager_Common.Save_Regedit("ssh_m15", JsonUtility.ToJson(_member));

                        Manager.instance.manager_Data.Todo_Insert("-", false, "0", "0", "0", "0", "0", "0", "0", "00:00",
                        (Manager_Data.Data_Todo _return) =>
                        {
                            Manager.instance.manager_Common.Active_Page(Manager.instance.page_Calender);
                            Manager.instance.manager_Common.Stop_Loading();
                        });

                    }));
                }
#endif
            }
        }

        CoroutineHandle cor_End_Dont_Back_Scale;
        CoroutineHandle cor_End_Dont_Back_Cvg;
        CoroutineHandle cor_End_Dont_Text;
        /// <summary>
        /// 취소 버튼 누를 시
        /// </summary>
        public void OnClickDown_End_Dont()
        {
            Debug.Log("OnClickDown_End_Dont : " + rect_End_Dont_Back.name, rect_End_Dont_Back);
            Manager_Common.StartCoroutine(ref cor_End_Dont_Back_Scale, Manager.instance.manager_Ui.Cor_Scale_Anchored(rect_End_Dont_Back, Vector3.one, 8));
            Manager_Common.StartCoroutine(ref cor_End_Dont_Back_Cvg, Manager.instance.manager_Ui.Cor_CanvasAlpha(rect_End_Dont_Back.GetComponent<CanvasGroup>(), true));
            Manager_Common.StartCoroutine(ref cor_End_Dont_Text, Manager.instance.manager_Ui.Cor_Color(text_Dont, Manager.instance.col_F7F7F7));
            Manager.instance.manager_Common.PointDown();
        }

        /// <summary>
        /// 취소 버튼 누른 후 뗄 시
        /// </summary>
        public void OnClickUp_End_Dont()
        {
            Manager_Common.StartCoroutine(ref cor_End_Dont_Back_Scale, Manager.instance.manager_Ui.Cor_Scale_Anchored(rect_End_Dont_Back, Vector3.zero));
            Manager_Common.StartCoroutine(ref cor_End_Dont_Back_Cvg, Manager.instance.manager_Ui.Cor_CanvasAlpha(rect_End_Dont_Back.GetComponent<CanvasGroup>(), false));
            Manager_Common.StartCoroutine(ref cor_End_Dont_Text, Manager.instance.manager_Ui.Cor_Color(text_Dont, Manager.instance.col_707070));
            if (Manager.instance.manager_Common.PointUp_OneClick())
            {
#if USE_LOGIN
                Manager.instance.manager_Common.Active_Page(Manager.instance.page_Start);
#else
                Manager.instance.manager_Common.Quit();
#endif
            }
        }

        public void OnClickButton_End_Back()
        {
            page_Type = Page_Type.Promise;
            ui_End.Active(false);
        }
#endregion
    }
}