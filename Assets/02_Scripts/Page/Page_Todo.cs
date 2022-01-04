using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NORK
{
    public class Page_Todo : Page
    {
        public enum Mode { Insert, Update }

        internal override void OnEnable()
        {
            base.OnEnable();

            Init();
        }

        internal override void Start()
        {
            base.Start();
            Init_Time();
        }

        private void Update()
        {
            if (Manager_Common.isBack)
                OnClickButton_Close();
        }

        [Header("할 일")]
        [SerializeField] private Manager_Data.Data_Todo todo; public void Set_Todo(Manager_Data.Data_Todo _todo) => todo = _todo;

        [Header("모드")]
        [Tooltip("모드")]
        [SerializeField] private Mode mode; public void Set_Mode(Mode _mode) => mode = _mode;
        [Tooltip("타이틀")]
        [SerializeField] private Text text_Title;

        [Tooltip("현재 할 일 텍스트")]
        [SerializeField] private InputField ipf_Todo_Name;

        /// <summary>
        /// 초기화
        /// </summary>
        private void Init()
        {
            Instan_Hour();
            Instan_Minute();
            Init_Page();
        }

        /// <summary>
        /// 페이지 초기화
        /// </summary>
        private void Init_Page()
        {
            switch (mode)
            {
                case Mode.Insert:
                    text_Title.text = Manager_Language.Get_Text("할 일 추가");
                    ipf_Todo_Name.text = "";

                    for (int i = 0; i < 7; i++)
                    {
                        int_Weeks[i] = 0;
                    }
                    toggle_Manager.IsOn = false;
                    break;
                case Mode.Update:
                    text_Title.text = Manager_Language.Get_Text("할 일 편집");
                    ipf_Todo_Name.text = todo.name;
                    toggle_Manager.IsOn = todo.usemanage.Equals("1");
                    break;
            }
            toggle_Manager.col_Right.col_Outline = Manager.instance.col_Main;
        }

        /// <summary>
        /// 완료
        /// </summary>
        public void OnClickButton_Confirm()
        {
            if(string.IsNullOrWhiteSpace(ipf_Todo_Name.text))
            {
                Manager.instance.manager_Common.Show_Alert0("할 일을 입력해 주세요.");
                return;
            }
            DateTime _now = DateTime.Now;
            string _hour = ipf_Cur_Hour != null ? ipf_Cur_Hour.text : _now.Hour.ToString("0#");
            string _minute = ipf_Cur_Minute != null ? ipf_Cur_Minute.text : _now.Minute.ToString("0#");
            switch (mode)
            {
                case Mode.Insert:
                    
                    Manager.instance.manager_Data.Todo_Insert(ipf_Todo_Name.text, toggle_Manager.IsOn, int_Weeks[0].ToString(), int_Weeks[1].ToString(), int_Weeks[2].ToString(), int_Weeks[3].ToString(), int_Weeks[4].ToString(), int_Weeks[5].ToString(), int_Weeks[6].ToString(), $"{_hour}:{_minute}",
                        (Manager_Data.Data_Todo _return) =>
                        {
                            Manager.instance.page_Calender.Instan_Todo(_return);
                            if (Manager.instance.page_Calender.Prefab_Todos.Count.Equals(1))
                            {
                                Prefab_Todo _prefab = Manager.instance.page_Calender.Prefab_Todos[0];
                                Manager.instance.page_Calender.Select_Todo(_prefab);
                            }
                            todo = new Manager_Data.Data_Todo();
                            Active(false);
                        });
                    break;
                case Mode.Update:
                    Manager.instance.manager_Data.Todo_Update(todo.id, ipf_Todo_Name.text, toggle_Manager.IsOn, int_Weeks[0].ToString(), int_Weeks[1].ToString(), int_Weeks[2].ToString(), int_Weeks[3].ToString(), int_Weeks[4].ToString(), int_Weeks[5].ToString(), int_Weeks[6].ToString(), $"{_hour}:{_minute}",
                        (Manager_Data.Data_Todo _return) =>
                        {
                            Manager.instance.page_Calender.Set_Todo(_return);
                            todo = new Manager_Data.Data_Todo();
                            Active(false);
                        });
                    break;
            }
        }

        /// <summary>
        /// 페이지 닫기
        /// </summary>
        public void OnClickButton_Close()
        {
            Active(false);
        }

        #region 관리
        [Header("관리")]
        [SerializeField] private Image img_Alarm;
        [SerializeField] private CustomToggle toggle_Manager;
        [SerializeField] private CanvasGroup cvg_Week;
        [SerializeField] private int[] int_Weeks;
        [SerializeField] private Text[] text_Weeks;

        CoroutineHandle cor_Week_Alpha;
        CoroutineHandle cor_Alarm_Color;
        CoroutineHandle cor_Select_Time;

        /// <summary>
        /// 관리 토글
        /// </summary>
        /// <param name="_isLeft"></param>
        public void OnToggle(bool _isRight)
        {
            if (_isRight)
            {
                Color col_Main = Manager.instance.col_Main;
                Color col_C7C7C7 = Manager.instance.col_C7C7C7;

                Manager_Common.StartCoroutine(ref cor_Week_Alpha, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Week, true, 0.025f, ()=> { Set_Start_Time(); cvg_Week.gameObject.SetActive(true); }));
                Manager_Common.StartCoroutine(ref cor_Select_Time, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Select_Time, true));
                Manager_Common.StartCoroutine(ref cor_Alarm_Color, Manager.instance.manager_Ui.Cor_Color(img_Alarm, col_Main));
                
                todo.usemanage = "true";

                if (!string.IsNullOrEmpty(todo.id))
                {
                    int_Weeks[0] = int.Parse(todo.sunday);
                    int_Weeks[1] = int.Parse(todo.monday);
                    int_Weeks[2] = int.Parse(todo.tuesday);
                    int_Weeks[3] = int.Parse(todo.wednesday);
                    int_Weeks[4] = int.Parse(todo.thursday);
                    int_Weeks[5] = int.Parse(todo.friday);
                    int_Weeks[6] = int.Parse(todo.saturday);
                }
                for (int i = 0; i < 7; i++)
                {
                    text_Weeks[i].color = int_Weeks[i].Equals(1) ? col_Main : col_C7C7C7;
                }
            }
            else
            {
                Manager_Common.StartCoroutine(ref cor_Week_Alpha, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Week, false, 0.025f, null, () => { cvg_Week.gameObject.SetActive(true); }));
                Manager_Common.StartCoroutine(ref cor_Select_Time, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Select_Time, false));
                Manager_Common.StartCoroutine(ref cor_Alarm_Color, Manager.instance.manager_Ui.Cor_Color(img_Alarm, Manager.instance.col_C7C7C7));

                if(!string.IsNullOrEmpty(todo.id))
                    todo.usemanage = "false";
            }
        }

        /// <summary>
        /// 할 일 요일
        /// </summary>
        /// <param name="_day"></param>
        public void OnClickButton_Todo_Day_Of_Week(int _day)
        {
            if (int_Weeks[_day].Equals(0))
            {
                int_Weeks[_day] = 1;
                text_Weeks[_day].color = Manager.instance.col_Main;
            }
            else
            {
                int_Weeks[_day] = 0;
                text_Weeks[_day].color = Manager.instance.col_C7C7C7;
            }
        }
        #endregion

        #region 시간
        [Header("시간")]
        [Tooltip("시간 설정 Bundle")] [SerializeField] private CanvasGroup cvg_Select_Time;
        [Tooltip("시작 시간, -1일 경우 현재 시간")] [SerializeField] private int start_Hour = -1;
        [Tooltip("시작 분, -1일 경우 현재 시간")] [SerializeField] private int start_Minute = -1;
        [Tooltip("현재 선택 된 시간")] [SerializeField] private string cur_Hour;
        [Tooltip("현재 선택 된 분")] [SerializeField] private string cur_Minute;
        [Tooltip("현재 선택 된 시간 인풋필드")] [SerializeField] private InputField ipf_Cur_Hour;
        [Tooltip("현재 선택 된 분 인풋필드")] [SerializeField] private InputField ipf_Cur_Minute;
        [Tooltip("인풋필드 프리팹")] [SerializeField] private GameObject go_Prefab_Inputfield;
        [Tooltip("시간 스크롤뷰")] [SerializeField] private RectTransform rect_Hour_ScollView;
        [Tooltip("시간 인풋필드 프리팹 부모")] [SerializeField] private RectTransform rect_Hour_Content;
        [Tooltip("분 스크롤뷰")] [SerializeField] private RectTransform rect_Minute_ScollView;
        [Tooltip("분 인풋필드 프리팹 부모")] [SerializeField] private RectTransform rect_Minute_Content;
        [Tooltip("시간 수정 프리팹 리스트")] [SerializeField] private List<RectTransform> rect_Hour_List;
        [Tooltip("분 수정 프리팹 리스트")] [SerializeField] private List<RectTransform> rect_Minute_List;
        [Tooltip("시간 수정 인풋필드 리스트")] [SerializeField] private List<InputField> ipf_Hour_List;
        [Tooltip("분 수정 인풋필드 리스트")] [SerializeField] private List<InputField> ipf_Minute_List;
        [Tooltip("드래그 위치 감지")] private Vector2 prev_Pos;

        private void Init_Time()
        {
            Canvas canvas = transform.GetComponentInParent<Canvas>();
            hour_Dis = 1296 * canvas.transform.localScale.x;
            minute_Dis = /*3240*/ 648 * canvas.transform.localScale.x;
        }

        /// <summary>
        /// 시작시간 설정하기
        /// </summary>
        private void Set_Start_Time()
        {
            rect_Hour_Content.anchoredPosition = rect_Minute_Content.anchoredPosition = Vector2.zero;
            if (!rect_Hour_List.Count.Equals(0))
            {
                start_Hour = !string.IsNullOrWhiteSpace(todo.id) ? todo.managetime.Hour : -1;
                int _start_Hour_Index = -1;
                _start_Hour_Index = ipf_Hour_List.FindIndex(n => int.Parse(n.text).Equals(!start_Hour.Equals(-1) ? start_Hour : DateTime.Now.Hour));
                if (!_start_Hour_Index.Equals(-1))
                {
                    float _tmp = rect_Hour_List[_start_Hour_Index].anchoredPosition.y;
                    for (int i = 0; i < rect_Hour_List.Count; i++)
                    {
                        rect_Hour_List[i].anchoredPosition -= new Vector2(0, _tmp);
                    }
                }
                Set_Hour();
	    Invoke("Set_Hour", 0.01f);
            }

            if (!rect_Minute_List.Count.Equals(0))
            {
                start_Minute = !string.IsNullOrWhiteSpace(todo.id) ? todo.managetime.Minute : -1;
                int _start_Minute_Index = -1;
                int _cur_minute = DateTime.Now.Minute;
                _start_Minute_Index = ipf_Minute_List.FindIndex(n => int.Parse(n.text).Equals(!start_Minute.Equals(-1) ? start_Minute : _cur_minute - _cur_minute % 5));
                if (!_start_Minute_Index.Equals(-1))
                {
                    float _tmp = rect_Minute_List[_start_Minute_Index].anchoredPosition.y;
                    for (int i = 0; i < rect_Minute_List.Count; i++)
                    {
                        rect_Minute_List[i].anchoredPosition -= new Vector2(0, _tmp);
                    }
                }
                Set_Minute();
	    Invoke("Set_Minute", 0.01f);
            }
        }

        /// <summary>
        /// 시간 생성하기
        /// </summary>
        private void Instan_Hour()
        {
            if (rect_Hour_List.Count.Equals(0))
            {
                rect_Hour_List = new List<RectTransform>();
                for (int i = 0; i < 24; i++)
                {
                    GameObject _pref = Instantiate(go_Prefab_Inputfield, rect_Hour_Content);
                    InputField ipf = _pref.GetComponent<InputField>();
                    ipf.text = i.ToString("0#");
                    RectTransform _rect = _pref.GetComponent<RectTransform>();
                    if (i <= 12)
                    {
                        _rect.anchoredPosition = new Vector2(0, -108 * i);
                    }
                    else
                    {
                        _rect.anchoredPosition = new Vector2(0, 1296 - (i - 12) * 108);
                    }
                    _pref.SetActive(true);
                    rect_Hour_List.Add(_rect);
                    ipf_Hour_List.Add(ipf);
                }
            }
        }

        /// <summary>
        /// 시간 드래그 시작
        /// </summary>
        /// <param name="eventData"></param>
        public void OnBeginDrag_Hour(BaseEventData eventData)
        {
            prev_Pos = ((PointerEventData)eventData).position;
        }

        /// <summary>
        /// 시간 드래그
        /// </summary>
        /// <param name="baseEventData"></param>
        public void OnDrag_Hour(BaseEventData eventData)
        {
            float _dis = prev_Pos.y - ((PointerEventData)eventData).position.y;
            rect_Hour_Content.anchoredPosition -= new Vector2(0, _dis);
            Content_Position_Y(rect_Hour_Content);
            Set_Hour();
            prev_Pos = ((PointerEventData)eventData).position;
        }

        CoroutineHandle cor_EndDrag_Hour;
        /// <summary>
        /// 시간 드래그 끝났을 시
        /// </summary>
        /// <param name="baseEventData"></param>
        public void OnEndDrag_Hour(BaseEventData eventData)
        {
            float _dis = prev_Pos.y - ((PointerEventData)eventData).position.y;
            Manager_Common.StartCoroutine(ref cor_EndDrag_Hour, Cor_EndDrag_Time(_dis, rect_Hour_Content, Set_Hour));
        }

        float hour_Dis;
        /// <summary>
        /// 시간 드래그시 설정하기
        /// </summary>
        private void Set_Hour()
        {
            List<RectTransform> _rects = rect_Hour_List.FindAll(n => (n.position.y - rect_Hour_ScollView.position.y) > hour_Dis);
            _rects = _rects.OrderByDescending(n => n.anchoredPosition.y).ToList();
            for (int i = 0; i < _rects.Count; i++)
            {
                _rects[i].anchoredPosition = new Vector2(0, rect_Hour_List.Min(n => n.anchoredPosition.y) - 108);
            }

            _rects = rect_Hour_List.FindAll(n => (n.position.y - rect_Hour_ScollView.position.y) < -hour_Dis);
            _rects = _rects.OrderBy(n => n.anchoredPosition.y).ToList();
            for (int i = 0; i < _rects.Count; i++)
            {
                _rects[i].anchoredPosition = new Vector2(0, rect_Hour_List.Max(n => n.anchoredPosition.y) + 108);
            }

            float _Min = 100000;
            InputField _ipf = null;
            for (int i = 0; i < rect_Hour_List.Count; i++)
            {
                float _dis = Mathf.Abs(rect_Hour_List[i].position.y - rect_Hour_ScollView.position.y);

                if (_dis < _Min)
                {
                    _Min = _dis;
                    _ipf = ipf_Hour_List[i];
                }
            }

            if (_ipf != null)
            {
                if (ipf_Cur_Hour != null)
                {
                    if (ipf_Cur_Hour.Equals(_ipf))
                        return;
                    else
                        ipf_Cur_Hour.textComponent.font = Manager.instance.font_ExtraLight;
                }
                ipf_Cur_Hour = _ipf;
                _ipf.textComponent.font = Manager.instance.font_Regular;
                cur_Hour = _ipf.text;
            }

            _rects = null;
        }

        /// <summary>
        /// 분 생성하기
        /// </summary>
        private void Instan_Minute()
        {
            if (rect_Minute_List.Count.Equals(0))
            {
                rect_Minute_List = new List<RectTransform>();
                for (int i = 0; i < 12; i++)
                {
                    GameObject _pref = Instantiate(go_Prefab_Inputfield, rect_Minute_Content);
                    InputField ipf = _pref.GetComponent<InputField>();
                    ipf.text = (i * 5).ToString("0#");
                    RectTransform _rect = _pref.GetComponent<RectTransform>();

                    if (i <= 6)
                    {
                        _rect.anchoredPosition = new Vector2(0, -108 * i);
                    }
                    else
                    {
                        _rect.anchoredPosition = new Vector2(0, /*3240*/ 648 - (i - 6) * 108);
                    }

                    
                    _pref.SetActive(true);
                    rect_Minute_List.Add(_rect);
                    ipf_Minute_List.Add(ipf);
                }
            }
        }

        /// <summary>
        /// 분 드래그 시작
        /// </summary>
        /// <param name="eventData"></param>
        public void OnBeginDrag_Minute(BaseEventData eventData)
        {
            prev_Pos = ((PointerEventData)eventData).position;
        }

        /// <summary>
        /// 분 드래그
        /// </summary>
        /// <param name="baseEventData"></param>
        public void OnDrag_Minute(BaseEventData eventData)
        {
            float _dis = prev_Pos.y - ((PointerEventData)eventData).position.y;
            rect_Minute_Content.anchoredPosition -= new Vector2(0, _dis);
            Content_Position_Y(rect_Minute_Content);
            Set_Minute();
            prev_Pos = ((PointerEventData)eventData).position;
        }

        CoroutineHandle cor_EndDrag_Minute;

        /// <summary>
        /// 분 드래그 끝났을 시
        /// </summary>
        /// <param name="baseEventData"></param>
        public void OnEndDrag_Minute(BaseEventData eventData)
        {
            float _dis = prev_Pos.y - ((PointerEventData)eventData).position.y;
            Manager_Common.StartCoroutine(ref cor_EndDrag_Minute, Cor_EndDrag_Time(_dis, rect_Minute_Content, Set_Minute));
        }

        float minute_Dis;

        /// <summary>
        /// 분 드래그시 설정하기
        /// </summary>
        private void Set_Minute()
        {
            List<RectTransform> _rects = rect_Minute_List.FindAll(n => (n.position.y - rect_Minute_ScollView.position.y) > minute_Dis);
            _rects = _rects.OrderByDescending(n => n.anchoredPosition.y).ToList();
            for (int i = 0; i < _rects.Count; i++)
            {
                _rects[i].anchoredPosition = new Vector2(0, rect_Minute_List.Min(n => n.anchoredPosition.y) - 108);
            }

            _rects = rect_Minute_List.FindAll(n => (n.position.y - rect_Minute_ScollView.position.y) < -minute_Dis);
            _rects = _rects.OrderBy(n => n.anchoredPosition.y).ToList();
            for (int i = 0; i < _rects.Count; i++)
            {
                _rects[i].anchoredPosition = new Vector2(0, rect_Minute_List.Max(n => n.anchoredPosition.y) + 108);
            }

            float _Min = 100000;
            InputField _ipf = null;
            for (int i = 0; i < rect_Minute_List.Count; i++)
            {
                float _dis = Mathf.Abs(rect_Minute_List[i].position.y - rect_Minute_ScollView.position.y);

                if (_dis < _Min)
                {
                    _Min = _dis;
                    _ipf = ipf_Minute_List[i];
                }
            }

            if (_ipf != null)
            {
                if (ipf_Cur_Minute != null)
                {
                    if (ipf_Cur_Minute.Equals(_ipf)) 
                        return;
                    else
                        ipf_Cur_Minute.textComponent.font = Manager.instance.font_ExtraLight;
                }
                ipf_Cur_Minute = _ipf;
                _ipf.textComponent.font = Manager.instance.font_Regular;
                cur_Minute = _ipf.text;
            }

            _rects = null;
        }

        /// <summary>
        /// 드래그 끝났을 시
        /// </summary>
        private IEnumerator<float> Cor_EndDrag_Time(float _dis, RectTransform _rect, UnityAction _action)
        {

            while (!(Mathf.Abs(_dis) < 2))
            {
                _rect.anchoredPosition -= new Vector2(0, _dis);

                Content_Position_Y(_rect);
                _action?.Invoke();
                _dis = Mathf.Lerp(_dis, 0, Time.smoothDeltaTime * 10);
                yield return Timing.WaitForSeconds(0);
            }

            float _d = _rect.anchoredPosition.y % 108;
            float _m = 0;
            if (_d > 0)
                if (_d > 54)
                    _m = 108 - _d;
                else
                    _m = -_d;
            else
                if (_d < -54)
                _m = -(108 + _d);
            else
                _m = -_d;
            while (!(Mathf.Abs(_m) < 0.1f))
            {
                float _move = Time.smoothDeltaTime * 200;
                if(Mathf.Abs(_m) < _move)
                {
                    if (_m > 0)
                    {
                        _rect.anchoredPosition += new Vector2(0, _m);
                    }
                    else
                    {
                        _rect.anchoredPosition -= new Vector2(0, _m);
                    }
                    _m -= _m;
                }
                else
                {
                    if (_m > 0)
                    {
                        _m -= _move;
                        _rect.anchoredPosition += new Vector2(0, _move);
                    }
                    else
                    {
                        _m += _move;
                        _rect.anchoredPosition -= new Vector2(0, _move);
                    }
                }
                yield return Timing.WaitForSeconds(0);
            }
        }

        /// <summary>
        /// 드래그 시
        /// </summary>
        private void Content_Position_Y(RectTransform _rect)
        {
            int _height = _rect.childCount * 108;
            int _height_Half = (int)(_height * 0.5f);
            if (_rect.anchoredPosition.y > _height_Half)
            {
                _rect.anchoredPosition -= new Vector2(0, _height);
                for (int i = 0; i < _rect.childCount; i++)
                {
                    _rect.GetChild(i).GetComponent<RectTransform>().anchoredPosition += new Vector2(0, _height);
                }
            }
            else if (_rect.anchoredPosition.y < -_height_Half)
            {
                _rect.anchoredPosition += new Vector2(0, _height);
                for (int i = 0; i < _rect.childCount; i++)
                {
                    _rect.GetChild(i).GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, _height);
                }
            }
        }
        #endregion
    }
}