using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MEC;

namespace NORK
{
    public class Manager_Language : MonoBehaviour
    {
        /// <summary>
        /// 언어
        /// </summary>
        public enum language { kr, en }

        /// <summary>
        /// 현재 설정된 언어
        /// </summary>
        public static language lang = language.kr;

#if UNITY_EDITOR
        public language unity_Lang;

        private void Update()
        {
            unity_Lang = lang;
        }
#endif

        /// <summary>
        /// 언어에 따른 텍스트 가져오기
        /// </summary>
        /// <param name="_kr"></param>
        /// <param name="_lang"></param>
        /// <returns></returns>
        public static string Get_Text(string _kr, language _lang, params string[] _str)
        {
            string _return = "";
            if (_lang.Equals(language.kr))
                _return =  _kr;
            else
            {
                int _index = data_Languages.FindIndex(n => n.kr.Equals(_kr));
                if (!_index.Equals(-1))
                {
                    switch (_lang)
                    {
                        default:
                            _return = data_Languages[_index].kr;
                            break;
                        case language.en:
                            _return = data_Languages[_index].en;
                            break;
                    }
                }
                else
                    _return = _kr;
            }
            for (int i = 0; i < _str.Length; i++)
            {
                _return = _return.Replace($"{{{i}}}", _str[i]);
            }
            return _return;
        }

        /// <summary>
        /// 언어 번역해주기
        /// </summary>
        /// <param name="_key">Text</param>
        /// <param name="_customlang">원하는 언어 반환 여부</param>
        /// <param name="_lang">원하는 언어</param>
        /// <returns></returns>
        public static string Get_Text(string _key, params string[] _obj)
        {
            int _index = data_Languages.FindIndex(n => n.kr.Equals(_key));
            string _return = "";
            if (!_index.Equals(-1))
            {
                switch (lang)
                {
                    case language.kr:
                        _return = data_Languages[_index].kr;
                        break;
                    case language.en:
                    default:
                        _return = data_Languages[_index].en;
                        break;
                }
            }
            else
            {
                _return = _key;
            }
            for (int i = 0; i < _obj.Length; i++)
            {
                _return = _return.Replace($"{{{i}}}", _obj[i]);
            }
            return _return;
        }

        /// <summary>
        /// 언어별 텍스트를 가지고 있는 구조체
        /// </summary>
        public struct Data_Language
        {
            public string kr;
            public string en;

            public Data_Language(string _kr, string _en)
            {
                kr = _kr;
                en = _en;
            }
        }

        public List<Prefab_Text> prefab_Texts = new List<Prefab_Text>();
        public void Change_Prefab_Text_Lang()
        {
            for (int i = 0; i < prefab_Texts.Count; i++)
            {
                prefab_Texts[i].ChangeText();
            }
        }
        [ContextMenu("Remove")]
        public void Test()
        {
            prefab_Texts.RemoveAll(n => n == null);
        }
        
