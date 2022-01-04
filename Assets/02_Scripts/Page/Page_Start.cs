using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

namespace NORK
{
    public class Page_Start : Page
    {
        internal override void OnEnable()
        {
            page_Start.Active(true);
            base.OnEnable();
            Init();
        }

        internal override void Start()
        {
            base.Start();
            Check_Member();
        }

        //�б��� : �α��� �̿��� �� ���
        /*
        private void Update()
        {
            Update_Page();
            Update_Login();
            Update_Join();
            Update_FindPwd();
            Update_RePwd();
        }
        */

        /// <summary>
        /// ��ü �ʱ�ȭ
        /// </summary>
        private void Init()
        {
            if(page_Login.gameObject.activeSelf)
                page_Login.Active(false);
            if (page_Join.gameObject.activeSelf)
                page_Join.Active(false);
            if (page_FindPwd.gameObject.activeSelf)
                page_FindPwd.Active(false);
            if (page_RePwd.gameObject.activeSelf)
                page_RePwd.Active(false);
            cur_Pages = new List<Page>();
            cur_Pages.Add(page_Start);

            Timing.RunCoroutine(Cor_Init_Start());
        }

        /// <summary>
        /// ��ü ������Ʈ
        /// </summary>
        private void Update_Page()
        {
            if (Manager_Common.isBack)
            {
                if(cur_Pages.Count.Equals(1))
                {
                    Manager.instance.manager_Common.Start_Quit();
                }
                else
                {
                    if(cur_Pages[cur_Pages.Count - 1].Equals(page_Login))
                        Active_Login(false);
                    else if (cur_Pages[cur_Pages.Count - 1].Equals(page_Join))
                        Active_Join(false);
                    else if (cur_Pages[cur_Pages.Count - 1].Equals(page_FindPwd))
                        Active_FindPwd(false);
                    else if (cur_Pages[cur_Pages.Count - 1].Equals(page_RePwd))
                        Active_RePwd(false);
                }
            }
        }

        #region 0. Subpage ����
        /// <summary>
        /// ȸ������ ����
        /// </summary>
        private void Set_Data_Member(string _data)
        {
            if(!string.IsNullOrWhiteSpace(_data))
                Manager.instance.manager_Member.Set_Data_Member(_data);
            Login();
        }

        /// <summary>
        /// ȸ������ ����
        /// </summary>
        private void Set_Data_Member(Manager_Member.Data_Member _data)
        {
            Manager.instance.manager_Member.Set_Data_Member(_data);
            Login();
        }

        /// <summary>
        /// �α���
        /// </summary>
        private void Login()
        {
            if (string.IsNullOrWhiteSpace(Manager.instance.manager_Member.Get_Data_Member.nickname))
                Manager.instance.manager_Common.Active_Page(Manager.instance.page_Set_Profile);
            else
                Manager.instance.manager_Common.Active_Page(Manager.instance.page_Calender);
        }

        /// <summary>
        /// �ڷΰ���
        /// </summary>
        public void OnClickButton_Back(Page _page)
        {
            _page.Active(false);
        }

        private List<Page> cur_Pages;
        /// <summary>
        /// �α��� ������ �ѱ�/����
        /// </summary>
        private void Active_Login(bool _active)
        {
            if (_active)
                Init_Login();
            page_Login.Active(_active);
            if (_active)
                cur_Pages.Add(page_Login);
            else
                cur_Pages.RemoveAt(cur_Pages.LastIndexOf(page_Login));
        }

        /// <summary>
        /// ȸ������ ������ �ѱ�/����
        /// </summary>
        private void Active_Join(bool _active)
        {
            if (_active)
                Init_Join();
            page_Join.Active(_active);
            if (_active)
                cur_Pages.Add(page_Join);
            else
                cur_Pages.RemoveAt(cur_Pages.LastIndexOf(page_Join));
        }

        /// <summary>
        /// ��й�ȣ ã�� ������ �ѱ�/����
        /// </summary>
        private void Active_FindPwd(bool _active)
        {
            if (_active)
                Init_FindPwd();
            page_FindPwd.Active(_active);
            if (_active)
                cur_Pages.Add(page_FindPwd);
            else
                cur_Pages.RemoveAt(cur_Pages.LastIndexOf(page_FindPwd));
        }

        /// <summary>
        /// ��й�ȣ �缳�� ������ �ѱ�/����
        /// </summary>
        private void Active_RePwd(bool _active)
        {
            if (_active)
                Init_RePwd();
            page_RePwd.Active(_active);
            if (_active)
                cur_Pages.Add(page_RePwd);
            else
                cur_Pages.RemoveAt(cur_Pages.LastIndexOf(page_RePwd));
        }

