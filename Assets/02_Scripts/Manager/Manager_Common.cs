using MEC;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Formatters.Binary;
using LitJson;

namespace NORK
{
    public class Manager_Common : MonoBehaviour
    {
        private void Awake()
        {
            Set_Ratio_Screen();
        }

        #region 시작페이지 알림
        [Header("앱 버전 체크")]
        public GameObject go_Start_Page_Alert;
        public Text text_Start_Page_Alert;
        #endregion

        #region 앱 버전
        /// <summary>
        /// 앱 버전 체크
        /// </summary>
        /// <param name="_action"></param>
        public void Check_Version(UnityAction _action)
        {
            go_Start_Page_Alert.SetActive(true);
            text_Start_Page_Alert.gameObject.SetActive(true);
            Timing.RunCoroutine(Manager.instance.manager_Web.Cor_Check_Version((string _return) => {

                JsonData _json = JsonMapper.ToObject(_return);

                int cur_Version = int.Parse(Application.version.Replace(".", ""));
                int new_Version = int.Parse(_json["version"].ToString());

                if (new_Version > cur_Version)
                {
                    text_Start_Page_Alert.text = Manager_Language.Get_Text("업데이트 후 이용해 주세요.");
                    Application.OpenURL(_json["path"].ToString());
                }
                else
                {
                    _action?.Invoke();
                    go_Start_Page_Alert.SetActive(false);
                }
            }));
        }
        #endregion

        #region 화면
        [Header("화면")]
        public RectTransform rect_CanvasMain;
        public float ratio;
        [ContextMenu("Set_Ratio_Screen")]
        /// <summary>
        /// 화면 비율
        /// </summary>  
        public void Set_Ratio_Screen()
        {
            Timing.RunCoroutine(Cor_Set_Ratio_Screen());
        }

        IEnumerator<float> Cor_Set_Ratio_Screen()
        {
            yield return Timing.WaitForSeconds(1);
            ratio = rect_CanvasMain.localScale.x;
        }
        #endregion


        #region 키 입력
        public static bool isTab;
        public static bool isEnter;
        public static bool isBack;

        private void Update()
        {
            isTab = Input.GetKeyDown(KeyCode.Tab);
            isEnter = Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter);
            isBack = Input.GetKeyDown(KeyCode.Escape);

            HotKey();
        }
        #endregion

        #region 단축키
        /// <summary>
        /// 단축키
        /// </summary>
        private void HotKey()
        {

        }
        #endregion

        #region 종료
        public void Start_Quit()
        {
            if (cor_Quit.IsRunning) return;
            cor_Quit = Timing.RunCoroutine(Cor_Quit());
        }

        CoroutineHandle cor_Quit;
        IEnumerator<float> Cor_Quit()
        {
            DateTime start = DateTime.Now;
            while ((DateTime.Now - start).TotalSeconds < 3)
            {
                if (isBack)
                {
                    Quit();
                    yield break;
                }
                yield return Timing.WaitForSeconds(0);
            }
        }

        /// <summary>
        /// 종료
        /// </summary>
        public void Quit()
        {
            Application.Quit();
        }
        #endregion

        #region 모드
        #region 솔로 / 멀티 모드
        /// <summary>
        /// 솔로 / 멀티 모드
        /// </summary>
        public enum Mode { solo, multi }

        /// <summary>
        /// 현재 선택된 모드
        /// </summary>
        [Header("현재 모드")]
        public Mode mode;
        #endregion

        #region 페이지 모드
        public enum Page_Mode { Start, Set_Profile, Calender, Todo, Record, Setting }
        #endregion
        #endregion

        #region 캐릭터
        /// <summary>
        /// 캐릭터 이름
        /// </summary>
        public enum Character_Name
        {
            komi,
            arthur,
            black_boy,
            black_girl,
            health_boy,
            health_girl,
            oing_boy,
            oing_girl,
            sensitive_boy,
            sensitive_girl,
            tired_boy,
            tired_girl
        }

        /// <summary>
        /// 캐릭터 종류
        /// </summary>
        [Serializable]
        public struct Data_Character
        {
            public Character_Name name;
            public Sprite img;
        }

        /// <summary>
        /// 캐릭터 종류 리스트
        /// </summary>
        [Header("캐릭터")]
        public List<Data_Character> data_Characters = new List<Data_Character>();
        #endregion