        /// <summary>
        /// 사용되는 모든 언어를 포함하고 있는 리스트
        /// </summary>
        public static List<Data_Language> data_Languages = new List<Data_Language>()
        {
            new Data_Language {kr = "버전 체크 중", en = "Checking the version"},
            new Data_Language {kr = "업데이트 후 이용해 주세요.", en = "Please use it after updating."},

            new Data_Language {kr = "환영합니다.\n#Selftodo의\n회원이신가요?", en = "Welcome.\nAre you a member of\n#Selfodo?"},
            new Data_Language {kr = "환영합니다.<br>#Selftodo의<br>회원이신가요?", en = "Welcome.<br>Are you a member of<br>#Selfodo?"},
            new Data_Language {kr = "회원가입 시, <b>이용약관</b>과 <b>개인정보처리방침</b>,<b>마케팅알림받기</b>에 동의한 것으로 간주합니다.", en = "When signing up as a member, it is considered that you have agreed to <b> Terms of Use</b> and <b> Privacy Policy</b> and <b> Marketing Notification</b>."},
            new Data_Language {kr = "네, 로그인 할래요.", en = "Yes, I want to log in."},
            new Data_Language {kr = "아니요, 회원가입 할래요.", en = "No, I want to sign up."},
            new Data_Language {kr = "아이디", en = "Account"},
            new Data_Language {kr = "비밀번호", en = "Password"},
            new Data_Language {kr = "로그인", en = "Login"},
            new Data_Language {kr = "아이디 또는 비밀번호를 확인해 주세요.", en = "Please check your account or password."},
            new Data_Language {kr = "비밀번호 찾기", en = "Find password"},
            new Data_Language {kr = "회원가입", en = "Join"},
            new Data_Language {kr = "이메일 입력", en = "Email"},
            new Data_Language {kr = "아이디 입력", en = "Account"},
            new Data_Language {kr = "인증번호", en = "Authentication number"},
            new Data_Language {kr = "완료", en = "Complet"},
            new Data_Language {kr = "비밀번호 입력", en = "Enter password"},
            new Data_Language {kr = "비밀번호 재입력", en = "Re-enter password"},
            new Data_Language {kr = "인증", en = "Authentication"},
            new Data_Language {kr = "비밀번호 재설정", en = "Reset password"},
            new Data_Language {kr = "캐릭터를 선택해 주세요.", en = "Please select character."},
            new Data_Language {kr = "아이디를 입력해 주세요.", en = "Please enter account"},
            new Data_Language {kr = "이메일을 입력해 주세요.", en = "Please enter email"},
            new Data_Language {kr = "비밀번호를 입력해 주세요.", en = "Please enter password"},
            new Data_Language {kr = "이메일 형식에 맞게 입력해 주세요.", en = "Please enter it according to the email format."},
            new Data_Language {kr = "하나 이상의 문자, 숫자, 특수문자를 포함한 8~20자를 입력해 주세요.", en = "Please enter 8-20 characters, including at least one letter, number, and special character."},
            new Data_Language {kr = "인증번호를 입력해 주세요.", en = "Please enter your authentication number."},
            new Data_Language {kr = "6자리의 인증번호를 입력해 주세요.", en = "Please enter a 6-digit authentication number."},
            new Data_Language {kr = "인증번호가 옳바르지 않습니다.", en = "The authentication number is not correct."},
            new Data_Language {kr = "인증이 완료되었습니다.", en = "The authentication has been completed."},
            new Data_Language {kr = "이미 등록된 이메일입니다.", en = "This email is already registered."},
            new Data_Language {kr = "등록되지 않은 이메일입니다.", en = "This is an unregistered email."},
            new Data_Language {kr = "인증번호가 전송되었습니다.", en = "The authentication number has been sent."},
            new Data_Language {kr = "이름을 입력해 주세요.", en = "Please enter your name."},

            new Data_Language {kr = "Step 1. 아바타를 선택해주세요.", en = "Step 1. Choose an avatar."},
            new Data_Language {kr = "Step 2. 이름을 입력해주세요.", en = "Step 2. Please enter your name."},
            new Data_Language {kr = "Step 3. 다짐을 입력해주세요.", en = "Step 3. Please enter your resolution."},
            new Data_Language {kr = "이름", en = "Name"},
            new Data_Language {kr = "예) 지나간 시간은 돌아오지 않는다.", en = "ex) Time that has passed does not return."},
            new Data_Language {kr = "{0} 님,\n#Selftodo를\n시작해볼까요?", en = "{0},\nshould we start\n #Selfodo?"},
            new Data_Language {kr = "네, 시작할게요!", en = "Yes, let's get started!"},
            new Data_Language {kr = "아니요, 돌아갈래요.", en = "No, I want to go back."},
            new Data_Language {kr = "일", en = "S"},
            new Data_Language {kr = "월", en = "M"},
            new Data_Language {kr = "화", en = "T"},
            new Data_Language {kr = "수", en = "W"},
            new Data_Language {kr = "목", en = "T"},
            new Data_Language {kr = "금", en = "F"},
            new Data_Language {kr = "토", en = "S"},
            new Data_Language {kr = "편집", en = "Editing"},
            new Data_Language {kr = "할일 편집", en = "Editing Todo"},
            new Data_Language {kr = "이름 입력", en = "Enter name"},
            new Data_Language {kr = "할일 삭제", en = "Delete Todo"},
            new Data_Language {kr = "내용을 입력해주세요.", en = "Please enter the contents."},
            new Data_Language {kr = "사진 촬영하기", en = "Taking pictures."},
            new Data_Language {kr = "사진 가져오기", en = "Bring a picture."},
            new Data_Language {kr = "지우기", en = "Delete"},
            new Data_Language {kr = "수정하기", en = "Edit"},
            new Data_Language {kr = "저장", en = "Save"},
            new Data_Language {kr = "알림 멘트", en = "Reminder"},

            //Page_Calender
            new Data_Language {kr = "할일", en = "Todo"},

            new Data_Language {kr = "1월", en = "Jan."},
            new Data_Language {kr = "2월", en = "Feb."},
            new Data_Language {kr = "3월", en = "Mar."},
            new Data_Language {kr = "4월", en = "Apr."},
            new Data_Language {kr = "5월", en = "May"},
            new Data_Language {kr = "6월", en = "Jun."},
            new Data_Language {kr = "7월", en = "Jul."},
            new Data_Language {kr = "8월", en = "Aug."},
            new Data_Language {kr = "9월", en = "Sep."},
            new Data_Language {kr = "10월", en = "Oct."},
            new Data_Language {kr = "11월", en = "Nov."},
            new Data_Language {kr = "12월", en = "Dec."},
            new Data_Language {kr = "{0}년 {1}월 {2}일", en = "{0}-{1}-{2}"},
            new Data_Language {kr = "할 일 추가", en = "Add Todo"},
            new Data_Language {kr = "할 일 편집", en = "Edit Todo"},
            new Data_Language {kr = "할 일을 입력해 주세요.", en = "Please enter todo name"},
            new Data_Language {kr = "체크하기", en = "Check"},
            new Data_Language {kr = "체크해제", en = "Uncheck"},
            new Data_Language {kr = "기록물 작성", en = "Writing a record"},
            new Data_Language {kr = "기록물 수정", en = "Revise the record"},

            new Data_Language {kr = "매일", en = "Everyday."},
            new Data_Language {kr = "평일", en = "Weekday"},
            new Data_Language {kr = "주말", en = "Weekend"},

            //Page_Setting
            new Data_Language {kr = "프로필 설정", en = "Profile setting"},
            new Data_Language {kr = "언어 설정", en = "Language setting"},
            new Data_Language {kr = "컬러 설정", en = "Color setting"},
            new Data_Language {kr = "알림 멘트 설정", en = "Promise setting"},
            new Data_Language {kr = "알림 멘트를 입력해 주세요.", en = "Please enter promise"},

            new Data_Language {kr = "사진은 {0}장까지 추가 가능합니다.", en = "Up to {0} pictures can be added."},
        };