        /// <summary>
        /// ��ư ���� ��
        /// </summary>
        public void OnClickDown_Btn(GameObject go_Arround)
        {
            go_Arround.SetActive(true);
        }

        /// <summary>
        /// ��ư ���� �� �� ��
        /// </summary>
        public void OnClickUp_Btn(GameObject go_Arround)
        {
            go_Arround.SetActive(false);
        }

        /// <summary>
        /// �� Ŭ�� ��
        /// </summary>
        private void OnClickButton_Eye(ref bool _show, InputField ipf_Pwd, Text txt_Pwd, Image img_Eye)
        {
            _show = !_show;
            if (_show)
            {
                ipf_Pwd.textComponent.gameObject.SetActive(true);
                txt_Pwd.gameObject.SetActive(false);
                img_Eye.color = Color.black;
                ipf_Pwd.caretPosition = ipf_Pwd.text.Length;
            }
            else
            {
                ipf_Pwd.textComponent.gameObject.SetActive(false);
                txt_Pwd.gameObject.SetActive(true);
                img_Eye.color = Manager.instance.col_C7C7C7;
                ipf_Pwd.caretPosition = txt_Pwd.text.Length;
            }
        }

        /// <summary>
        /// ��й�ȣ ������ �ڸ� �����ϰ� *�� �����
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        private string ConvertPassword(string _value)
        {
            int _length = _value.Length;
            string _t = "";
            if (_length > 0)
            {
                for (int i = 0; i < _length - 1; i++)
                {
                    _t += "*";
                }
                _t = $"{_t}{_value[_length - 1]}";
            }
            return _t;
        }

        private string Ssh_m15(string _account, string _pwd)
        {
            return $"{{\"account\":\"{_account}\",\"pwd\":\"{_pwd}\"}}";
        }
        #endregion

        #region 1. Subpage_Start
        [Header("Subpage_Start")]
        [SerializeField] private Page page_Start;

        [SerializeField] RectTransform rect_Start_Title;
        [SerializeField] RectTransform rect_Start_Login;
        [SerializeField] RectTransform rect_Start_Join;

        [SerializeField] CanvasGroup cvg_Start_Title;
        [SerializeField] CanvasGroup cvg_Start_Login;
        [SerializeField] CanvasGroup cvg_Start_Join;

        /// <summary>
        /// Start ����
        /// </summary>
        /// <returns></returns>
        IEnumerator<float> Cor_Init_Start()
        {
            cvg_Start_Title.alpha = 0;
            cvg_Start_Login.alpha = 0;
            cvg_Start_Join.alpha = 0;

            Vector3 title_Pos = rect_Start_Title.anchoredPosition;
            Vector3 login_Pos = rect_Start_Login.anchoredPosition;
            Vector3 join_Pos = rect_Start_Join.anchoredPosition;

            rect_Start_Title.anchoredPosition = title_Pos - new Vector3(0, 50);
            rect_Start_Login.anchoredPosition = login_Pos - new Vector3(0, 50);
            rect_Start_Join.anchoredPosition = join_Pos - new Vector3(0, 50);

            Timing.RunCoroutine(Manager.instance.manager_Ui.Cor_Pos_Anchored(rect_Start_Title, title_Pos));
            Timing.RunCoroutine(Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Start_Title, true));

            yield return Timing.WaitForSeconds(0.5f);

            Timing.RunCoroutine(Manager.instance.manager_Ui.Cor_Pos_Anchored(rect_Start_Login, login_Pos));
            Timing.RunCoroutine(Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Start_Login, true));

            yield return Timing.WaitForSeconds(0.5f);
           
            Timing.RunCoroutine(Manager.instance.manager_Ui.Cor_Pos_Anchored(rect_Start_Join, join_Pos));
            Timing.RunCoroutine(Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Start_Join, true));
        }

        /// <summary>
        /// ȸ������ üũ
        /// </summary>
        private void Check_Member()
        {
            string _member = Manager.instance.manager_Common.Load_Regedit("ssh_m15"); // ����� ȸ������ ��������

#if USE_LOGIN
            if (!string.IsNullOrWhiteSpace(_member)) // ����� ȸ�������� ������ �ٷ� ������������ �Ѿ��
            {
                Active_Login(true);
                JsonData _json = JsonMapper.ToObject(_member);
                ipf_Login_Account.text = _json["account"].ToString();
                ipf_Login_Pwd.text = _json["pwd"].ToString();
                OnClickButton_Login();
            }
#else
            Set_Data_Member(_member);
#endif
        }

        /// <summary>
        /// �α��� �ϱ� ��ư ������
        /// </summary>
        public void OnClicDown_Start_Login(GameObject go_Arround)
        {
            OnClickDown_Btn(go_Arround);
            Manager.instance.manager_Common.PointDown();
        }

        /// <summary>
        /// �α��� �ϱ� ��ư ���� �� �� ��
        /// </summary>
        public void OnClickUp_Start_Login(GameObject go_Arround)
        {
            OnClickUp_Btn(go_Arround);
            if (Manager.instance.manager_Common.PointUp_OneClick())
                Active_Login(true);
        }

        /// <summary>
        /// ȸ������ �ϱ� ��ư ������
        /// </summary>
        public void OnClicDown_Start_Join(GameObject go_Arround)
        {
            OnClickDown_Btn(go_Arround);
            Manager.instance.manager_Common.PointDown();
        }

        /// <summary>
        /// ȸ������ �ϱ� ��ư ���� �� �� ��
        /// </summary>
        public void OnClickUp_Start_Join(GameObject go_Arround)
        {
            OnClickUp_Btn(go_Arround);
            if (Manager.instance.manager_Common.PointUp_OneClick())
                Active_Join(true);
        }