        #region UI 등장 효과
        /// <summary>
        /// 디졸브 등장 효과
        /// </summary>
        public enum UI_Dissolve {
            True,
            False
        }

        /// <summary>
        /// 움직임 등장 효과
        /// </summary>
        public enum UI_Move
        {
            None,
            LeftToCenter,
            RightToCenter,
            TopToCenter,
            BottomToCenter
        }

        /// <summary>
        /// 현재 켜져있는 페이지
        /// </summary>
        [Header("UI 등장 효과")]
        [SerializeField] private Page opened_Page;
        public void Active_Page(Page _new_page, bool _deActive_Opened = true, UnityAction start_Action = null, UnityAction end_Action = null)
        {
            if (_deActive_Opened)
            {
                if (opened_Page != null)
                    opened_Page.Active(false);
            }
            opened_Page = _new_page;
            _new_page.Active(true, start_Action, end_Action);
        }

        /// <summary>
        /// UI 등장 효과
        /// </summary>
        /// <param name="rect_UI"></param>
        /// <param name="cvg_UI"></param>
        /// <param name="dissolve"></param>
        /// <param name="move"></param>
        /// <param name="active"></param>
        public void Appear_Effect(GameObject go_UI, ref CoroutineHandle cor_Dissolve, ref CoroutineHandle cor_Move, UI_Dissolve dissolve, UI_Move move, bool active, UnityAction start_Action = null, UnityAction end_Action = null)
        {
            RectTransform rect_UI = go_UI.GetComponent<RectTransform>();
            CanvasGroup cvg_UI = go_UI.GetComponent<CanvasGroup>();

            if (cor_Move.IsRunning)
                Timing.KillCoroutines(cor_Move);
            switch (move)
            {
                case UI_Move.None:
                    rect_UI.anchoredPosition = Vector2.zero;
                    start_Action?.Invoke();
                    end_Action?.Invoke();
                    break;
                case UI_Move.LeftToCenter:
                    cor_Move = Timing.RunCoroutine(Manager.instance.manager_Ui.Cor_Pos_Anchored(rect_UI, active ? Vector2.zero : new Vector2(-Screen.width, 0), 10, () =>
                    {
                        if (active)
                        {
                            rect_UI.anchoredPosition = new Vector2(-Screen.width, 0);
                            go_UI.SetActive(true);
                        }
                        else
                        {

                        }
                        start_Action?.Invoke();
                    }, null,
                    () =>
                    {
                        if (active)
                        {

                        }
                        else
                        {
                            rect_UI.anchoredPosition = new Vector2(-Screen.width, 0);
                            go_UI.SetActive(false);
                        }
                        end_Action?.Invoke();
                    }));
                    break;
                case UI_Move.RightToCenter:
                    cor_Move = Timing.RunCoroutine(Manager.instance.manager_Ui.Cor_Pos_Anchored(rect_UI, active ? Vector2.zero : new Vector2(Screen.width, 0), 10, () =>
                    {
                        if (active)
                        {
                            rect_UI.anchoredPosition = new Vector2(Screen.width, 0);
                            go_UI.SetActive(true);
                        }
                        else
                        {

                        }
                        start_Action?.Invoke();
                    }, null,
                    () =>
                    {
                        if (active)
                        {

                        }
                        else
                        {
                            rect_UI.anchoredPosition = new Vector2(Screen.width, 0);
                            go_UI.SetActive(false);
                        }
                        end_Action?.Invoke();
                    }));
                    break;
                case UI_Move.TopToCenter:
                    cor_Move = Timing.RunCoroutine(Manager.instance.manager_Ui.Cor_Pos_Anchored(rect_UI, active ? Vector2.zero : new Vector2(0, Screen.height), 10, () =>
                    {
                        if (active)
                        {
                            rect_UI.anchoredPosition = new Vector2(0, Screen.height);
                            go_UI.SetActive(true);
                        }
                        else
                        {

                        }
                        start_Action?.Invoke();
                    }, null,
                    () =>
                    {
                        if (active)
                        {

                        }
                        else
                        {
                            rect_UI.anchoredPosition = new Vector2(0, Screen.height);
                            go_UI.SetActive(false);
                        }
                        end_Action?.Invoke();
                    }));
                    break;
                case UI_Move.BottomToCenter:
                    cor_Move = Timing.RunCoroutine(Manager.instance.manager_Ui.Cor_Pos_Anchored(rect_UI, active ? Vector2.zero : new Vector2(0, -Screen.height), 10, () =>
                    {
                        if (active)
                        {
                            rect_UI.anchoredPosition = new Vector2(0, -Screen.height);
                            go_UI.SetActive(true);
                        }
                        else
                        {

                        }
                        start_Action?.Invoke();
                    }, null,
                    () =>
                    {
                        if (active)
                        {

                        }
                        else
                        {
                            rect_UI.anchoredPosition = new Vector2(0, -Screen.height);
                            go_UI.SetActive(false);
                        }
                        end_Action?.Invoke();
                    }));
                    break;
            }

            if (cor_Dissolve.IsRunning)
                Timing.KillCoroutines(cor_Dissolve);
            switch (dissolve)
            {
                case UI_Dissolve.True:
                    cor_Dissolve = Timing.RunCoroutine(Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_UI, active, 0.05f, () =>
                    {
                        if (active)
                            go_UI.SetActive(true);
                    },
                    () =>
                    {
                        if (!active)
                            go_UI.SetActive(false);
                    }
                    ));
                    break;
                case UI_Dissolve.False:
                    if (active)
                    {
                        if (move.Equals(UI_Move.None))
                        {
                            cvg_UI.alpha = 1;
                            go_UI.SetActive(true);
                        }
                    }
                    else
                    {
                        if (move.Equals(UI_Move.None))
                        {
                            cvg_UI.alpha = 0;
                            go_UI.SetActive(false);
                        }
                    }
                    break;
            }
        }
        #endregion

        #region key, value struct
        /// <summary>
        /// key, value struct
        /// </summary>
        [Serializable]
        public struct Data_Dictionary
        {
            public string key;
            public string value;

            public Data_Dictionary(string _key, string _value)
            {
                key = _key;
                value = _value;
            }
        }

        /// <summary>
        /// Data_Dictionary를 array로 만들기
        /// </summary>
        /// <param name="_key"></param>
        /// <param name="_value"></param>
        /// <returns></returns>
        public Data_Dictionary[] Get_Data_Dictionarys(string[] _key, string[] _value)
        {
            Data_Dictionary[] dd = new Data_Dictionary[_key.Length];
            for (int i = 0; i < dd.Length; i++)
            {
                dd[i] = new Data_Dictionary(_key[i], _value[i]);
            }
            return dd;
        }
        #endregion

        #region 레지스트리
        /// <summary>
        /// 레지스트리 저장
        /// </summary>
        public void Save_Regedit(string key, string value)
        {
            Save.SaveString(key, Crypto.Encrypt(value));
        }

        /// <summary>
        /// 레지스트리 불러오기
        /// </summary>
        public string Load_Regedit(string key)
        {
            return Crypto.Decrypt(Load.LoadString(key));
        }
        #endregion

        #region 정규문자
        /// <summary>
        /// 정규문자 치환
        /// </summary>
        public static string Regular_Expression_Encryption(string _text)
        {
            return _text.Replace("\\\"", "\"").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("\'", "&#39;").Replace("´", "&acute;").Replace("[", "&#91;").Replace("]", "&#93;").Replace("{", "&#123;").Replace("}", "&#125;");
        }

        /// <summary>
        /// 정규문자 역치환
        /// </summary>
        public static string Regular_Expression_Decryption(string _text)
        {
            return _text.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"").Replace("&#39;", "\'").Replace("&acute;", "´").Replace("&#91;", "[").Replace("&#93;", "]").Replace("&#123;", "{").Replace("&#125;", "}");
        }
        #endregion

        #region 정규식
        //아이디 체크
        Regex regex_Account = new Regex(@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@" + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\." + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|" + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$");
        public bool Check_Regax_Account(string _data)
        {
            return regex_Account.IsMatch(_data);
        }

        //비밀번호 체크
        Regex regex_Pwd = new Regex(@"^(?=.*[a-zA-Z ])(?=.*[0-9])(?=.*[\W]).{8,20}$");
        public bool Check_Regax_Pwd(string _data)
        {
            return regex_Pwd.IsMatch(_data);
        }

        //휴대폰 체크
        Regex regex_Phone = new Regex(@"010-[0-9]{4}-[0-9]{4}$");
        public bool Check_Regax_Phone(string _data)
        {
            return regex_Phone.IsMatch(_data);
        }
        #endregion

        #region 로딩
        [Header("로딩")]
        public GameObject go_Loading;
        public int loading_Count;
        public void Start_Loading()
        {
            loading_Count++;
            if(loading_Count.Equals(1))
                cor_Start_Loading = Timing.RunCoroutine(Cor_Start_Loading());
        }

        CoroutineHandle cor_Start_Loading;
        IEnumerator<float> Cor_Start_Loading()
        {
            yield return Timing.WaitForSeconds(0.2f);
            go_Loading.SetActive(true);
        }

        public void Stop_Loading()
        {
            loading_Count--;
            if(loading_Count.Equals(0))
            {
                if (cor_Start_Loading.IsRunning)
                    Timing.KillCoroutines(cor_Start_Loading);
                if (go_Loading.activeSelf)
                    go_Loading.SetActive(false);
            }
        }
        #endregion

        #region 알림
        public enum Alert { alert0, alert1, alert2 }
        /// <summary>
        /// 버튼 없는 알림
        /// </summary>
        [Header("알림")]
        [SerializeField] private Prefab_Alart0 go_Pref_Alert0;
        [SerializeField] private RectTransform rect_Pref_Alart_Content;
        private Queue<Prefab_Alart0> alart0_que = new Queue<Prefab_Alart0>();
        /// <summary>
        /// Enqueue하기
        /// </summary>
        /// <param name="_alart"></param>
        public void Enqueue_Alart0(Prefab_Alart0 _alart) => alart0_que.Enqueue(_alart);
        /// <summary>
        /// 버튼 없는 알림창 띄우기
        /// </summary>
        /// <param name="_message"></param>
        public void Show_Alert0(string _message, float _showtime = 3)
        {
            Prefab_Alart0 pref = null;
            if (alart0_que.Count > 0)
                pref = alart0_que.Dequeue();
            else
                pref = Instantiate(go_Pref_Alert0, rect_Pref_Alart_Content).GetComponent<Prefab_Alart0>();
            pref.Start_Move(Manager_Language.Get_Text(_message), _showtime);
        }

        /// <summary>
        /// 확인 버튼 있는 알림
        /// </summary>
        /// <param name="_message"></param>
        public void Show_Alert1(string _message)
        {
            Debug.Log(_message);
        }

        /// <summary>
        /// 확인, 취소 버튼 있는 알림
        /// </summary>
        /// <param name="_message"></param>
        public void Show_Alert2(string _message)
        {
            Debug.Log(_message);
        }
        #endregion

        #region 클릭
        DateTime pointDown_Time;
        Vector3 pointDown_Pos;

        public void PointDown()
        {
            pointDown_Time = DateTime.Now;
            pointDown_Pos = Input.mousePosition;
        }

        public bool PointUp_OneClick()
        {
            return (DateTime.Now - pointDown_Time).TotalSeconds < 0.2f && Vector3.Distance(pointDown_Pos, Input.mousePosition) < 10;
        }

        public struct Data_Drag
        {
            public float drag_x;//드래그 x 거리
            public float drag_y;//드래그 y 거리

            public float ratio_x;//드래그 화면 비율 x
            public float ratio_y;//드래그 화면 비율 y

            public float time;//드래그 한 시간
        }

        DateTime beganDrag_Time;
        Vector2 beganDrag_Pos;

        public void BeganDrag(Vector2 position)
        {
            beganDrag_Time = DateTime.Now;
            beganDrag_Pos = position;
        }

        public Data_Drag EndDrag(Vector2 position)
        {
            Data_Drag dd = new Data_Drag();
            dd.drag_x = position.x - beganDrag_Pos.x;
            dd.drag_y = position.y - beganDrag_Pos.y;

            dd.ratio_x = dd.drag_x / Screen.width;
            dd.ratio_y = dd.drag_y / Screen.height;

            dd.time = (float)(DateTime.Now - beganDrag_Time).TotalSeconds;

            return dd;
        }
        #endregion

        #region 색상
        /// <summary>
        /// UI 색상
        /// </summary>
        [System.Serializable]
        public struct Data_Set_Color
        {
            public List<MaskableGraphic> col_1;
            public List<MaskableGraphic> col_2;
            public List<MaskableGraphic> col_3;
            public List<MaskableGraphic> col_4;
            public List<MaskableGraphic> col_5;
            public List<MaskableGraphic> col_6;
            public List<MaskableGraphic> col_7;
            public List<MaskableGraphic> col_8;
        }

        /// <summary>
        /// UI 색 변경
        /// </summary>
        [ContextMenu("색 변경")]
        public void Set_Color(Data_Set_Color set_Color)
        {
            Set_Color(set_Color.col_1, Manager.instance.col_Main);
            Set_Color(set_Color.col_2, Manager.instance.col_Main_Light);
            Set_Color(set_Color.col_3, Manager.instance.col_Main_Light_Sub);

            void Set_Color(List<MaskableGraphic> _mg, Color _col)
            {
                for (int i = 0; i < _mg.Count; i++)
                {
                    _mg[i].color = _col;
                }
            }
        }
        #endregion

        #region Toggle
        /// <summary>
        /// 토글 UI 색상
        /// </summary>
        [Serializable]
        public struct Data_Toggle_Color
        {
            public Color col_Outline;
            public Color col_Fill;
            public Color col_Circle;
        }
        #endregion

        #region 코루틴
        public static void StartCoroutine(ref CoroutineHandle coroutineHandle, IEnumerator<float> action, string _tag = "")
        {
            StopCoroutine(ref coroutineHandle);
            coroutineHandle = Timing.RunCoroutine(action, _tag);
        }

        private static void StopCoroutine(ref CoroutineHandle coroutineHandle)
        {
            if (coroutineHandle.IsRunning)
            {
                Timing.KillCoroutines(coroutineHandle);
            }
        }

        public static IEnumerator<float> Action_WaitTime(float wait_Time, UnityAction action)
        {
            yield return Timing.WaitForSeconds(wait_Time);
            action?.Invoke();
        }

        #endregion

        #region 코루틴 실행
        #region Setting
        /// <summary>
        /// 프로필 설정 저장
        /// </summary>
        public void Profile_Nickname_Character_Save(string nickname, string character, UnityAction action)
        {
            Manager_Member.Data_Member dm = Manager.instance.manager_Member.Get_Data_Member;
            
            Timing.RunCoroutine(Manager.instance.manager_Web.Cor_Member_Update_Nickname_Character(dm.id, nickname, character, (string _return) => {
                if(_return == null)
                {
                    dm.nickname = nickname;
                    dm.nickname_code = "";
                    dm.character = character;
                }
                else
                {
                    JsonData _json = JsonMapper.ToObject(_return);
                    bool nickname_changed = bool.Parse(_json["nickname_changed"].ToString());
                    dm.nickname = nickname;
                    if (nickname_changed)
                        dm.nickname_code = _json["nickname_code"].ToString();
                    dm.character = character;
                }
                
                Manager.instance.manager_Member.Set_Data_Member(dm);
                action?.Invoke();
            }));
        }

        /// <summary>
        /// 프로필 설정 저장
        /// </summary>
        public void Profile_Promise_Save(string promise, UnityAction action)
        {
            Manager_Member.Data_Member dm = Manager.instance.manager_Member.Get_Data_Member;

            dm.promise = promise;
            Manager.instance.manager_Member.Set_Data_Member(dm);
            action?.Invoke();

            Timing.RunCoroutine(Manager.instance.manager_Web.Cor_Member_Update_Promise(dm.id, promise));
        }
        #endregion
        #endregion

        #region 압축
        /// <summary>
        /// 문자열 압축
        /// </summary>
        public string Compress_Text(string _str)
        {
            byte[] _byte;

            using (var uncompressedStream = new MemoryStream(Encoding.UTF8.GetBytes(_str)))
            {
                using (var compressedStream = new MemoryStream())
                {
                    using (var compressorStream = new DeflateStream(compressedStream, System.IO.Compression.CompressionLevel.Fastest, true))
                    {
                        uncompressedStream.CopyTo(compressorStream);
                    }

                    _byte = compressedStream.ToArray();
                }
            }

            return Convert.ToBase64String(_byte);
        }

        /// <summary>
        /// 문자열 압축 풀기
        /// </summary>
        public string Decompress_Text(string _str)
        {
            byte[] _byte;

            var compressedStream = new MemoryStream(Convert.FromBase64String(_str));

            using (var decompressorStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
            {
                using (var decompressedStream = new MemoryStream())
                {
                    decompressorStream.CopyTo(decompressedStream);

                    _byte = decompressedStream.ToArray();
                }
            }

            return Encoding.UTF8.GetString(_byte);
        }
        #endregion

        #region Int To Day
        /// <summary>
        /// Int To Day
        /// </summary>
        /// <param name="_int"></param>
        /// <returns></returns>
        public string Get_Day_One_Char_Eng(int _int)
        {
            switch (_int)
            {
                case 0:
                default:
                    return "S";
                case 1:
                    return "M";
                case 2:
                    return "T";
                case 3:
                    return "W";
                case 4:
                    return "T";
                case 5:
                    return "F";
                case 6:
                    return "S";
            }
        }

        /// <summary>
        /// Int To Day
        /// </summary>
        /// <param name="_int"></param>
        /// <returns></returns>
        public string Get_Day_Three_Char_Eng(int _int)
        {
            switch (_int)
            {
                case 0:
                default:
                    return "Sun";
                case 1:
                    return "Mon";
                case 2:
                    return "Tue";
                case 3:
                    return "Wen";
                case 4:
                    return "Thu";
                case 5:
                    return "Fri";
                case 6:
                    return "Sat";
            }
        }

        /// <summary>
        /// Int To Day
        /// </summary>
        /// <param name="_int"></param>
        /// <returns></returns>
        public string Get_Day_One_Char_Kor(int _int)
        {
            switch (_int)
            {
                case 0:
                default:
                    return "일";
                case 1:
                    return "월";
                case 2:
                    return "화";
                case 3:
                    return "수";
                case 4:
                    return "목";
                case 5:
                    return "금";
                case 6:
                    return "토";
            }
        }

        /// <summary>
        /// Int To Day
        /// </summary>
        /// <param name="_int"></param>
        /// <returns></returns>
        public string Get_Day_Three_Char_Kor(int _int)
        {
            switch (_int)
            {
                case 0:
                default:
                    return "일요일";
                case 1:
                    return "월요일";
                case 2:
                    return "화요일";
                case 3:
                    return "수요일";
                case 4:
                    return "목요일";
                case 5:
                    return "금요일";
                case 6:
                    return "토요일";
            }
        }

        /// <summary>
        /// 날짜를 요일로 전환
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public int Get_Days_Int(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Monday: return 1;
                case DayOfWeek.Tuesday: return 2;
                case DayOfWeek.Wednesday: return 3;
                case DayOfWeek.Thursday: return 4;
                case DayOfWeek.Friday: return 5;
                case DayOfWeek.Saturday: return 6;
                case DayOfWeek.Sunday: return 0;
            }

            return 0;
        }
        #endregion

        #region ToDo Data To Days
        public List<string> Days_To_StringList(Manager_Data.Data_Todo _data)
        {
            List<string> _days = new List<string>();

            if(_data.sunday.Equals("1") && _data.monday.Equals("1") && _data.tuesday.Equals("1") && _data.wednesday.Equals("1") && _data.thursday.Equals("1") && _data.friday.Equals("1") && _data.saturday.Equals("1"))
            {
                _days.Add(Manager_Language.Get_Text("매일"));
                return _days;
            }
            if(_data.sunday.Equals("0") && _data.monday.Equals("1") && _data.tuesday.Equals("1") && _data.wednesday.Equals("1") && _data.thursday.Equals("1") && _data.friday.Equals("1") && _data.saturday.Equals("0"))
            {
                _days.Add(Manager_Language.Get_Text("평일"));
                return _days;
            }
            if (_data.sunday.Equals("1") && _data.monday.Equals("0") && _data.tuesday.Equals("0") && _data.wednesday.Equals("0") && _data.thursday.Equals("0") && _data.friday.Equals("0") && _data.saturday.Equals("1"))
            {
                _days.Add(Manager_Language.Get_Text("주말"));
                return _days;
            }
            if (_data.sunday.Equals("1"))
                _days.Add(Manager.instance.manager_Common.Get_Day_One_Char_Eng(0));
            if (_data.monday.Equals("1"))
                _days.Add(Manager.instance.manager_Common.Get_Day_One_Char_Eng(1));
            if (_data.tuesday.Equals("1"))
                _days.Add(Manager.instance.manager_Common.Get_Day_One_Char_Eng(2));
            if (_data.wednesday.Equals("1"))
                _days.Add(Manager.instance.manager_Common.Get_Day_One_Char_Eng(3));
            if (_data.thursday.Equals("1"))
                _days.Add(Manager.instance.manager_Common.Get_Day_One_Char_Eng(4));
            if (_data.friday.Equals("1"))
                _days.Add(Manager.instance.manager_Common.Get_Day_One_Char_Eng(5));
            if (_data.saturday.Equals("1"))
                _days.Add(Manager.instance.manager_Common.Get_Day_One_Char_Eng(6));
            return _days;
        }

        public List<int> Days_To_IntList(Manager_Data.Data_Todo _data)
        {
            List<int> _days = new List<int>();
            if (_data.sunday.Equals("1"))
                _days.Add(0);
            if (_data.monday.Equals("1"))
                _days.Add(1);
            if (_data.tuesday.Equals("1"))
                _days.Add(2);
            if (_data.wednesday.Equals("1"))
                _days.Add(3);
            if (_data.thursday.Equals("1"))
                _days.Add(4);
            if (_data.friday.Equals("1"))
                _days.Add(5);
            if (_data.saturday.Equals("1"))
                _days.Add(6);
            return _days;
        }
        #endregion

        #region Month To Name
        public string Month(int _ind)
        {
            switch (_ind)
            {
                case 1:
                default:
                    return "January";
                case 2:
                    return "February";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                case 12:
                    return "December";
            }
        }
        #endregion

        #region Inputfield
        public void Select_Inputfield(InputField ipf)
        {
            ipf.ActivateInputField();
        }
        #endregion

        #region DateTimeToString
        public string DateTimeToYMD(DateTime _datetime)
        {
            return _datetime.ToString("yyyy-MM-dd");
        }

        public string DateTimeToYM(DateTime _datetime)
        {
            return _datetime.ToString("yyyy-MM");
        }

        public string DateTimeToYMDHmS(DateTime _datetime)
        {
            return _datetime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public string DateTimeToYMDHmS2(DateTime _datetime)
        {
            return _datetime.ToString("yyyyMMddHHmmss");
        }
        #endregion

        #region CSV
        public Manager_Oddjob odd;
        [ContextMenu("Save_Lang")]
        public void Save_Lang()
        {
            //List<string> _str = new List<string>();
            //for (int i = 0; i < odd.text.Count; i++)
            //{
            //    _str.Add(odd.text[i].kr);
            //}
            //Save_Lang(_str);
        }
        public void Save_Lang(List<string> _str)
        {
            string path = $"{Application.persistentDataPath}/lang.csv";
            // Creating First row of titles manually..
            string[] rowDataTemp = new string[2];
            List<string[]> rowData = new List<string[]>();

            rowDataTemp[0] = "kr";
            rowDataTemp[1] = "en";
            rowData.Add(rowDataTemp);
            // You can add up the values in as many cells as you want.
            for (int i = 0; i < _str.Count; i++)
            {
                rowDataTemp = new string[2];
                rowDataTemp[0] = _str[i];
                rowDataTemp[1] = Manager_Language.Get_Text(_str[i]);
                rowData.Add(rowDataTemp);
            }

            string[][] output = new string[rowData.Count][];

            for (int i = 0; i < output.Length; i++)
            {
                output[i] = rowData[i];
            }

            int length = output.GetLength(0);
            string delimiter = ",";

            StringBuilder sb = new StringBuilder();

            for (int index = 0; index < length; index++)
                sb.AppendLine(string.Join(delimiter, output[index]));

            StreamWriter outStream = new StreamWriter(path, true, Encoding.GetEncoding("euc-kr"));
            outStream.WriteLine(sb);
            outStream.Close();
        }

        [ContextMenu("Get_Lang")]
        public void Get_Lang()
        {
            string path = $"{Application.persistentDataPath}/lang.csv";
            string load_data = File.ReadAllText(path, Encoding.GetEncoding("euc-kr"));

            string[] sales_data = load_data.Split('\n');
            
            for (int i = 1; i < sales_data.Length; i++)
            {
                if (string.IsNullOrEmpty(sales_data[i].Trim())) continue;
                string[] sales_data_line = sales_data[i].Split(',');
                Manager_Language.data_Languages.Add(new Manager_Language.Data_Language(sales_data_line[0], sales_data_line[1]));
            }
        }
        #endregion
    }
}