        #region Google Spread Sheet
        [System.Serializable]
        public struct Data_Google_SpreadSheet
        {
            public Data_Table table;
        }

        public Data_Google_SpreadSheet google_SpreadSheet;

        [System.Serializable]
        public struct Data_Table
        {
            //public List<Data_Cols> cols;
            public List<Data_Rows> rows;
        }

        [System.Serializable]
        public struct Data_Cols
        {
            public string label;
        }


        [System.Serializable]
        public struct Data_Rows
        {
            public List<Data_C> c;
        }

        [System.Serializable]
        public struct Data_C
        {
            public string v;
        }
        IEnumerator<float> Get_Lang()
        {
            data_Languages = new List<Data_Language>();

            UnityWebRequest www = UnityWebRequest.Get("https://docs.google.com/spreadsheets/d/1yCSkTwSWuLUxCi0Cr8ubcfXfrjrNJVp-UxUJk8DUT-U/gviz/tq?tqx=out:json");
            yield return Timing.WaitUntilDone(www.SendWebRequest());

            string _text = www.downloadHandler.text.Substring(47);
            _text = _text.Remove(_text.Length - 2);
            Debug.Log(_text);

            google_SpreadSheet = JsonUtility.FromJson<Data_Google_SpreadSheet>(_text);

            for (int i = 0; i < google_SpreadSheet.table.rows.Count; i++)
            {
                Data_Rows _rows = google_SpreadSheet.table.rows[i];
                data_Languages.Add(new Data_Language(_rows.c[1].v, _rows.c[2].v));
            }
        }
        #endregion
    }
}