#endregion

#region 2. Subpage_Login
        [Header("Subpage_Login")]
        [Tooltip("�α��� ������")]
        [SerializeField] private Page page_Login;
        [Tooltip("�α��� ����")]
        [SerializeField] private InputField ipf_Login_Account;
        [Tooltip("�α��� ��й�ȣ")]
        [SerializeField] private InputField ipf_Login_Pwd;
        [Tooltip("�α��� ��й�ȣ *�� ǥ�õǴ� �ؽ�Ʈ")]
        [SerializeField] private Text text_Login_Pwd;
        [Tooltip("�α��� ��й�ȣ �� �̹���")]
        [SerializeField] private Image img_Login_Pwd_Eye;
        [Tooltip("�α��� ��й�ȣ �� Ŭ�� ����")]
        [SerializeField] private bool b_Login_Pwd_Show;

        /// <summary>
        /// �α��� ������Ʈ
        /// </summary>
        private void Update_Login()
        {
            if (Manager_Common.isTab)
            {
                if (ipf_Login_Account.isFocused)
                    Manager.instance.manager_Common.Select_Inputfield(ipf_Login_Pwd);
            }
            if (Manager_Common.isEnter)
                OnClickButton_Login();
        }

        /// <summary>
        /// �α��� ������ �ʱ�ȭ
        /// </summary>
        private void Init_Login()
        {
            ipf_Login_Account.text = ipf_Login_Pwd.text = text_Login_Pwd.text = string.Empty;
            b_Login_Pwd_Show = false;
            ipf_Login_Pwd.textComponent.gameObject.SetActive(false);
            text_Login_Pwd.gameObject.SetActive(true);
            img_Login_Pwd_Eye.color = Manager.instance.col_C7C7C7;
        }

        /// <summary>
        /// ��й�ȣ �Է� ��
        /// </summary>
        /// <param name="_value"></param>
        public void OnValueChanged_Login_Pwd(string _value)
        {
            text_Login_Pwd.text = ConvertPassword(_value);
            ipf_Login_Pwd.caretPosition = b_Login_Pwd_Show ? ipf_Login_Pwd.text.Length : text_Login_Pwd.text.Length;
        }

        /// <summary>
        /// �� Ŭ�� ��
        /// </summary>
        /// <param name="_show"></param>
        public void OnClickButton_Login_EyePwd()
        {
            OnClickButton_Eye(ref b_Login_Pwd_Show, ipf_Login_Pwd, text_Login_Pwd, img_Login_Pwd_Eye);
        }

        /// <summary>
        /// �α��� ��ư Ŭ�� ��
        /// </summary>
        public void OnClickButton_Login()
        {
            if (string.IsNullOrWhiteSpace(ipf_Login_Account.text))
            {
                Manager.instance.manager_Common.Show_Alert0("���̵� �Է��� �ּ���.");
                Manager.instance.manager_Common.Select_Inputfield(ipf_Login_Account);
                return;
            }
            if (!Manager.instance.manager_Common.Check_Regax_Account(ipf_Login_Account.text))
            {
                Manager.instance.manager_Common.Show_Alert0("�̸��� ���Ŀ� �°� �Է��� �ּ���.");
                Manager.instance.manager_Common.Select_Inputfield(ipf_Login_Account);
                return;
            }
            if (string.IsNullOrWhiteSpace(ipf_Login_Pwd.text))
            {
                Manager.instance.manager_Common.Show_Alert0("��й�ȣ�� �Է��� �ּ���.");
                Manager.instance.manager_Common.Select_Inputfield(ipf_Login_Pwd);
                return;
            }
            if (!Manager.instance.manager_Common.Check_Regax_Pwd(ipf_Login_Pwd.text))
            {
                Manager.instance.manager_Common.Show_Alert0("�ϳ� �̻��� ����, ����, Ư�����ڸ� ������ 8~20�ڸ� �Է��� �ּ���.");
                Manager.instance.manager_Common.Select_Inputfield(ipf_Login_Pwd);
                return;
            }
            Manager.instance.manager_Common.Start_Loading();
            Timing.RunCoroutine(Manager.instance.manager_Web.Cor_Member_Check(ipf_Login_Account.text, ipf_Login_Pwd.text, (string _return) =>
            {
                if(string.IsNullOrWhiteSpace(_return) || _return.Equals("null"))
                {
                    Manager.instance.manager_Common.Show_Alert0("���̵� �Ǵ� ��й�ȣ�� Ȯ���� �ּ���.");
                }
                else
                {
                    Manager.instance.manager_Common.Save_Regedit("ssh_m15", Ssh_m15(ipf_Login_Account.text, ipf_Login_Pwd.text));
                    Set_Data_Member(_return);
                }
                
                Manager.instance.manager_Common.Stop_Loading();
            }));
        }

        /// <summary>
        /// ��й�ȣ ã��
        /// </summary>
        public void OnClickButton_Go_FindPwd()
        {
            Active_FindPwd(true);
        }

        /// <summary>
        /// ȸ������
        /// </summary>
        public void OnClickButton_Go_Join()
        {
            Active_Join(true);
        }
