using LitJson;
using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NORK
{
    public class Page_Setting : Page
    {
        internal override void OnEnable()
        {
            base.OnEnable();
            Init();
        }

        internal override void Start()
        {
            base.Start();
        }

        private void Update()
        {
            if (Manager_Common.isBack)
                OnClickButton_Close();
        }

        private void Init()
        {
            if(!member) member = Manager.instance.manager_Member;
            if(!setting) setting = Manager.instance.manager_Setting;
            Init_Profile();
            Init_Language();
            Init_Color();
            Init_Promise();
        }

        #region 공통
        private Manager_Member member;
        private Manager_Setting setting;

        /// <summary>
        /// 페이지 닫기
        /// </summary>
        public void OnClickButton_Close()
        {
            Active(false);
        }
        #endregion

        #region 프로필 설정
        [Header("프로필 설정")]
        [Tooltip("프로필 설정")]         [SerializeField] private RectTransform rect_Profile;
        [Tooltip("프로필 설정 세팅")]    [SerializeField] private RectTransform rect_Profile_Setting;
        [Tooltip("프로필 설정 화살표")]  [SerializeField] private RectTransform rect_Profile_Arrow;
        [Tooltip("프로필 설정 저장")]    [SerializeField] private RectTransform rect_Profile_Save;
        [Tooltip("프로필 설정 Enable")]  [SerializeField] private bool b_Profile_Enable;

        [Tooltip("캐릭터 리스트")]       [SerializeField] private List<Prefab_Box_Character> character_List;
        [Tooltip("현재 선택된 캐릭터")]  [SerializeField] private Prefab_Box_Character cur_Character;
        [Tooltip("캐릭터 이름")]         [SerializeField] private InputField ipf_Profile_Nickname;
        [Tooltip("캐릭터 이름 언더바")]  [SerializeField] private RectTransform rect_Name_UnderLine;

        /// <summary>
        /// 프로필 설정 초기화
        /// </summary>
        private void Init_Profile()
        {
            OnSelect_Character(character_List.Find(n => n.get_Char_Name.ToString().Equals(member.Get_Data_Member.character)));
            ipf_Profile_Nickname.text = member.Get_Data_Member.nickname;
            OnSelect_Ipf_Name();
        }

        /// <summary>
        /// 프로필 설정 클릭 업
        /// </summary>
        public void OnClickButton_Profile()
        {
            b_Profile_Enable = !b_Profile_Enable;
            Enable_Profile(b_Profile_Enable);
        }

        CoroutineHandle cor_Enable_Profile;
        /// <summary>
        /// 프로필 설정 켜기 / 닫기
        /// </summary>
        /// <param name="_enable"></param>
        private void Enable_Profile(bool _enable)
        {
            rect_Profile_Arrow.localEulerAngles = new Vector3(0, 0, _enable ? 180 : 0);
            Manager_Common.StartCoroutine(ref cor_Enable_Profile, Manager.instance.manager_Ui.Cor_Rect_Anchored(rect_Profile, new Vector2(936, _enable ? 844 : 240), 10, null, Profile_Size, Profile_Size));

            void Profile_Size()
            {
                rect_Profile_Setting.sizeDelta = new Vector2(0, rect_Profile.rect.height - 240);
            }
        }

        /// <summary>
        /// 캐릭터 선택 시
        /// </summary>
        public void OnSelect_Character(Prefab_Box_Character prefab_Box_Character)
        {
            if (cur_Character)
                cur_Character.DeSelect();
            cur_Character = prefab_Box_Character;
            cur_Character.Select();
            Profile_Is_Save();
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
            if (string.IsNullOrWhiteSpace(ipf_Profile_Nickname.text))
            {
                Manager_Common.StartCoroutine(ref cor_Ipf_Name, Manager.instance.manager_Ui.Cor_Scale_Anchored(rect_Name_UnderLine, new Vector3(0, 1, 1), 10));
            }
        }

        /// <summary>
        /// 닉네임 변경 시
        /// </summary>
        public void OnValueChanged_Ipf_Name()
        {
            Profile_Is_Save();
        }

        /// <summary>
        /// 저장버튼 활성화 여부
        /// </summary>
        private void Profile_Is_Save()
        {
            bool isShowSave = !(member.Get_Data_Member.nickname.Equals(ipf_Profile_Nickname.text) && member.Get_Data_Member.character.Equals(cur_Character.get_Char_Name.ToString()));
            rect_Profile_Save.gameObject.SetActive(isShowSave);
            rect_Profile_Arrow.gameObject.SetActive(!isShowSave);
        }

        /// <summary>
        /// 프로필 설정 저장
        /// </summary>
        public void OnClickButton_Profile_Save()
        {
            if(string.IsNullOrWhiteSpace(ipf_Profile_Nickname.text))
            {
                Manager.instance.manager_Common.Show_Alert0("이름을 입력해 주세요.");
                return;
            }

            Manager.instance.manager_Common.Start_Loading();

            Manager.instance.manager_Common.Profile_Nickname_Character_Save(ipf_Profile_Nickname.text, cur_Character.get_Char_Name.ToString(), ()=> {

                rect_Profile_Save.gameObject.SetActive(false);
                rect_Profile_Arrow.gameObject.SetActive(true);
                Manager.instance.manager_Common.Stop_Loading();

            });
        }
        #endregion

        #region 언어 설정
        [Header("언어 설정")]
        [Tooltip("언어 설정")] [SerializeField] private RectTransform rect_Language;
        [Tooltip("언어 설정 세팅")] [SerializeField] private RectTransform rect_Language_Setting;
        [Tooltip("언어 설정 화살표")] [SerializeField] private RectTransform rect_Language_Arrow;
        [Tooltip("언어 설정 저장")] [SerializeField] private RectTransform rect_Language_Save;
        [Tooltip("언어 설정 Enable")] [SerializeField] private bool b_Language_Enable;
        [Tooltip("언어 설정")] [SerializeField] private Dropdown drop_Language;


        /// <summary>
        /// 언어 설정 초기화
        /// </summary>
        private void Init_Language()
        {
            switch (setting.data_Setting.language)
            {
                case "kr":
                    drop_Language.value = 0;
                    break;
                case "en":
                    drop_Language.value = 1;
                    break;
            }
            drop_Language.captionText.text = drop_Language.options[drop_Language.value].text;
        }

        /// <summary>
        /// 언어 설정 클릭 업
        /// </summary>
        public void OnClickButton_Language()
        {
            b_Language_Enable = !b_Language_Enable;
            Enable_Language(b_Language_Enable);
        }

        CoroutineHandle cor_Enable_Language;
        /// <summary>
        /// 언어 설정 켜기 / 닫기
        /// </summary>
        /// <param name="_enable"></param>
        private void Enable_Language(bool _enable)
        {
            rect_Language_Arrow.localEulerAngles = new Vector3(0, 0, _enable ? 180 : 0);
            Manager_Common.StartCoroutine(ref cor_Enable_Language, Manager.instance.manager_Ui.Cor_Rect_Anchored(rect_Language, new Vector2(936, _enable ? 500 : 240), 10, null, Language_Size, Language_Size));

            void Language_Size()
            {
                rect_Language_Setting.sizeDelta = new Vector2(0, rect_Language.rect.height - 240);
            }
        }

        public void OnValueChanged_Language()
        {
            Language_Is_Save();
        }

        /// <summary>
        /// 저장버튼 활성화 여부
        /// </summary>
        private void Language_Is_Save()
        {
            bool isShowSave = setting.data_Setting.language.Equals("kr") ? drop_Language.value.Equals(1) : drop_Language.value.Equals(0);
            rect_Language_Save.gameObject.SetActive(isShowSave);
            rect_Language_Arrow.gameObject.SetActive(!isShowSave);
        }

        /// <summary>
        /// 언어 설정 저장
        /// </summary>
        public void OnClickButton_Language_Save()
        {
            setting.Save_Language(drop_Language.value.Equals(0) ? "kr" : "en");
            Manager.instance.manager_Language.Change_Prefab_Text_Lang();
            rect_Language_Save.gameObject.SetActive(false);
            rect_Language_Arrow.gameObject.SetActive(true);
        }
        #endregion

        #region 컬러 설정
        [Header("컬러 설정")]
        [Tooltip("컬러 설정")] [SerializeField] private RectTransform rect_Color;
        [Tooltip("컬러 설정 세팅")] [SerializeField] private RectTransform rect_Color_Setting;
        [Tooltip("컬러 설정 화살표")] [SerializeField] private RectTransform rect_Color_Arrow;
        [Tooltip("컬러 설정 저장")] [SerializeField] private RectTransform rect_Color_Save;
        [Tooltip("컬러 설정 Enable")] [SerializeField] private bool b_Color_Enable;
        [Tooltip("컬러 설정 체크")] [SerializeField] private List<GameObject> go_Color_Checks;
        [Tooltip("컬러 설정 현재 선택된 컬러 체크")] [SerializeField] private GameObject go_Color_Cur_Check;
        [Tooltip("컬러 설정 현재 선택된 컬러 체크 인덱스")] [SerializeField] private int int_Color_Index;

        /// <summary>
        /// 컬러 설정 초기화
        /// </summary>
        private void Init_Color()
        {
            int_Color_Index = setting.data_Setting.color; ;
            for (int i = 0; i < go_Color_Checks.Count; i++)
            {
                if (int_Color_Index.Equals(i))
                {
                    go_Color_Checks[i].SetActive(true);
                    go_Color_Cur_Check = go_Color_Checks[i];
                }
                else
                    go_Color_Checks[i].SetActive(false);
            }
        }

        /// <summary>
        /// 컬러 설정 클릭 업
        /// </summary>
        public void OnClickButton_Color()
        {
            b_Color_Enable = !b_Color_Enable;
            Enable_Color(b_Color_Enable);
        }

        CoroutineHandle cor_Enable_Color;
        /// <summary>
        /// 컬러 설정 켜기 / 닫기
        /// </summary>
        /// <param name="_enable"></param>
        private void Enable_Color(bool _enable)
        {
            rect_Color_Arrow.localEulerAngles = new Vector3(0, 0, _enable ? 180 : 0);
            Manager_Common.StartCoroutine(ref cor_Enable_Color, Manager.instance.manager_Ui.Cor_Rect_Anchored(rect_Color, new Vector2(936, _enable ? 500 : 240), 10, null, Color_Size, Color_Size));

            void Color_Size()
            {
                rect_Color_Setting.sizeDelta = new Vector2(0, rect_Color.rect.height - 240);
            }
        }

        /// <summary>
        /// 컬러 선택
        /// </summary>
        /// <param name="_index"></param>
        public void OnClickButton_Color(int _index)
        {
            if (go_Color_Cur_Check != null)
                go_Color_Cur_Check.SetActive(false);
            go_Color_Cur_Check = go_Color_Checks[_index];
            go_Color_Cur_Check.SetActive(true);
            int_Color_Index = _index;
            
            Color_Is_Save();
        }

        /// <summary>
        /// 저장버튼 활성화 여부
        /// </summary>
        private void Color_Is_Save()
        {
            bool isShowSave = !setting.data_Setting.color.Equals(int_Color_Index);
            rect_Color_Save.gameObject.SetActive(isShowSave);
            rect_Color_Arrow.gameObject.SetActive(!isShowSave);
        }

        /// <summary>
        /// 컬러 설정 저장
        /// </summary>
        public void OnClickButton_Color_Save()
        {
            setting.Save_Color(int_Color_Index);

            //페이지 색상 재설정
            Set_Color();
            Manager.instance.page_Start.Set_Color();
            Manager.instance.page_Set_Profile.Set_Color();
            Manager.instance.page_Calender.Set_Color();
            Manager.instance.page_Todo.Set_Color();
            Manager.instance.page_Record.Set_Color();

            // 캘린더 프리팹 색상 재설정
            Manager.instance.page_Calender.Set_Color_Prefab_Days();

            rect_Color_Save.gameObject.SetActive(false);
            rect_Color_Arrow.gameObject.SetActive(true);
        }
        #endregion

        #region 알림 멘트 설정
        [Header("알림 멘트 설정")]
        [Tooltip("알림 멘트 설정")] [SerializeField] private RectTransform rect_Promise;
        [Tooltip("알림 멘트 설정 세팅")] [SerializeField] private RectTransform rect_Promise_Setting;
        [Tooltip("알림 멘트 설정 화살표")] [SerializeField] private RectTransform rect_Promise_Arrow;
        [Tooltip("알림 멘트 설정 저장")] [SerializeField] private RectTransform rect_Promise_Save;
        [Tooltip("알림 멘트 설정 Enable")] [SerializeField] private bool b_Promise_Enable;

        [Tooltip("알림 멘트 이름")] [SerializeField] private InputField ipf_Promise;
        [Tooltip("알림 멘트 이름 언더바")] [SerializeField] private RectTransform rect_Promise_UnderLine;
        /// <summary>
        /// 알림 멘트 설정 초기화
        /// </summary>
        private void Init_Promise()
        {
            ipf_Promise.text = member.Get_Data_Member.promise;
            if (!string.IsNullOrWhiteSpace(ipf_Promise.text))
                OnSelect_Ipf_Promise();
            else
                OnDeselect_Ipf_Promise();
        }

        /// <summary>
        /// 알림 멘트 설정 클릭 업
        /// </summary>
        public void OnClickButton_Promise()
        {
            b_Promise_Enable = !b_Promise_Enable;
            Enable_Promise(b_Promise_Enable);
        }

        CoroutineHandle cor_Enable_Promise;
        /// <summary>
        /// 알림 멘트 설정 켜기 / 닫기
        /// </summary>
        /// <param name="_enable"></param>
        private void Enable_Promise(bool _enable)
        {
            rect_Promise_Arrow.localEulerAngles = new Vector3(0, 0, _enable ? 180 : 0);
            Manager_Common.StartCoroutine(ref cor_Enable_Promise, Manager.instance.manager_Ui.Cor_Rect_Anchored(rect_Promise, new Vector2(936, _enable ? 500 : 240), 10, null, Promise_Size, Promise_Size));

            void Promise_Size()
            {
                rect_Promise_Setting.sizeDelta = new Vector2(0, rect_Promise.rect.height - 240);
            }
        }

        CoroutineHandle cor_Ipf_Promise;
        /// <summary>
        /// 알림 멘트 인풋필드 클릭시
        /// </summary>
        public void OnSelect_Ipf_Promise()
        {
            Manager_Common.StartCoroutine(ref cor_Ipf_Promise, Manager.instance.manager_Ui.Cor_Scale_Anchored(rect_Promise_UnderLine, Vector3.one, 10));
        }

        /// <summary>
        /// 알림 멘트 인풋필드 클릭 해제시
        /// </summary>
        public void OnDeselect_Ipf_Promise()
        {
            if (string.IsNullOrWhiteSpace(ipf_Profile_Nickname.text))
            {
                Manager_Common.StartCoroutine(ref cor_Ipf_Promise, Manager.instance.manager_Ui.Cor_Scale_Anchored(rect_Promise_UnderLine, new Vector3(0, 1, 1), 10));
            }
        }

        /// <summary>
        /// 알림 멘트 변경 시
        /// </summary>
        public void OnValueChanged_Ipf_Promise()
        {
            Promise_Is_Save();
        }

        /// <summary>
        /// 저장버튼 활성화 여부
        /// </summary>
        private void Promise_Is_Save()
        {
            bool isShowSave = !member.Get_Data_Member.promise.Equals(ipf_Promise.text);
            rect_Promise_Save.gameObject.SetActive(isShowSave);
            rect_Promise_Arrow.gameObject.SetActive(!isShowSave);
        }

        /// <summary>
        /// 프로필 설정 저장
        /// </summary>
        public void OnClickButton_Promise_Save()
        {
            if (string.IsNullOrWhiteSpace(ipf_Promise.text))
            {
                Manager.instance.manager_Common.Show_Alert0("알림 멘트를 입력해 주세요.");
                return;
            }

            Manager.instance.manager_Common.Profile_Promise_Save(ipf_Promise.text, () => {

                rect_Promise_Save.gameObject.SetActive(false);
                rect_Promise_Arrow.gameObject.SetActive(true);

            });
        }
        #endregion

    }
}