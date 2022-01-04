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

        #region 0. Subpage ����
        public enum Page_Type { Character, Name, Promise, End }
        [Header("Subpage_����")]
        [SerializeField] private Page_Type page_Type;
        [Tooltip("Ÿ��Ʋ Title_1")]
        [SerializeField] private CanvasGroup cvg_Title1;
        [Tooltip("Ÿ��Ʋ Title_2")]
        [SerializeField] private CanvasGroup cvg_Title2;
        [Tooltip("Ÿ��Ʋ Title_3")]
        [SerializeField] private CanvasGroup cvg_Title3;

        [Tooltip("��ư ȭ��ǥ - Next")]
        [SerializeField] private Image img_Btn_Arrow_Next;
        [Tooltip("��ư ȭ��ǥ - Prev")]
        [SerializeField] private Image img_Btn_Arrow_Prev;
        [Tooltip("��ư �׶��̼� - Next")]
        [SerializeField] private CanvasGroup cvg_Btn_Gradation_Next;
        [Tooltip("��ư �׶��̼� - Prev")]
        [SerializeField] private CanvasGroup cvg_Btn_Gradation_Prev;
        [Tooltip("�̹��� �׶��̼� - Next")]
        [SerializeField] private Image img_Btn_Gradation_Next;
        [Tooltip("�̹��� �׶��̼� - Prev")]
        [SerializeField] private Image img_Btn_Gradation_Prev;
        [Tooltip("��ư Fill_2")]
        [SerializeField] private CanvasGroup cvg_Fill2;
        [Tooltip("��ư Fill_3")]
        [SerializeField] private CanvasGroup cvg_Fill3;
        [Tooltip("��ư Step_1")]
        [SerializeField] private CanvasGroup cvg_Step1;
        [Tooltip("��ư Step_2")]
        [SerializeField] private CanvasGroup cvg_Step2;
        [Tooltip("��ư Step_3")]
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
        /// ���� �ʱ�ȭ
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
        /// �̸� ��ư ���� ��
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
        /// �̸� ��ư ���� �� �� ��
        /// </summary>
        public void OnClickUp_Btn(bool _next)
        {
            bool click = Manager.instance.manager_Common.PointUp_OneClick();
            MovePage(click, _next);
        }

        /// <summary>
        /// ���� �������� �̵�
        /// </summary>
        public void Move_NextPage()
        {
            MovePage(true, true);
        }

        /// <summary>
        /// ������ �̵�
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
                            Manager.instance.manager_Common.Show_Alert0("ĳ���͸� ������ �ּ���.");
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
                                Manager.instance.manager_Common.Show_Alert0("�̸��� �Է��� �ּ���.");
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
        /// ��ư ���� ��
        /// </summary>
        private void OnClickDown_Btn(ref CoroutineHandle cor_Gra, ref CoroutineHandle cor_Arrow, CanvasGroup cvg_gra, Image img_gra, Image img_Arrow, bool isFilled)
        {
            img_gra.color = isFilled ? Color.white : Manager.instance.col_Main;
            Manager_Common.StartCoroutine(ref cor_Gra, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_gra, 0.5f));
            Manager_Common.StartCoroutine(ref cor_Arrow, Manager.instance.manager_Ui.Cor_Color(img_Arrow, isFilled ? Manager.instance.col_Main : Manager.instance.col_F7F7F7));
        }

        /// <summary>
        /// ��ư ���� �� �� ��
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
        [Tooltip("���� ���õ� ĳ����")]
        [SerializeField] private Prefab_Box_Character cur_Character;

        /// <summary>
        /// ĳ���� ���� ������ �ʱ�ȭ
        /// </summary>
        private void Init_Character()
        {
            cur_Character = null;
        }

        /// <summary>
        /// ĳ���� ���� ��
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
        [Tooltip("�̸� ��ǲ�ʵ�")]
        [SerializeField] private InputField ipf_Name;
        [Tooltip("�̸� ��ǲ�õ� �������")]
        [SerializeField] private RectTransform rect_Name_UnderLine;
        

        /// <summary>
        /// �̸� ������ �ʱ�ȭ
        /// </summary>
        private void Init_Name()
        {
            ipf_Name.text = string.Empty;
        }

        CoroutineHandle cor_Ipf_Name;
        /// <summary>
        /// �̸� ��ǲ�ʵ� Ŭ����
        /// </summary>
        public void OnSelect_Ipf_Name()
        {
            Manager_Common.StartCoroutine(ref cor_Ipf_Name, Manager.instance.manager_Ui.Cor_Scale_Anchored(rect_Name_UnderLine, Vector3.one, 10));
        }

        /// <summary>
        /// �̸� ��ǲ�ʵ� Ŭ�� ������
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
        [Tooltip("��� ��ǲ�ʵ�")]
        [SerializeField] private InputField ipf_Promise;
        [Tooltip("��� ��ǲ�õ� �������")]
        [SerializeField] private RectTransform rect_Promise_UnderLine;

        /// <summary>
        /// ��� ������ �ʱ�ȭ
        /// </summary>
        private void Init_Promise()
        {
            ipf_Promise.text = string.Empty;
        }

        CoroutineHandle cor_Ipf_Promise;
        /// <summary>
        /// ��� ��ǲ�ʵ� Ŭ����
        /// </summary>
        public void OnSelect_Ipf_Promise()
        {
            float width = rect_Name_UnderLine.transform.parent.GetComponent<RectTransform>().rect.width;
            Manager_Common.StartCoroutine(ref cor_Ipf_Promise, Manager.instance.manager_Ui.Cor_Scale_Anchored(rect_Promise_UnderLine, Vector3.one, 10));
        }

        /// <summary>
        /// ��� ��ǲ�ʵ� Ŭ�� ������
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
        /// End �����ؽ�Ʈ �Է�
        /// </summary>
        private void Set_Text_End_Main()
        {
            txt_End_Main.text = Manager_Language.Get_Text($"{{0}} ��,\n#Selftodo��\n�����غ����?", ipf_Name.text);
            img_Character.sprite = cur_Character.get_Img_Char.sprite;
        }

        CoroutineHandle cor_End_Do_Back_Scale;
        CoroutineHandle cor_End_Do_Back_Cvg;
        CoroutineHandle cor_End_Do_Text;
        /// <summary>
        /// �Ϸ� ��ư ���� ��
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
        /// �Ϸ� ��ư ���� �� �� ��
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
                    Manager.instance.manager_Common.Active_Page(Manager.instance.page_Calender); // Ķ���� �ѱ�
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
        /// ��� ��ư ���� ��
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
        /// ��� ��ư ���� �� �� ��
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