#endregion

#region 3. Subpage_Join
        [Header("Subpage_Join")]
        [Tooltip("ȸ������ ������")]
        [SerializeField] private Page page_Join;
        [Tooltip("ȸ������ ����")]
        [SerializeField] private InputField ipf_Join_Account;
        [Tooltip("ȸ������ ���� ��ư")]
        [SerializeField] private Button btn_Join_Cert_Send;
        [Tooltip("ȸ������ ������ȣ")]
        [SerializeField] private InputField ipf_Join_Cert;
        [Tooltip("ȸ������ ����Ȯ�� ��ư")]
        [SerializeField] private Button btn_Join_Cert_Check;
        [Tooltip("ȸ������ �����Ϸ� ����")]
        [SerializeField] private bool b_Join_Cert_Complete;

        [Tooltip("ȸ������ ��й�ȣ ")]
        [SerializeField] private RectTransform rect_Join_Pwd;
        [Tooltip("ȸ������ ��й�ȣ")]
        [SerializeField] private InputField ipf_Join_Pwd;
        [Tooltip("ȸ������ ��й�ȣ2")]
        [SerializeField] private InputField ipf_Join_Pwd2;
        [Tooltip("ȸ������ ��й�ȣ *�� ǥ�õǴ� �ؽ�Ʈ")]
        [SerializeField] private Text text_Join_Pwd2;
        [Tooltip("ȸ������ ��й�ȣ �� �̹���")]
        [SerializeField] private Image img_Join_Pwd2_Eye;
        [Tooltip("ȸ������ ��й�ȣ �� Ŭ�� ����")]
        [SerializeField] private bool b_Join_Pwd2_Show;
        
        /// <summary>
        /// ȸ������ ������Ʈ
        /// </summary>
        private void Update_Join()
        {
            if (Manager_Common.isTab)
            {
                if (ipf_Join_Pwd.isFocused)
                    Manager.instance.manager_Common.Select_Inputfield(ipf_Join_Pwd2);
            }
            if (Manager_Common.isEnter)
            {
                if (ipf_Join_Account.isFocused)
                    OnClickButton_Join_Cert_Insert();
                else if (ipf_Join_Cert.isFocused)
                    OnClickButton_Join_Cert_Check();
                else if (ipf_Join_Pwd2.isFocused)
                    OnClickButton_Join();
            }
        }

        /// <summary>
        /// ȸ������ ������ �ʱ�ȭ
        /// </summary>
        private void Init_Join()
        {
            b_Join_Pwd2_Show = false;
            ipf_Join_Pwd2.textComponent.gameObject.SetActive(false);
            text_Join_Pwd2.gameObject.SetActive(true);
            ipf_Join_Account.readOnly = ipf_Join_Cert.readOnly = false;
            ipf_Join_Cert.readOnly = ipf_Join_Pwd.readOnly = ipf_Join_Pwd2.readOnly = true;
            btn_Join_Cert_Send.interactable = false;
            rect_Join_Pwd.localScale = new Vector3(1, 0, 1);
            b_Join_Cert_Complete = false;
        }

        /// <summary>
        /// ȸ������ ���� ��ǲ�ʵ� �Է½�
        /// </summary>
        /// <param name="_value"></param>
        public void OnValueChanged_Join_Account(string _value)
        {
            btn_Join_Cert_Send.interactable = Manager.instance.manager_Common.Check_Regax_Account(_value);
        }

        /// <summary>
        /// ���� ������
        /// </summary>
        public void OnClickButton_Join_Cert_Insert()
        {
            if (b_Join_Cert_Complete) return;
            if (string.IsNullOrWhiteSpace(ipf_Join_Account.text))
            {
                Manager.instance.manager_Common.Show_Alert0("�̸����� �Է��� �ּ���.");
                return;
            }
            if (!Manager.instance.manager_Common.Check_Regax_Account(ipf_Join_Account.text))
            {
                Manager.instance.manager_Common.Show_Alert0("�̸��� ���Ŀ� �°� �Է��� �ּ���.");
                return;
            }

            Manager.instance.manager_Common.Start_Loading();
            Timing.RunCoroutine(Manager.instance.manager_Web.Cor_Cert_Insert(ipf_Join_Account.text, (string _return) =>
            {
                JsonData _json = JsonMapper.ToObject(_return);
                string cert= _json["cert"].ToString();
                Manager.instance.manager_Common.Show_Alert0("������ȣ�� ���۵Ǿ����ϴ�.");
                Manager.instance.manager_Email.Send_Mail(ipf_Join_Account.text, Manager.instance.manager_Email.Get_Data_Mail(Manager_Email.Type_Mail.������ȣ, cert));
                btn_Join_Cert_Check.gameObject.SetActive(true);
                ipf_Join_Cert.readOnly = false;
                Manager.instance.manager_Common.Stop_Loading();
            }));
        }

        /// <summary>
        /// ���� Ȯ���ϱ�
        /// </summary>
        /// <returns></returns>
        public void OnClickButton_Join_Cert_Check()
        {
            if (b_Join_Cert_Complete) return;
            if (string.IsNullOrWhiteSpace(ipf_Join_Cert.text) || (!string.IsNullOrWhiteSpace(ipf_Join_Cert.text) && ipf_Join_Cert.text.Length < 6))
            {
                Manager.instance.manager_Common.Show_Alert0("6�ڸ��� ������ȣ�� �Է��� �ּ���.");
                return;
            }

            Manager.instance.manager_Common.Start_Loading();
            Timing.RunCoroutine(Manager.instance.manager_Web.Cor_Cert_Check(ipf_Join_Account.text, ipf_Join_Cert.text, (string _return) =>
            {
                JsonData _json = JsonMapper.ToObject(_return);
                bool result = bool.Parse(_json["result"].ToString());
                if (result)
                {
                    // Todo..
                    Debug.Log("���� ����");
                    Manager.instance.manager_Common.Show_Alert0("������ �Ϸ�Ǿ����ϴ�.");
                    btn_Join_Cert_Check.interactable = btn_Join_Cert_Send.interactable = false;
                    ipf_Join_Account.readOnly = ipf_Join_Cert.readOnly = true;
                    ipf_Join_Pwd.readOnly = ipf_Join_Pwd2.readOnly = false;
                    Timing.RunCoroutine(Manager.instance.manager_Ui.Cor_Scale_Anchored(rect_Join_Pwd, Vector3.one, 10));
                    b_Join_Cert_Complete = true;
                    Manager.instance.manager_Common.Select_Inputfield(ipf_Join_Pwd);
                }
                else
                {
                    Debug.Log("���� ����");
                    Manager.instance.manager_Common.Show_Alert0("������ȣ�� �ǹٸ��� �ʽ��ϴ�.");
                }
                Manager.instance.manager_Common.Stop_Loading();
            }));
        }

        /// <summary>
        /// ��й�ȣ �Է� ��
        /// </summary>
        /// <param name="_value"></param>
        public void OnValueChanged_Join_Pwd2(string _value)
        {
            text_Join_Pwd2.text = ConvertPassword(_value);
            ipf_Join_Pwd2.caretPosition = b_Join_Pwd2_Show ? ipf_Join_Pwd2.text.Length : text_Join_Pwd2.text.Length;
        }

        /// <summary>
        /// �� Ŭ�� ��
        /// </summary>
        /// <param name="_show"></param>
        public void OnClickButton_Join_EyePwd2()
        {
            OnClickButton_Eye(ref b_Join_Pwd2_Show, ipf_Join_Pwd2, text_Join_Pwd2, img_Join_Pwd2_Eye);
        }

        /// <summary>
        /// ȸ������
        /// </summary>
        public void OnClickButton_Join()
        {
            if (string.IsNullOrWhiteSpace(ipf_Join_Pwd.text))
            {
                Manager.instance.manager_Common.Show_Alert0("��й�ȣ�� �Է��� �ּ���.");
                Manager.instance.manager_Common.Select_Inputfield(ipf_Join_Pwd);
                return;
            }
            if (!Manager.instance.manager_Common.Check_Regax_Pwd(ipf_Join_Pwd.text))
            {
                Manager.instance.manager_Common.Show_Alert0("�ϳ� �̻��� ����, ����, Ư�����ڸ� ������ 8~20�ڸ� �Է��� �ּ���.");
                Manager.instance.manager_Common.Select_Inputfield(ipf_Join_Pwd);
                return;
            }
            if (!ipf_Join_Pwd.text.Equals(ipf_Join_Pwd2.text))
            {
                Manager.instance.manager_Common.Show_Alert0("��й�ȣ�� ��ġ���� �ʽ��ϴ�. ���Է��� �ּ���.");
                Manager.instance.manager_Common.Select_Inputfield(ipf_Join_Pwd2);
                return;
            }
            Manager.instance.manager_Common.Start_Loading();
            Timing.RunCoroutine(Cor_Join());
        }

        IEnumerator<float> Cor_Join()
        {
            string platform = Application.platform.ToString();
#if UNITY_ANDROID
            platform = "Android";
#elif UNITY_IOS
            platform = "iOS";
#endif
            if (string.IsNullOrWhiteSpace(Manager.instance.manager_Firebase.token))
            {
                Manager.instance.manager_Firebase.GetToken();
                yield return Timing.WaitUntilTrue(() => !string.IsNullOrWhiteSpace(Manager.instance.manager_Firebase.token));
            }

            //Timing.RunCoroutine(Manager.instance.manager_Web.Cor_Member_Insert(ipf_Join_Account.text, ipf_Join_Pwd.text, platform, Manager.instance.manager_Firebase.token, (string _return) =>
            //{
            //    JsonData _json = JsonMapper.ToObject(_return);
            //    bool result = bool.Parse(_json["result"].ToString());
            //    if (result)
            //    {
            //        Debug.Log("ȸ������ ����");
            //        string id = _json["id"].ToString();
            //        string special_code = "";
            //        Manager_Member.Data_Member _member = new Manager_Member.Data_Member(id.ToString(), ipf_Join_Account.text, "", "", "", "", platform, Manager.instance.manager_Firebase.token, special_code);
            //        Set_Data_Member(_member);
            //        Manager.instance.manager_Common.Save_Regedit("ssh_m15", Ssh_m15(ipf_Join_Account.text, ipf_Join_Pwd.text));
            //        Manager.instance.manager_Common.Active_Page(Manager.instance.page_Set_Profile);
            //    }
            //    else
            //    {
            //        Manager.instance.manager_Common.Show_Alert0("�̹� ��ϵ� �̸����Դϴ�.");
            //        Debug.Log("ȸ������ ����");
            //    }
            //    Manager.instance.manager_Common.Stop_Loading();
            //}));
        }
#endregion

#region 4. Subpage_FindPwd
        [Header("Subpage_FindPwd")]
        [Tooltip("��й�ȣ ã�� ������")]
        [SerializeField] private Page page_FindPwd;
        [Tooltip("��й�ȣ ã�� ���̵�")]
        [SerializeField] private InputField ipf_FindPwd_Account;
        [Tooltip("��й�ȣ ã�� ���� ��ư")]
        [SerializeField] private Button btn_FindPwd_Cert_Send;
        [Tooltip("��й�ȣ ã�� ������ȣ")]
        [SerializeField] private InputField ipf_FindPwd_Cert;
        [Tooltip("��й�ȣ ã�� ����Ȯ�� ��ư")]
        [SerializeField] private Button btn_FindPwd_Cert_Check;

        /// <summary>
        /// ��й�ȣ ã�� ������Ʈ
        /// </summary>
        private void Update_FindPwd()
        {
            if (Manager_Common.isEnter)
            {
                if (ipf_FindPwd_Account.isFocused)
                    OnClickButton_FindPwd_Cert_Insert();
                else if (ipf_FindPwd_Cert.isFocused)
                    OnClickButton_FindPwd_Cert_Check();
            }
        }

        /// <summary>
        /// ��й�ȣ ã�� �ʱ�ȭ
        /// </summary>
        private void Init_FindPwd()
        {
            ipf_FindPwd_Account.text = ipf_FindPwd_Cert.text = "";
            ipf_FindPwd_Cert.readOnly = true;
            btn_FindPwd_Cert_Check.gameObject.SetActive(false);
            btn_FindPwd_Cert_Check.interactable = btn_FindPwd_Cert_Send.interactable = false;
        }

        /// <summary>
        /// ��й�ȣ ã�� ���� ��ǲ�ʵ� �Է½�
        /// </summary>
        /// <param name="_value"></param>
        public void OnValueChanged_FindPwd_Account(string _value)
        {
            btn_FindPwd_Cert_Send.interactable = Manager.instance.manager_Common.Check_Regax_Account(_value);
        }

        /// <summary>
        /// ���� ������
        /// </summary>
        public void OnClickButton_FindPwd_Cert_Insert()
        {
            if (string.IsNullOrWhiteSpace(ipf_FindPwd_Account.text))
            {
                Manager.instance.manager_Common.Show_Alert0("�̸����� �Է��� �ּ���.");
                return;
            }
            if (!Manager.instance.manager_Common.Check_Regax_Account(ipf_FindPwd_Account.text))
            {
                Manager.instance.manager_Common.Show_Alert0("�̸��� ���Ŀ� �°� �Է��� �ּ���.");
                return;
            }

            Manager.instance.manager_Common.Start_Loading();
            Timing.RunCoroutine(Manager.instance.manager_Web.Cor_Cert_Insert_Check_Member(ipf_FindPwd_Account.text, (string _return) =>
            {
                if(_return.Equals("false"))
                {
                    Manager.instance.manager_Common.Show_Alert0("��ϵ��� ���� �̸����Դϴ�.");
                }
                else
                {
                    JsonData _json = JsonMapper.ToObject(_return);
                    string cert = _json["cert"].ToString();
                    Manager.instance.manager_Common.Show_Alert0("������ȣ�� ���۵Ǿ����ϴ�.");
                    Manager.instance.manager_Email.Send_Mail(ipf_FindPwd_Account.text, Manager.instance.manager_Email.Get_Data_Mail(Manager_Email.Type_Mail.������ȣ, cert));
                    btn_FindPwd_Cert_Check.gameObject.SetActive(true);
                    ipf_FindPwd_Cert.readOnly = false;
                }
                
                Manager.instance.manager_Common.Stop_Loading();
            }));
        }

        /// <summary>
        /// ���� Ȯ���ϱ�
        /// </summary>
        /// <returns></returns>
        public void OnClickButton_FindPwd_Cert_Check()
        {
            if (string.IsNullOrWhiteSpace(ipf_FindPwd_Cert.text) || (!string.IsNullOrWhiteSpace(ipf_FindPwd_Cert.text) && ipf_FindPwd_Cert.text.Length < 6))
            {
                Manager.instance.manager_Common.Show_Alert0("6�ڸ��� ������ȣ�� �Է��� �ּ���.");
                return;
            }

            Manager.instance.manager_Common.Start_Loading();
            Timing.RunCoroutine(Manager.instance.manager_Web.Cor_Cert_Check(ipf_FindPwd_Account.text, ipf_FindPwd_Cert.text, (string _return) =>
            {
                JsonData _json = JsonMapper.ToObject(_return);
                bool result = bool.Parse(_json["result"].ToString());
                if (result)
                {
                    // Todo..
                    Manager.instance.manager_Common.Show_Alert0("������ �Ϸ�Ǿ����ϴ�.");
                    btn_FindPwd_Cert_Check.interactable = btn_FindPwd_Cert_Send.interactable = false;
                    Active_RePwd(true);
                }
                else
                {
                    Manager.instance.manager_Common.Show_Alert0("������ȣ�� �ǹٸ��� �ʽ��ϴ�.");
                }
                Manager.instance.manager_Common.Stop_Loading();
            }));
        }
#endregion

#region 5. Subpage_RePwd
        [Header("Subpage_RePwd")]
        [SerializeField] private Page page_RePwd;
        [Tooltip("��й�ȣ �缳�� ��й�ȣ")]
        [SerializeField] private InputField ipf_RePwd_Pwd;
        [Tooltip("��й�ȣ �缳�� ��й�ȣ *�� ǥ�õǴ� �ؽ�Ʈ")]
        [SerializeField] private Text text_RePwd_Pwd;
        [Tooltip("��й�ȣ �缳�� ��й�ȣ �� �̹���")]
        [SerializeField] private Image img_RePwd_Pwd_Eye;
        [Tooltip("��й�ȣ �缳�� ��й�ȣ �� Ŭ�� ����")]
        [SerializeField] private bool b_RePwd_Pwd_Show;
        [Tooltip("��й�ȣ �缳�� ��й�ȣ2")]
        [SerializeField] private InputField ipf_RePwd_Pwd2;
        [Tooltip("��й�ȣ �缳�� ��й�ȣ2 *�� ǥ�õǴ� �ؽ�Ʈ")]
        [SerializeField] private Text text_RePwd_Pwd2;
        [Tooltip("��й�ȣ �缳�� ��й�ȣ2 �� �̹���")]
        [SerializeField] private Image img_RePwd_Pwd2_Eye;
        [Tooltip("��й�ȣ �缳�� ��й�ȣ2 �� Ŭ�� ����")]
        [SerializeField] private bool b_RePwd_Pwd2_Show;

        /// <summary>
        /// ��й�ȣ ������Ʈ
        /// </summary>
        private void Update_RePwd()
        {
            if (Manager_Common.isTab)
            {
                if (ipf_RePwd_Pwd.isFocused)
                    Manager.instance.manager_Common.Select_Inputfield(ipf_RePwd_Pwd2);
            }
            if (Manager_Common.isEnter)
            {
                if (ipf_RePwd_Pwd2.isFocused)
                    OnClickButton_RePwd();
            }
        }

        /// <summary>
        /// ��й�ȣ �缳�� �ʱ�ȭ
        /// </summary>
        private void Init_RePwd()
        {
            ipf_RePwd_Pwd.text = ipf_RePwd_Pwd2.text = "";
            b_RePwd_Pwd_Show = b_RePwd_Pwd2_Show = false;
            img_RePwd_Pwd_Eye.color = img_RePwd_Pwd2_Eye.color = Manager.instance.col_C7C7C7;
        }

        /// <summary>
        /// ��й�ȣ �Է� ��
        /// </summary>
        /// <param name="_value"></param>
        public void OnValueChanged_RePwd_Pwd(string _value)
        {
            text_RePwd_Pwd.text = ConvertPassword(_value);
            ipf_RePwd_Pwd.caretPosition = b_RePwd_Pwd_Show ? ipf_RePwd_Pwd.text.Length : text_RePwd_Pwd.text.Length;
        }

        /// <summary>
        /// ��й�ȣ �� Ŭ�� ��
        /// </summary>
        /// <param name="_show"></param>
        public void OnClickButton_RePwd_EyePwd()
        {
            OnClickButton_Eye(ref b_RePwd_Pwd_Show, ipf_RePwd_Pwd, text_RePwd_Pwd, img_RePwd_Pwd_Eye);
        }

        /// <summary>
        /// ��й�ȣ2 �Է� ��
        /// </summary>
        /// <param name="_value"></param>
        public void OnValueChanged_RePwd_Pwd2(string _value)
        {
            text_RePwd_Pwd2.text = ConvertPassword(_value);
            ipf_RePwd_Pwd2.caretPosition = b_RePwd_Pwd2_Show ? ipf_RePwd_Pwd2.text.Length : text_RePwd_Pwd2.text.Length;
        }

        /// <summary>
        /// ��й�ȣ2 �� Ŭ�� ��
        /// </summary>
        /// <param name="_show"></param>
        public void OnClickButton_RePwd_EyePwd2()
        {
            OnClickButton_Eye(ref b_RePwd_Pwd2_Show, ipf_RePwd_Pwd2, text_RePwd_Pwd2, img_RePwd_Pwd2_Eye);
        }

        /// <summary>
        /// ��й�ȣ �缳��
        /// </summary>
        public void OnClickButton_RePwd()
        {
            if (string.IsNullOrWhiteSpace(ipf_RePwd_Pwd.text))
            {
                Manager.instance.manager_Common.Show_Alert0("��й�ȣ�� �Է��� �ּ���.");
                Manager.instance.manager_Common.Select_Inputfield(ipf_RePwd_Pwd);
                return;
            }
            if (!Manager.instance.manager_Common.Check_Regax_Pwd(ipf_RePwd_Pwd.text))
            {
                Manager.instance.manager_Common.Show_Alert0("�ϳ� �̻��� ����, ����, Ư�����ڸ� ������ 8~20�ڸ� �Է��� �ּ���.");
                Manager.instance.manager_Common.Select_Inputfield(ipf_RePwd_Pwd);
                return;
            }
            if (!ipf_RePwd_Pwd.text.Equals(ipf_RePwd_Pwd2.text))
            {
                Manager.instance.manager_Common.Show_Alert0("��й�ȣ�� ��ġ���� �ʽ��ϴ�. ���Է��� �ּ���.");
                Manager.instance.manager_Common.Select_Inputfield(ipf_RePwd_Pwd2);
                return;
            }

            Manager.instance.manager_Common.Start_Loading();
            Timing.RunCoroutine(Manager.instance.manager_Web.Cor_Member_Update_Password(ipf_FindPwd_Account.text, ipf_RePwd_Pwd.text, () =>
            {
                Manager.instance.manager_Common.Save_Regedit("ssh_m15", Ssh_m15(ipf_FindPwd_Account.text, ipf_RePwd_Pwd.text));
                Manager.instance.manager_Common.Active_Page(Manager.instance.page_Set_Profile);
            }));
        }
#endregion
    }
}