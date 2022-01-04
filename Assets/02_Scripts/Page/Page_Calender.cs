using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NORK
{
    public class Page_Calender : Page
    {
        internal override void OnEnable()
        {
            base.OnEnable();

            Init();
        }

        internal override void Start()
        {
            base.Start();

            Start_Week();
        }

        private void Update()
        {
            if(Manager_Common.isBack)
            {
                if (page_Todo_Page.gameObject.activeSelf)
                    OnClickButton_Open_Todo_Page(false);
                else if (!Manager.instance.page_Setting.gameObject.activeSelf && !Manager.instance.page_Todo.gameObject.activeSelf && !Manager.instance.page_Record.gameObject.activeSelf)
                    Manager.instance.manager_Common.Start_Quit();
            }

#if UNITY_EDITOR
            str_date_Year_Month = Manager.instance.manager_Common.DateTimeToYMD(date_Year_Month);
            str_date_Year_Month_Day = Manager.instance.manager_Common.DateTimeToYMD(date_Year_Month_Day);
#endif
        }

#if UNITY_EDITOR
    public string str_date_Year_Month;
        public string str_date_Year_Month_Day;
#endif

        private void Init()
        {
            cvg_Month.alpha = 0;
            go_Month_Loading.SetActive(true);
            img_Char.sprite = Manager.instance.manager_Common.data_Characters.Find(n => n.name.ToString().Equals(Manager.instance.manager_Member.Get_Data_Member.character)).img;
            date_Year_Month = DateTime.Now;
            date_Year_Month_Day = new DateTime(date_Year_Month.Year, date_Year_Month.Month, date_Year_Month.Day);
            date_Year_Month = new DateTime(date_Year_Month.Year, date_Year_Month.Month, 1);
            Timing.RunCoroutine(Init_Todo());
        }

        #region 할일 공통
        [Header("할 일")]
        [Tooltip("현재 선택된 할 일")]
        [SerializeField] private Manager_Data.Data_Todo cur_Todo;
        public Manager_Data.Data_Todo Cur_Todo { get { return cur_Todo; } }

        /// <summary>
        /// 현재 할 일이 존재하는지 체크
        /// </summary>
        /// <returns></returns>
        private bool HasTodo()
        {
            return !string.IsNullOrWhiteSpace(cur_Todo.id);
        }

        /// <summary>
        /// 현재 할 일 선택
        /// </summary>
        /// <param name="_cur_Todo"></param>
        public void Set_Cur_Todo(Manager_Data.Data_Todo _cur_Todo)
        {
            Manager.instance.manager_Common.Start_Loading();
            Manager_Common.StartCoroutine(ref cor_Set_Cur_Todo, Cor_Set_Cur_Todo(_cur_Todo));
        }

        CoroutineHandle cor_Set_Cur_Todo;
        /// <summary>
        /// 현재 할 일 선택
        /// </summary>
        /// <param name="_cur_Todo"></param>
        IEnumerator<float> Cor_Set_Cur_Todo(Manager_Data.Data_Todo _cur_Todo)
        {
            cur_Todo = _cur_Todo;

            yield return Timing.WaitUntilDone(Cor_Load_Data_Set_Days());
        }

        IEnumerator<float> Cor_Load_Data_Set_Days(UnityAction action_Year = null, UnityAction action_Month = null)
        {
            string _date = date_Year_Month.ToString("yyyy");
            Manager.instance.manager_Common.Start_Loading();
            if(HasTodo())
            {
                Manager.instance.manager_Data.Todo_Done_List(cur_Todo.id, _date);
                Manager.instance.manager_Data.Todo_Record_List(cur_Todo.id, _date);

                yield return Timing.WaitUntilTrue(() => { return Manager.instance.manager_Data.todo_Done_List_Loaded && Manager.instance.manager_Data.todo_Record_List_Loaded; });
            }

            Set_Days(action_Year, action_Month);
            Manager.instance.manager_Common.Stop_Loading();
        }

        /// <summary>
        /// 할 일 초기 설정
        /// </summary>
        private IEnumerator<float> Init_Todo()
        {
            yield return Timing.WaitUntilTrue(() => { return Manager.instance.manager_Data.todo_List_Loaded; });
            cur_Todo = new Manager_Data.Data_Todo();
            string saved_id = Manager.instance.manager_Common.Load_Regedit("ssh_t13");
            List<Manager_Data.Data_Todo> todos = Manager.instance.manager_Data.data_ToDos;
            if (string.IsNullOrEmpty(saved_id))//저장 된 Todo가 없을 시
            {
                if (!todos.Count.Equals(0))
                    Set_Cur_Todo(todos[0]);
            }
            else//저장 된 Todo가 있을 시
            {
                int _index = todos.FindIndex(n => n.id.Equals(saved_id));
                if (!_index.Equals(-1))
                    Set_Cur_Todo(todos[_index]);
                else
                {
                    if (!todos.Count.Equals(0))
                        Set_Cur_Todo(todos[0]);
                }
            }

            if (string.IsNullOrEmpty(cur_Todo.id))
            {
                Set_Days();
            }
        }

        /// <summary>
        /// 프리팹 색상
        /// </summary>
        public void Set_Color_Prefab_Days()
        {
            for (int i = 0; i < 12; i++)
            {
                Set_Color(data_Years[i].prefab_Days);
                Set_Color(data_Years_Back[i].prefab_Days);
            }

            Set_Color(month_Days);
            Set_Color(month_Back_Days);

            void Set_Color(List<Prefab_Day> prefab_Days)
            {
                for (int i = 0; i < prefab_Days.Count; i++)
                {
                    prefab_Days[i].Set_Color();
                }
            }
        }
        #endregion

        #region 0. Subpage 공통
        public enum Calender_Mode { Year, Month, Week }
        public bool IsYearOrMonth(Calender_Mode _mode)
        {
            return _mode.Equals(Calender_Mode.Year) || _mode.Equals(Calender_Mode.Month);
        }

        public bool IsMonth(Calender_Mode _mode)
        {
            return _mode.Equals(Calender_Mode.Month);
        }

        public bool IsWeek(Calender_Mode _mode)
        {
            return _mode.Equals(Calender_Mode.Week);
        }
        
        [Header("공통")]
        [Tooltip("현재 모드")]
        [SerializeField] private Calender_Mode calender_Mode; public Calender_Mode Get_Calender_Mode { get { return calender_Mode; } }
        [Tooltip("일 프리팹 오브젝트 - 월")]
        [SerializeField] private GameObject go_Prefab_Month_Day;
        [Tooltip("일 프리팹 오브젝트 - 주")]
        [SerializeField] private GameObject go_Prefab_Week_Day;
        [Tooltip("현재 년-월")]
        [SerializeField] private DateTime date_Year_Month; public DateTime Date_Year_Month { get { return date_Year_Month; } }
        [Tooltip("현재 년-월-일")]
        [SerializeField] private DateTime date_Year_Month_Day; public DateTime Date_Year_Month_Day { get { return date_Year_Month_Day; } }

        /// <summary>
        /// 연, 월, 주 
        /// </summary>
        /// <returns></returns>
        private void Set_Days(UnityAction action_Year = null, UnityAction action_Month = null)
        {
            Set_Year(action_Year);
            if (!calender_Mode.Equals(Calender_Mode.Year))
                Set_Month(action_Month);

            cvg_Month.alpha = 1;
            go_Month_Loading.SetActive(false);
            Manager.instance.manager_Common.Stop_Loading();
        }

        /// <summary>
        /// Prefab_Day 설정
        /// </summary>
        /// <param name="_day"></param>
        /// <param name="thatDay"></param>
        /// <param name="_isThisMonth"></param>
        /// <param name="_isToday"></param>
        void Set_Day(Prefab_Day _day, DateTime thatDay, bool _isThisMonth, bool _isToday)
        {
            string done_id = "-1";
            string record_id = "-1";

            int index = Manager.instance.manager_Data.data_ToDo_Dones.FindIndex(n => n.tdid.Equals(cur_Todo.id) && n.date.Equals(thatDay));
            if (!index.Equals(-1)) done_id = Manager.instance.manager_Data.data_ToDo_Dones[index].id;

            index = Manager.instance.manager_Data.data_ToDo_Records.FindIndex(n => n.tdid.Equals(cur_Todo.id) && n.date.Equals(thatDay));
            if (!index.Equals(-1)) record_id = Manager.instance.manager_Data.data_ToDo_Records[index].id;

            _day.Set_Data(thatDay,
                _isThisMonth,
                _isToday,
                done_id,
                record_id,
                Manager.instance.manager_Common.Get_Days_Int(thatDay.DayOfWeek));
        }

        public void OnClickButton_Open_Setting()
        {
            Manager.instance.manager_Common.Active_Page(Manager.instance.page_Setting, false);
        }

        /// <summary>
        /// 게시글 페이지 열기
        /// </summary>
        public void OnClickButton_Open_Record()
        {
            if (string.IsNullOrWhiteSpace(cur_Todo.id)) return;
            Manager.instance.manager_Common.Active_Page(Manager.instance.page_Record, false);
        }
        #endregion

        #region 1. Subpage_Year
        /// <summary>
        /// 년 데이터
        /// </summary>
        [Serializable]
        public struct Data_Year
        {
            // Month
            public int month;
            public GameObject go_Month;
            public Text text_Month;

            // Days
            public List<Prefab_Day> prefab_Days;
        }

        [Header("년")]
        [Tooltip("Bundle 년")]
        [SerializeField] private RectTransform rect_Bundle_Year;
        [Tooltip("년 할일")]
        [SerializeField] private Text text_Year_Year_Todo;
        [Tooltip("년 Month Bundle")] [SerializeField] private RectTransform rect_Year_Bundle_Months;
        [Tooltip("년 Month Bundle_Back")] [SerializeField] private RectTransform rect_Year_Bundle_Months_Back;

        [Tooltip("년 데이터 리스트")]
        [SerializeField] private List<Data_Year> data_Years;
        [Tooltip("년 데이터_Back 리스트")]
        [SerializeField] private List<Data_Year> data_Years_Back;

        /// <summary>
        /// 현재 년 세팅하기
        /// </summary>
        private void Set_Year(UnityAction action = null)
        {
            #region Year, Todo
            string _todo = string.IsNullOrWhiteSpace(cur_Todo.name) ? "-" : cur_Todo.name;
            text_Year_Year_Todo.text = $"{date_Year_Month.Year} / <color=black>{_todo}</color>";
            #endregion

            #region days
            DateTime now = DateTime.Now;
            for (int i = 0; i < 12; i++)
            {
                DateTime firstDay = new DateTime(date_Year_Month.Year, i + 1, 1);
                bool isThisMonth = firstDay.Year.Equals(now.Year) && firstDay.Month.Equals(now.Month);
                data_Years[i].text_Month.color = isThisMonth ? Manager.instance.col_Main : Color.black;

                int index = Manager.instance.manager_Common.Get_Days_Int(firstDay.DayOfWeek);
                int date = 0;

                for (int j = 0; j < 42; j++)
                {
                    Prefab_Day _day = data_Years[i].prefab_Days[j];
                    _day.gameObject.SetActive(false);

                    if (j >= index)
                    {
                        DateTime thatDay = firstDay.AddDays(date);

                        if (!thatDay.Month.Equals(firstDay.Month))
                            continue;
                        _day.gameObject.SetActive(true);
                        ++date;

                        Set_Day(_day, thatDay, true, firstDay.Year.Equals(now.Year) && firstDay.Month.Equals(now.Month) && date.Equals(now.Day));
                    }
                }
            }

            
            #endregion

            action?.Invoke();
        }

        /// <summary>
        /// 연에서 날짜 찾기
        /// </summary>
        /// <param name="_md"></param>
        /// <returns></returns>
        public Prefab_Day Find_Prefab_Day_Year(DateTime _md)
        {
            Data_Year dy = data_Years.Find(n => n.month.Equals(_md.Month));
            return dy.prefab_Days.Find(n => n.gameObject.activeSelf && n.IsThisMonth && n.Date.Equals(_md));
        }

        /// <summary>
        /// 월로 가기
        /// </summary>
        public void OnClickButton_Go_Month_From_Year(int _month)
        {
            calender_Mode = Calender_Mode.Month;

            date_Year_Month = new DateTime(date_Year_Month.Year, _month, 1);
            date_Year_Month_Day = date_Year_Month;

            Set_Month();

            rect_Bundle_Month_Week.gameObject.SetActive(true);
            rect_Bundle_Year.gameObject.SetActive(false);

            ShowOrHide_Button(true);
        }

        /// <summary>
        /// 년 드래그 시작
        /// </summary>
        /// <param name="_base"></param>
        public void OnBeganDrag_Year(BaseEventData _base)
        {
            Manager.instance.manager_Common.BeganDrag(((PointerEventData)_base).position);
        }


        CoroutineHandle cor_rect_Year_Bundle_Months;
        /// <summary>
        /// 년 드래그 끝났을 시
        /// </summary>
        /// <param name="_base"></param>
        public void OnEndDrag_Year(BaseEventData _base)
        {
            if (cor_rect_Year_Bundle_Months.IsRunning) return;
            Manager_Common.Data_Drag dd = Manager.instance.manager_Common.EndDrag(((PointerEventData)_base).position);
            if (Mathf.Abs(dd.ratio_y) < 0.1f && dd.time < 0.5f)
            {
                if (dd.ratio_x > 0.3f)
                {
                    rect_Year_Bundle_Months.anchoredPosition = new Vector2(-1080, -222);
                    date_Year_Month = date_Year_Month.AddYears(-1);
                }
                else if (dd.ratio_x < -0.3f)
                {
                    rect_Year_Bundle_Months.anchoredPosition = new Vector2(1080, -222);
                    date_Year_Month = date_Year_Month.AddYears(1);
                }
                else
                    return;

                for (int i = 0; i < 12; i++)
                {
                    data_Years_Back[i].text_Month.color = data_Years[i].text_Month.color;
                    for (int j = 0; j < 42; j++)
                    {
                        Prefab_Day _pd = data_Years[i].prefab_Days[j];
                        bool _active = _pd.gameObject.activeSelf;
                        data_Years_Back[i].prefab_Days[j].gameObject.SetActive(_active);
                        if (_active)
                            data_Years_Back[i].prefab_Days[j].Set_Data(_pd.Date, _pd.IsThisMonth, _pd.IsToday, _pd.Data_ToDo_Done_Id, _pd.Data_ToDo_Record_Id, _pd.Week);
                    }
                }
                rect_Year_Bundle_Months_Back.anchoredPosition = rect_Year_Bundle_Months.anchoredPosition;

                Timing.RunCoroutine(Cor_Load_Data_Set_Days(Set_Pos));

                void Set_Pos()
                {
                    if (dd.ratio_x > 0.3f)
                    {
                        Manager_Common.StartCoroutine(ref cor_rect_Year_Bundle_Months, Manager.instance.manager_Ui.Cor_Pos_Anchored(new RectTransform[2] { rect_Year_Bundle_Months, rect_Year_Bundle_Months_Back }, new Vector2[2] { new Vector2(0, -222), new Vector2(1080, -222) }, 8));
                    }
                    else if (dd.ratio_x < -0.3f)
                    {
                        Manager_Common.StartCoroutine(ref cor_rect_Year_Bundle_Months, Manager.instance.manager_Ui.Cor_Pos_Anchored(new RectTransform[2] { rect_Year_Bundle_Months, rect_Year_Bundle_Months_Back }, new Vector2[2] { new Vector2(0, -222), new Vector2(-1080, -222) }, 8));
                    }
                }
            }
        }
        #endregion

        #region 2. Subpage_Month
        [Header("월")]
        [Tooltip("월 캔퍼스 그룹")]
        [SerializeField] private CanvasGroup cvg_Month;
        [Tooltip("월 로딩")]
        [SerializeField] private GameObject go_Month_Loading;
        [Tooltip("Bundle 월 - 주")]
        [SerializeField] private RectTransform rect_Bundle_Month_Week;
        [Tooltip("캐릭터")]
        [SerializeField] private Image img_Char;

        [Tooltip("달 연-월")]
        [SerializeField] private Text text_Month_Year_Month;
        [Tooltip("달 할일 이름")]
        [SerializeField] private Text text_Month_Todo;
        [Tooltip("달 첫째주")]
        [SerializeField] private RectTransform rect_Month_Day_1;
        [SerializeField] private CanvasGroup cvg_Month_Day_1;
        [Tooltip("달 둘째주")]
        [SerializeField] private RectTransform rect_Month_Day_2;
        [SerializeField] private CanvasGroup cvg_Month_Day_2;
        [Tooltip("달 셋째주")]
        [SerializeField] private RectTransform rect_Month_Day_3;
        [SerializeField] private CanvasGroup cvg_Month_Day_3;
        [Tooltip("달 넷째주")]
        [SerializeField] private RectTransform rect_Month_Day_4;
        [SerializeField] private CanvasGroup cvg_Month_Day_4;
        [Tooltip("달 다섯째주")]
        [SerializeField] private RectTransform rect_Month_Day_5;
        [SerializeField] private CanvasGroup cvg_Month_Day_5;
        [Tooltip("달 여섯째주")]
        [SerializeField] private RectTransform rect_Month_Day_6;
        [SerializeField] private CanvasGroup cvg_Month_Day_6;

        [Tooltip("Bundle_Day")]
        [SerializeField] private RectTransform rect_Bundle_Day;
        [Tooltip("Bundle_Day_Back")]
        [SerializeField] private RectTransform rect_Bundle_Day_Back;

        [Tooltip("일 - 42개")]
        [SerializeField] private List<Prefab_Day> month_Days;
        [Tooltip("일_Back - 42개")]
        [SerializeField] private List<Prefab_Day> month_Back_Days;
        [Tooltip("현재 선택한 주")]
        [SerializeField] private RectTransform cur_Month_Week;
        [Tooltip("현재 선택한 주")]
        [SerializeField] private CanvasGroup cvg_Month_Week;
        [Tooltip("현재 선택한 날")]
        [SerializeField] private Prefab_Day cur_Prefab_Day;
        public Prefab_Day Cur_Prefab_Day { get { return cur_Prefab_Day; } }
        [Tooltip("주간으로 이동하는 버튼")]
        [SerializeField] private GameObject go_Week_Button;

        /// <summary>
        /// 년 달 클릭시
        /// </summary>
        public void OnClickButton_Year_Month()
        {
            switch (calender_Mode)
            {
                case Calender_Mode.Year:
                    break;
                case Calender_Mode.Month:
                    OnClickButton_Go_Year_From_Month();
                    break;
                case Calender_Mode.Week:
                    OnClickButton_Go_Month_From_Week();
                    break;
            }
        }

        /// <summary>
        /// 현재 달 세팅하기
        /// </summary>
        private void Set_Month(UnityAction action = null)
        {
            #region Todo
            Set_Month_Todo_Text();
            #endregion

            #region Month
            text_Month_Year_Month.text = date_Year_Month.ToString("yyyy.MM");
            #endregion

            #region Day
            DateTime firstDay = date_Year_Month.AddDays(-(date_Year_Month.Day - 1));
            DateTime now = DateTime.Now;
            int index = Manager.instance.manager_Common.Get_Days_Int(firstDay.DayOfWeek);
            int date = 0;

            for (int i = 0; i < 42; i++)
            {
                Prefab_Day _day = month_Days[i];
                //_day.gameObject.SetActive(false);
                if (i < index)//이전달
                {
                    DateTime thatDay = firstDay.AddDays(i - index);
                    Set_Day(_day, thatDay, false, false);
                    //_day.gameObject.SetActive(true);
                }
                else//이번달, 다음달
                {
                    DateTime thatDay = firstDay.AddDays(date);
                    //_day.gameObject.SetActive(true);
                    ++date;
                    
                    bool _isThisMonth = thatDay.Month.Equals(firstDay.Month);
                    bool _isToday = date_Year_Month.Year.Equals(now.Year) && date_Year_Month.Month.Equals(now.Month) && date.Equals(now.Day);

                    Set_Day(_day, thatDay, _isThisMonth, _isToday);
                }
            }
            Select_Cur_Day(Find_Prefab_Day_Month(date_Year_Month_Day), false);
            #endregion

            action?.Invoke();
        }

        /// <summary>
        /// 월에서 날짜 찾기
        /// </summary>
        /// <param name="_md"></param>
        /// <returns></returns>
        public Prefab_Day Find_Prefab_Day_Month(DateTime _md)
        {
            return month_Days.Find(n => n.gameObject.activeSelf && n.Date.Equals(_md));
        }

        public void Set_Month_Todo_Text()
        {
            text_Month_Todo.text = string.IsNullOrWhiteSpace(cur_Todo.name) ? "-" : cur_Todo.name;
        }

        /// <summary>
        /// 일 선택
        /// </summary>
        public void Select_Cur_Day(Prefab_Day _day, bool _use_Week = true)
        {
            if (calender_Mode.Equals(Calender_Mode.Week) && _use_Week)
                Select_Cur_Day_Week(_day, true);
            else
                Set_Cur_Day(_day);
        }

        public void Select_Cur_Day_Week(Prefab_Day _day, bool _check, bool _set_Cur_Day = true)
        {
            if (_set_Cur_Day)
                Set_Cur_Day(_day);
            if (calender_Mode.Equals(Calender_Mode.Week))
            {
                Manager_Common.StartCoroutine(ref cor_Week_Select_Move, Manager.instance.manager_Ui.Cor_Pos_Anchored(rect_Bundle_Week_FirstPoint, new Vector2(-cur_Prefab_Day.transform.GetSiblingIndex() * 531, 0), 5, null, ()=> { Set_Arrow_Pos();/*Set_Drag_Week(false, false);*/ }, () => { Set_Drag_Week(false); }));
                if (_check)
                    Check_Day(_day);
            }
        }

        public void Select_Cur_Day_Week_Drag(Prefab_Day _day, bool _check, bool _set_Cur_Day = true)
        {
            if (_set_Cur_Day)
                Set_Cur_Day(_day);
            if (calender_Mode.Equals(Calender_Mode.Week))
            {
                Manager_Common.StartCoroutine(ref cor_Week_Select_Move, Manager.instance.manager_Ui.Cor_Pos_Anchored(rect_Bundle_Week_FirstPoint, new Vector2(-cur_Prefab_Day.transform.GetSiblingIndex() * 531, 0), 5, null, () => { Set_Arrow_Pos(); }, () => { Set_Arrow_Pos(); }));
                if (_check)
                    Check_Day(_day);
            }
        }

        private void Set_Cur_Day(Prefab_Day _day)
        {
            if (cur_Prefab_Day != null && !calender_Mode.Equals(Calender_Mode.Week))
                cur_Prefab_Day.Select(false);
            cur_Prefab_Day = _day;
            if (!calender_Mode.Equals(Calender_Mode.Week))
                cur_Prefab_Day.Select(true);
            date_Year_Month_Day = cur_Prefab_Day.Date;
            switch (cur_Prefab_Day.transform.parent.GetSiblingIndex())
            {
                case 0:
                    cur_Month_Week = rect_Month_Day_1;
                    cvg_Month_Week = cvg_Month_Day_1;
                    break;
                case 1:
                    cur_Month_Week = rect_Month_Day_2;
                    cvg_Month_Week = cvg_Month_Day_2;
                    break;
                case 2:
                    cur_Month_Week = rect_Month_Day_3;
                    cvg_Month_Week = cvg_Month_Day_3;
                    break;
                case 3:
                    cur_Month_Week = rect_Month_Day_4;
                    cvg_Month_Week = cvg_Month_Day_4;
                    break;
                case 4:
                    cur_Month_Week = rect_Month_Day_5;
                    cvg_Month_Week = cvg_Month_Day_5;
                    break;
                case 5:
                    cur_Month_Week = rect_Month_Day_6;
                    cvg_Month_Week = cvg_Month_Day_6;
                    break;
            }
        }

        public bool IsSelectDay(Prefab_Day _day)
        {
            if (cur_Prefab_Day != null && cur_Prefab_Day.Equals(_day))
                return true;
            else
                return false;
        }

        public void Check_Day(Prefab_Day _day)
        {
            if (!string.IsNullOrWhiteSpace(cur_Todo.id))
            {
                Prefab_Day _y = Find_Prefab_Day_Year(_day.Date);
                Prefab_Day _m = Find_Prefab_Day_Month(_day.Date);
                Prefab_Day _w = Find_Prefab_Day_Week(_day.Date);

                if (_day.Data_ToDo_Done_Id.Equals("-1"))
                {
                    Set_Done_B(_y, true);
                    Set_Done_B(_m, true);
                    Set_Done_B(_w, true);
                    Manager.instance.manager_Data.Todo_Done_Insert(cur_Todo.id, Manager.instance.manager_Common.DateTimeToYMD(_day.Date), (string _return) =>
                    {
                        Set_Done_S(_y, _return);
                        Set_Done_S(_m, _return);
                        Set_Done_S(_w, _return);
                    });
                }
                else
                {
                    Set_Done_B(_y, false);
                    Set_Done_B(_m, false);
                    Set_Done_B(_w, false);
                    Manager.instance.manager_Data.Todo_Done_Delete(_day.Data_ToDo_Done_Id, () =>
                    {
                        Set_Done_S(_y, "-1");
                        Set_Done_S(_m, "-1");
                        Set_Done_S(_w, "-1");
                    });
                }

                void Set_Done_B(Prefab_Day _day, bool _b)
                {
                    if (_day) _day.Set_Done(_b);
                }
                void Set_Done_S(Prefab_Day _day, string _i)
                {
                    if (_day) _day.Set_Done(_i);
                }
            }
        }

        /// <summary>
        /// 년으로 이동
        /// </summary>
        private void OnClickButton_Go_Year_From_Month()
        {
            calender_Mode = Calender_Mode.Year;

            rect_Bundle_Month_Week.gameObject.SetActive(false);
            rect_Bundle_Year.gameObject.SetActive(true);

            ShowOrHide_Button(false);
        }


        CoroutineHandle cor_Month_Week_Move;
        CoroutineHandle cor_Week_Move;
        CoroutineHandle cor_Week_Select_Move;
        /// <summary>
        /// 주간으로 이동
        /// </summary>
        public void OnClickButton_Go_Week_From_Month()
        {
            calender_Mode = Calender_Mode.Week;
            Active_Month_Week(false);
            Change_Button(false);
            cur_Prefab_Day.Select(false);
            

            Set_Week();
        }

        CoroutineHandle cor_Alpha_Month_Day_1;
        CoroutineHandle cor_Alpha_Month_Day_2;
        CoroutineHandle cor_Alpha_Month_Day_3;
        CoroutineHandle cor_Alpha_Month_Day_4;
        CoroutineHandle cor_Alpha_Month_Day_5;
        CoroutineHandle cor_Alpha_Month_Day_6;
        /// <summary>
        /// 선택된 날의 주 제외하고 껏다 켰다 하기
        /// </summary>
        /// <param name="_enable"></param>
        private void Active_Month_Week(bool _enable, bool _direct = false)
        {
            if(_direct)
            {
                Enable(_enable ? 1 : 0);
                void Enable(int _int)
                {
                    if (!cur_Month_Week.Equals(rect_Month_Day_1))
                        cvg_Month_Day_1.alpha = _int;
                    if (!cur_Month_Week.Equals(rect_Month_Day_2))
                        cvg_Month_Day_2.alpha = _int;
                    if (!cur_Month_Week.Equals(rect_Month_Day_3))
                        cvg_Month_Day_3.alpha = _int;
                    if (!cur_Month_Week.Equals(rect_Month_Day_4))
                        cvg_Month_Day_4.alpha = _int;
                    if (!cur_Month_Week.Equals(rect_Month_Day_5))
                        cvg_Month_Day_5.alpha = _int;
                    if (!cur_Month_Week.Equals(rect_Month_Day_6))
                        cvg_Month_Day_6.alpha = _int;
                }
            }
            else
            {
                if (!cur_Month_Week.Equals(rect_Month_Day_1))
                    Manager_Common.StartCoroutine(ref cor_Alpha_Month_Day_1, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Month_Day_1, _enable));
                if (!cur_Month_Week.Equals(rect_Month_Day_2))
                    Manager_Common.StartCoroutine(ref cor_Alpha_Month_Day_2, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Month_Day_2, _enable));
                if (!cur_Month_Week.Equals(rect_Month_Day_3))
                    Manager_Common.StartCoroutine(ref cor_Alpha_Month_Day_3, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Month_Day_3, _enable));
                if (!cur_Month_Week.Equals(rect_Month_Day_4))
                    Manager_Common.StartCoroutine(ref cor_Alpha_Month_Day_4, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Month_Day_4, _enable));
                if (!cur_Month_Week.Equals(rect_Month_Day_5))
                    Manager_Common.StartCoroutine(ref cor_Alpha_Month_Day_5, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Month_Day_5, _enable));
                if (!cur_Month_Week.Equals(rect_Month_Day_6))
                    Manager_Common.StartCoroutine(ref cor_Alpha_Month_Day_6, Manager.instance.manager_Ui.Cor_CanvasAlpha(cvg_Month_Day_6, _enable));
            }
        }

        /// <summary>
        /// 월 / 주 버튼
        /// </summary>
        /// <param name="_isMonth"></param>
        private void Change_Button(bool _isMonth)
        {
            go_Week_Button.SetActive(_isMonth);
            go_Month_Button.SetActive(!_isMonth);
        }

        /// <summary>
        /// 월 / 주 버튼
        /// </summary>
        /// <param name="_isMonth"></param>
        private void ShowOrHide_Button(bool _isMonth)
        {
            if (_isMonth)
                Change_Button(true);
            else
            {
                go_Week_Button.SetActive(false);
                go_Month_Button.SetActive(false);
            }
        }

        /// <summary>
        /// 월 드래그 시작
        /// </summary>
        /// <param name="_base"></param>
        public void OnBeganDrag_Month(BaseEventData _base)
        {
            if (calender_Mode.Equals(Calender_Mode.Week)) return;
            Manager.instance.manager_Common.BeganDrag(((PointerEventData)_base).position);
        }

        CoroutineHandle cor_rect_Month_Bundle_Day;
        /// <summary>
        /// 월 드래그 끝났을 시
        /// </summary>
        /// <param name="_base"></param>
        public void OnEndDrag_Month(BaseEventData _base)
        {
            if (calender_Mode.Equals(Calender_Mode.Week)) return;
            if (cor_rect_Month_Bundle_Day.IsRunning) return;
            Manager_Common.Data_Drag dd = Manager.instance.manager_Common.EndDrag(((PointerEventData)_base).position);
            if (Mathf.Abs(dd.ratio_y) < 0.1f && dd.time < 0.5f)
            {
                int origin_year = date_Year_Month.Year;
                if (dd.ratio_x > 0.3f)
                {
                    rect_Bundle_Day.anchoredPosition = new Vector2(-1080, -135);
                    date_Year_Month = date_Year_Month.AddMonths(-1);
                }
                else if (dd.ratio_x < -0.3f)
                {
                    rect_Bundle_Day.anchoredPosition = new Vector2(1080, -135);
                    date_Year_Month = date_Year_Month.AddMonths(1);
                }
                else
                    return;

                for (int i = 0; i < month_Days.Count; i++)
                {
                    bool _active = month_Days[i].gameObject.activeSelf;
                    month_Back_Days[i].gameObject.SetActive(_active);
                    if (_active)
                        month_Back_Days[i].Set_Data(month_Days[i].Date, month_Days[i].IsThisMonth, month_Days[i].IsToday, month_Days[i].Data_ToDo_Done_Id, month_Days[i].Data_ToDo_Record_Id, month_Days[i].Week);
                }
                rect_Bundle_Day_Back.anchoredPosition = rect_Bundle_Day.anchoredPosition;

                date_Year_Month_Day = date_Year_Month;
                int cur_year = date_Year_Month.Year;
                if (!origin_year.Equals(cur_year))
                {
                    Timing.RunCoroutine(Cor_Load_Data_Set_Days(null, Set_Pos));
                }
                else
                {
                    Set_Month(Set_Pos);
                }

                void Set_Pos()
                {
                    if (dd.ratio_x > 0.3f)
                    {
                        Manager_Common.StartCoroutine(ref cor_rect_Month_Bundle_Day, Manager.instance.manager_Ui.Cor_Pos_Anchored(new RectTransform[2] { rect_Bundle_Day, rect_Bundle_Day_Back }, new Vector2[2] { new Vector2(0, -135), new Vector2(1080, -135) }, 8));
                    }
                    else if (dd.ratio_x < -0.3f)
                    {
                        Manager_Common.StartCoroutine(ref cor_rect_Month_Bundle_Day, Manager.instance.manager_Ui.Cor_Pos_Anchored(new RectTransform[2] { rect_Bundle_Day, rect_Bundle_Day_Back }, new Vector2[2] { new Vector2(0, -135), new Vector2(-1080, -135) }, 8));
                    }
                }
            }
        }

        /// <summary>
        /// 다음달로 이동
        /// </summary>
        /// <param name="_day"></param>
        public void OnMoveMonth(Prefab_Day _day)
        {
            int origin_year = date_Year_Month.Year;
            date_Year_Month = new DateTime(_day.Date.Year, _day.Date.Month, 1);
            date_Year_Month_Day = _day.Date;
            int cur_year = date_Year_Month.Year;
            if (!origin_year.Equals(cur_year))
            {
                Timing.RunCoroutine(Cor_Load_Data_Set_Days());
            }
            else
                Set_Month();
        }

        [Header("메뉴")]
        [SerializeField] private RectTransform rect_Bundle_Menu_Page;
        [SerializeField] private Prefab_Day month_Prefab_Day;
        [SerializeField] private Prefab_Day menu_Prefab_Day;
        [SerializeField] private RectTransform rect_Bundle_Menu;

        [SerializeField] private Text text_Menu_Check;
        [SerializeField] private Text text_Menu_Record;

        CoroutineHandle cor_Show_Menu;
        /// <summary>
        /// 메뉴 나오기
        /// </summary>
        public void Show_Menu(Prefab_Day _day)
        {
            if (string.IsNullOrWhiteSpace(cur_Todo.id)) return;
            rect_Bundle_Menu_Page.gameObject.SetActive(true);
            month_Prefab_Day = _day;
            menu_Prefab_Day.transform.position = _day.transform.position;
            menu_Prefab_Day.Set_Data(_day.Date, _day.IsThisMonth, _day.IsToday, _day.Data_ToDo_Done_Id, _day.Data_ToDo_Record_Id, _day.Week);

            text_Menu_Check.text = menu_Prefab_Day.Data_ToDo_Done_Id.Equals(-1) ? Manager_Language.Get_Text("체크하기") : Manager_Language.Get_Text("체크해제");
            text_Menu_Record.text = menu_Prefab_Day.Data_ToDo_Record_Id.Equals(-1) ? Manager_Language.Get_Text("기록물 작성") : Manager_Language.Get_Text("기록물 수정");

            Manager_Common.StartCoroutine(ref cor_Show_Menu, Manager.instance.manager_Ui.Cor_Scale_Anchored(rect_Bundle_Menu, Vector3.one));
        }

        /// <summary>
        /// 메뉴 끄기
        /// </summary>
        public void Hide_Menu()
        {
            month_Prefab_Day = null;
            rect_Bundle_Menu_Page.gameObject.SetActive(false);
            rect_Bundle_Menu.localScale = new Vector3(1, 0, 1);
        }

        /// <summary>
        /// 메뉴에서 체크하기
        /// </summary>
        public void OnClickButton_Menu_Check()
        {
            Manager.instance.manager_Common.Start_Loading();
            if (menu_Prefab_Day.Data_ToDo_Done_Id.Equals("-1"))
            {
                Manager.instance.manager_Data.Todo_Done_Insert(cur_Todo.id, $"{date_Year_Month_Day.Year}-{date_Year_Month_Day.Month}-{menu_Prefab_Day.Date}", (string _return) =>
                {

                    menu_Prefab_Day.Set_Done(_return);
                    month_Prefab_Day.Set_Done(_return);
                    Manager.instance.manager_Common.Stop_Loading();
                });
            }
            else
            {
                Manager.instance.manager_Data.Todo_Done_Delete(month_Prefab_Day.Data_ToDo_Done_Id, () =>
                {

                    menu_Prefab_Day.Set_Done("-1");
                    month_Prefab_Day.Set_Done("-1");
                    Manager.instance.manager_Common.Stop_Loading();
                });
            }
            text_Menu_Check.text = menu_Prefab_Day.Data_ToDo_Done_Id.Equals(-1) ? Manager_Language.Get_Text("체크하기") : Manager_Language.Get_Text("체크해제");
        }

        /// <summary>
        /// 메뉴에서 기록물 넘어가기
        /// </summary>
        public void OnClickButton_Menu_Record()
        {
            OnClickButton_Open_Record();
            Hide_Menu();
        }
        #endregion

        #region 3. Subpage_Week
        [Header("주")]
        [Tooltip("Subpage_Week")]
        [SerializeField] private RectTransform rect_Bundle_Week;
        [Tooltip("Subpage_Week_Back")]
        [SerializeField] private RectTransform rect_Bundle_Week_Back;
        [Tooltip("Week_Days")]
        [SerializeField] private List<Prefab_Day> prefab_Days_Week;
        [Tooltip("Week_Days_Back")]
        [SerializeField] private List<Prefab_Day> prefab_Days_Week_Back;
        [Tooltip("Subpage_Week_FirstPoint")]
        [SerializeField] private RectTransform rect_Bundle_Week_FirstPoint;
        [Tooltip("Subpage_Week_FirstPoint_Back")]
        [SerializeField] private RectTransform rect_Bundle_Week_FirstPoint_Back;
        [Tooltip("월간으로 이동하는 버튼")]
        [SerializeField] private GameObject go_Month_Button;
        [Tooltip("요일 표시 화살표")]
        [SerializeField] private RectTransform rect_Week_Arrow;
        [Tooltip("드래그 위치 감지")] private Vector2 prev_Pos;
        [Tooltip("선택 된 주 Prefab_Day")] [SerializeField] private Prefab_Day cur_Week_Prefab_Day;
        private void Start_Week()
        {
            ratio = 405f / 1593;
        }

        /// <summary>
        /// 월간으로 이동
        /// </summary>
        public void OnClickButton_Go_Month_From_Week()
        {
            calender_Mode = Calender_Mode.Month;
            Active_Month_Week(true);
            Change_Button(true);
            cur_Prefab_Day.Select(true);
            rect_Week_Arrow.gameObject.SetActive(false);
            Manager_Common.StartCoroutine(ref cor_Month_Week_Move, Manager.instance.manager_Ui.Cor_Pos_Anchored(cur_Month_Week, new Vector2(0, -cur_Month_Week.transform.GetSiblingIndex() * 135)));
            Manager_Common.StartCoroutine(ref cor_Week_Move, Manager.instance.manager_Ui.Cor_Pos_Anchored(rect_Bundle_Week, new Vector2(1080, -720)));
            Manager_Common.StartCoroutine(ref cor_Week_Select_Move, Manager.instance.manager_Ui.Cor_Pos_Anchored(rect_Bundle_Week_FirstPoint, new Vector2(0, 0)));
        }

        /// <summary>
        /// 주 설정하기
        /// </summary>
        private void Set_Week(bool direct = false)
        {
            if(direct)
            {
                cur_Month_Week.anchoredPosition = new Vector2(0, 50);
            }
            else
            {
                Manager_Common.StartCoroutine(ref cor_Month_Week_Move, Manager.instance.manager_Ui.Cor_Pos_Anchored(cur_Month_Week, new Vector2(0, 50)));
                Manager_Common.StartCoroutine(ref cor_Week_Move, Manager.instance.manager_Ui.Cor_Pos_Anchored(rect_Bundle_Week, new Vector2(0, -720)));
                    
                Manager_Common.StartCoroutine(ref cor_Week_Select_Move, Manager.instance.manager_Ui.Cor_Pos_Anchored(rect_Bundle_Week_FirstPoint, new Vector2(-cur_Prefab_Day.transform.GetSiblingIndex() * 531, 0)));
            }

            Set_Week_Data();
        }

        private void Set_Week_Data() {
            if (cur_Week_Prefab_Day != null && cur_Week_Prefab_Day.IsSelect)
                cur_Week_Prefab_Day.Select(false);

            cur_Week_Prefab_Day = prefab_Days_Week[cur_Prefab_Day.transform.GetSiblingIndex()];
            cur_Week_Prefab_Day.Select(true);
            rect_Week_Arrow.anchoredPosition = new Vector2((cur_Prefab_Day.transform.GetSiblingIndex() - 3) * 135, 0);
            rect_Week_Arrow.gameObject.SetActive(true);
            int _start_Index = 7 * cur_Month_Week.transform.GetSiblingIndex();
            for (int i = _start_Index; i < _start_Index + 7; i++)
            {
                Prefab_Day _pd = month_Days[i];
                prefab_Days_Week[i - _start_Index].Set_Data(_pd.Date, _pd.IsThisMonth, _pd.IsToday, _pd.Data_ToDo_Done_Id, _pd.Data_ToDo_Record_Id, _pd.Week);
            }
        }

        [SerializeField] private bool isDragEnd_Left;
        [SerializeField] private bool isDragEnd_Right;
        List<Vector2> drag_Week_Pos = new List<Vector2>();
        DateTime date_Drag_Week;
        /// <summary>
        /// 드래그 시작
        /// </summary>
        /// <param name="eventData"></param>
        public void OnBeginDrag_Week(BaseEventData eventData)
        {
            date_Drag_Week = date_Year_Month_Day;
            prev_Pos = ((PointerEventData)eventData).position;
            drag_Week_Pos.Add(prev_Pos);
            isDragEnd_Left = rect_Bundle_Week_FirstPoint.anchoredPosition.x > -5;
            isDragEnd_Right = rect_Bundle_Week_FirstPoint.anchoredPosition.x < -3181;
            if (isDragEnd_Left || isDragEnd_Right)
                Manager.instance.manager_Common.BeganDrag(prev_Pos);
        }

        /// <summary>
        /// 드래그
        /// </summary>
        /// <param name="baseEventData"></param>
        public void OnDrag_Week(BaseEventData eventData)
        {
            Vector2 position = ((PointerEventData)eventData).position;
            float _dis = prev_Pos.x - position.x;
            if(!((isDragEnd_Left && _dis < 0) || (isDragEnd_Right && _dis > 0)))
            {
                if(isDragEnd_Left && _dis > 0)
                    isDragEnd_Left = false;
                if(isDragEnd_Right && _dis < 0)
                    isDragEnd_Right = false;
                rect_Bundle_Week_FirstPoint.anchoredPosition = new Vector2(Mathf.Clamp(rect_Bundle_Week_FirstPoint.anchoredPosition.x - _dis, -3186, 0), 0);
                Set_Drag_Week(true);
            }
            prev_Pos = position;
            drag_Week_Pos.Add(prev_Pos);
        }

        CoroutineHandle cor_EndDrag_Week;
        CoroutineHandle cor_rect_Bundle_Week;
        /// <summary>
        /// 드래그 끝났을 시
        /// </summary>
        /// <param name="baseEventData"></param>
        public void OnEndDrag_Week(BaseEventData eventData)
        {
            Vector2 position = ((PointerEventData)eventData).position;
            if (!(isDragEnd_Left || isDragEnd_Right))
            {
                float _dis = prev_Pos.x - position.x;
                if(_dis.Equals(0f))
                {
                    for (int i = drag_Week_Pos.Count - 1; i >= 0; i--)
                    {
                        _dis = drag_Week_Pos[i].x - position.x;
                        if (!_dis.Equals(0f))
                        {
                            break;
                        }
                    }
                }
                if (cor_Week_Select_Move.IsRunning) Timing.KillCoroutines(cor_Week_Select_Move);
                Manager_Common.StartCoroutine(ref cor_EndDrag_Week, Cor_EndDrag_Week(_dis, () => { Set_Drag_Week(true); }));
            }
            else
            {
                if (cor_rect_Bundle_Week.IsRunning) return;
                if (cor_EndDrag_Week.IsRunning) Timing.KillCoroutines(cor_EndDrag_Week);
                Manager_Common.Data_Drag dd = Manager.instance.manager_Common.EndDrag(position);
                if (Mathf.Abs(dd.ratio_y) < 0.1f && dd.time < 0.5f)
                {
                    DateTime after_Date = new DateTime();

                    if (dd.ratio_x > 0.3f)
                    {
                        rect_Bundle_Week_FirstPoint.anchoredPosition = new Vector2(-3186, 0);
                        rect_Bundle_Week.anchoredPosition = new Vector2(-1080, -720);
                        after_Date = cur_Prefab_Day.Date.AddDays(-1);
                    }
                    else if (dd.ratio_x < -0.3f)
                    {
                        rect_Bundle_Week_FirstPoint.anchoredPosition = Vector2.zero;
                        rect_Bundle_Week.anchoredPosition = new Vector2(1080, -720);
                        after_Date = cur_Prefab_Day.Date.AddDays(1);
                    }
                    else
                    { 
                        Clear_Pos(); 
                        return; 
                    }
                    for (int i = 0; i < 7; i++)
                    {
                        Prefab_Day _pd = prefab_Days_Week[i];
                        prefab_Days_Week_Back[i].Set_Data(_pd.Date, _pd.IsThisMonth, _pd.IsToday, _pd.Data_ToDo_Done_Id, _pd.Data_ToDo_Record_Id, _pd.Week);
                    }
                    rect_Bundle_Week_Back.anchoredPosition = rect_Bundle_Week.anchoredPosition;
                    rect_Bundle_Week_FirstPoint_Back.anchoredPosition = rect_Bundle_Week_FirstPoint.anchoredPosition;

                    //현재 주 원위치
                    cur_Month_Week.anchoredPosition = new Vector2(0, -cur_Month_Week.transform.GetSiblingIndex() * 135);
                    cvg_Month_Week.alpha = 0;
                    int _after_month = after_Date.Month;
                    int _after_year = after_Date.Year;
                    if (date_Year_Month.Month.Equals(_after_month))//달 동일
                    {
                        date_Year_Month_Day = after_Date;
                        Set();
                    }
                    else//달 변경
                    {
                        int _origin_year = date_Year_Month.Year;
                        date_Year_Month = new DateTime(after_Date.Year, after_Date.Month, 1);
                        date_Year_Month_Day = after_Date;
                        if (_origin_year.Equals(_after_year))//년 동일
                        {
                            Set_Month();
                            Set();
                        }
                        else//년 변경
                        {
                            Timing.RunCoroutine(Cor_Load_Data_Set_Days(null, () =>
                            {
                                Set();
                            }));
                        }
                    }

                    void Set()
                    {
                        Select_Cur_Day_Week_Drag(Find_Prefab_Day_Month(date_Year_Month_Day), false);
                        cvg_Month_Week.alpha = 1;
                        Set_Week(true);
                        Set_Pos();
                    }

                    void Set_Pos()
                    {
                        if (dd.ratio_x > 0.3f)
                        {
                            Manager_Common.StartCoroutine(ref cor_rect_Bundle_Week, Manager.instance.manager_Ui.Cor_Pos_Anchored(new RectTransform[3] { rect_Bundle_Week, rect_Bundle_Week_Back, rect_Bundle_Week_FirstPoint_Back }, new Vector2[3] { new Vector2(0, -720), new Vector2(1080, -720), Vector2.zero }, 8));
                        }
                        else if (dd.ratio_x < -0.3f)
                        {
                            Manager_Common.StartCoroutine(ref cor_rect_Bundle_Week, Manager.instance.manager_Ui.Cor_Pos_Anchored(new RectTransform[3] { rect_Bundle_Week, rect_Bundle_Week_Back, rect_Bundle_Week_FirstPoint_Back }, new Vector2[3] { new Vector2(0, -720), new Vector2(-1080, -720), new Vector2(-3186, 0) }, 8));
                        }
                    }
                }
            }
            Clear_Pos();
            void Clear_Pos()
            {
                drag_Week_Pos.Clear();
            }
        }

        int hour_Dis;
        /// <summary>
        /// 드래그시 설정하기
        /// </summary>
        private void Set_Drag_Week(bool _set_Cur_Day)
        {
            Set_Arrow_Pos();

            float _Min = 100000;
            Prefab_Day _pd = null;
            for (int i = 0; i < prefab_Days_Week.Count; i++)
            {
                float _dis = Mathf.Abs(prefab_Days_Week[i].transform.position.x - rect_Bundle_Week.position.x);

                if (_dis < _Min)
                {
                    _Min = _dis;
                    _pd = prefab_Days_Week[i];
                }
            }

            if (_pd != null)
            {
                if (cur_Week_Prefab_Day != null)
                {
                    if (cur_Week_Prefab_Day.Equals(_pd))
                        return;
                    else
                        cur_Week_Prefab_Day.Select(false);
                }
                if (cur_Week_Prefab_Day.Date.Year.Equals(_pd.Date.Year))//년 동일
                {
                    if (!cur_Week_Prefab_Day.Date.Month.Equals(_pd.Date.Month))//달 변경
                    {
                        //현재 주 원위치
                        cur_Month_Week.anchoredPosition = new Vector2(0, -cur_Month_Week.transform.GetSiblingIndex() * 135);
                        cvg_Month_Week.alpha = 0;

                        Set_Day();
                        date_Year_Month = new DateTime(_pd.Date.Year, _pd.Date.Month, 1);
                        date_Year_Month_Day = _pd.Date;
                        Set_Month();
                        this.Set_Week(true);
                        Set_Week();
                    }
                    else//달 동일
                    {
                        Set_Day();
                        Set_Week();
                    }
                }
                else//년 변경
                {
                    //현재 주 원위치
                    cur_Month_Week.anchoredPosition = new Vector2(0, -cur_Month_Week.transform.GetSiblingIndex() * 135);
                    cvg_Month_Week.alpha = 0;

                    date_Year_Month = new DateTime(_pd.Date.Year, _pd.Date.Month, 1);
                    date_Year_Month_Day = _pd.Date;
                    Timing.RunCoroutine(Cor_Load_Data_Set_Days(null, () =>
                    {
                        Set_Day();
                        this.Set_Week(true);
                        Set_Week();
                    }));
                }
                void Set_Day()
                {
                    if (cur_Week_Prefab_Day != null && cur_Week_Prefab_Day.IsSelect)
                        cur_Week_Prefab_Day.Select(false);
                    cur_Week_Prefab_Day = _pd;
                    cur_Week_Prefab_Day.Select(true);
                }

                void Set_Week()
                {
                    int _start_Index = 7 * cur_Month_Week.transform.GetSiblingIndex();
                    Select_Cur_Day_Week(month_Days[_start_Index + cur_Week_Prefab_Day.transform.GetSiblingIndex()], false, _set_Cur_Day);
                    Active_Month_Week(false, true);
                    cvg_Month_Week.alpha = 1;
                }
            }
        }

        float ratio;
        private void Set_Arrow_Pos()
        {
            rect_Week_Arrow.anchoredPosition = new Vector2((-rect_Bundle_Week_FirstPoint.anchoredPosition.x - 1593) * ratio, 0);
        }

        /// <summary>
        /// 드래그 끝났을 시
        /// </summary>
        private IEnumerator<float> Cor_EndDrag_Week(float _dis, UnityAction _action)
        {
            bool isLeft = _dis < 0 ? true : false;
            while (!(Mathf.Abs(_dis) < Time.smoothDeltaTime * 500))
            {
                rect_Bundle_Week_FirstPoint.anchoredPosition = new Vector2(Mathf.Clamp(rect_Bundle_Week_FirstPoint.anchoredPosition.x - _dis, -3186, 0), 0);
                _dis = Mathf.Lerp(_dis, 0, Time.smoothDeltaTime * 10);
                Set_Arrow_Pos();
                yield return Timing.WaitForSeconds(0);
            }

            float _d = rect_Bundle_Week_FirstPoint.anchoredPosition.x % 531;
            float _m = 0;

            if (isLeft)
            {
                _m = -_d;
                //_m is plus
            }
            else
            {
                _m = -(531 + _d);
                //_m is minus
            }

            while (!(Mathf.Abs(_m) < 0.1f))
            {
                float _move = Time.smoothDeltaTime * 500;
                if (Mathf.Abs(_m) < _move)
                {
                    rect_Bundle_Week_FirstPoint.anchoredPosition = new Vector2(Mathf.Clamp(rect_Bundle_Week_FirstPoint.anchoredPosition.x + _m, -3186, 0), 0);
                    _m -= _m;
                }
                else
                {
                    if (_m > 0)
                    {
                        _m -= _move;
                        rect_Bundle_Week_FirstPoint.anchoredPosition = new Vector2(Mathf.Clamp(rect_Bundle_Week_FirstPoint.anchoredPosition.x + _move, -3186, 0), 0);
                    }
                    else
                    {
                        _m += _move;
                        rect_Bundle_Week_FirstPoint.anchoredPosition = new Vector2(Mathf.Clamp(rect_Bundle_Week_FirstPoint.anchoredPosition.x - _move, -3186, 0), 0);
                    }
                }
                Set_Arrow_Pos();
                yield return Timing.WaitForSeconds(0);
            }
            _action?.Invoke();
        }

        /// <summary>
        /// 주에서 날짜 찾기
        /// </summary>
        /// <param name="_md"></param>
        /// <returns></returns>
        public Prefab_Day Find_Prefab_Day_Week(DateTime _md)
        {
            return prefab_Days_Week.Find(n => n.Date.Equals(_md));
        }

        #endregion

        #region 4. 할 일 리스트
        [Header("할 일")]
        [Tooltip("할 일 전체 페이지")]
        [SerializeField] private Page page_Todo_Page;
        [Tooltip("할 일 리스트 페이지")]
        [SerializeField] private Page Page_Todo_List_Page;

        [Tooltip("할 일 프리팹")]
        [SerializeField] private GameObject go_Prefab_Todo;
        [Tooltip("할 일 프리팹 부모")]
        [SerializeField] private RectTransform rect_Todo_Content;

        [Tooltip("생성 된 할 일 프리팹")]
        [SerializeField] private List<Prefab_Todo> prefab_Todos = new List<Prefab_Todo>();
        public List<Prefab_Todo> Prefab_Todos { get { return prefab_Todos; } }

        [Tooltip("선택 된 할 일")]
        [SerializeField] private Prefab_Todo cur_Select_Prefab_Todo;

        [Tooltip("편집중인지 여부")]
        [SerializeField] private bool b_Todo_Edit; public bool B_Todo_Edit { get { return b_Todo_Edit; } }

        /// <summary>
        /// 할 일 페이지 클릭
        /// </summary>
        public void OnClickButton_Open_Todo_Page(bool _active)
        {
            page_Todo_Page.Active(_active);
            Page_Todo_List_Page.Active(_active);

            End_Edit_Todo();

            if (_active)
            {
                if (prefab_Todos.Count.Equals(0))
                    Instan_Todo();
                else
                    Set_Todo();
            }
        }

        #region 드래그 해서 페이지 켜기/끄기
        /// <summary>
        /// 드래그 시작해서 페이지 켜기
        /// </summary>
        /// <param name="_base"></param>
        public void OnBeganDrag_Todo_Page(BaseEventData _base)
        {
            Manager.instance.manager_Common.BeganDrag(((PointerEventData)_base).position);
        }

        /// <summary>
        /// 드래그 끝나서 페이지 켜기
        /// </summary>
        /// <param name="_base"></param>
        public void OnEndDrag_Open_Todo_Page(BaseEventData _base)
        {
            Manager_Common.Data_Drag dd = Manager.instance.manager_Common.EndDrag(((PointerEventData)_base).position);
            if(dd.ratio_y >= 0.1f && dd.time < 1f)
            {
                OnClickButton_Open_Todo_Page(true);
            }
        }

        /// <summary>
        /// 드래그 끝나서 페이지 끄기
        /// </summary>
        /// <param name="_base"></param>
        public void OnEndDrag_Close_Todo_Page(BaseEventData _base)
        {
            Manager_Common.Data_Drag dd = Manager.instance.manager_Common.EndDrag(((PointerEventData)_base).position);
            if (dd.ratio_y <= -0.1f && dd.time < 1f)
            {
                OnClickButton_Open_Todo_Page(false);
            }
        }
        #endregion

        /// <summary>
        /// 할 일 생성
        /// </summary>
        private void Instan_Todo()
        {
            prefab_Todos = new List<Prefab_Todo>();
            List<Manager_Data.Data_Todo> data_ToDos = Manager.instance.manager_Data.data_ToDos;
            for (int i = 0; i < data_ToDos.Count; i++)
            {
                Instan_Todo(data_ToDos[i]);
            }
            data_ToDos = null;
        }

        /// <summary>
        /// 할 일 생성
        /// </summary>
        public void Instan_Todo(Manager_Data.Data_Todo _todo)
        {
            GameObject pref = Instantiate(go_Prefab_Todo, rect_Todo_Content);
            Prefab_Todo todo = pref.GetComponent<Prefab_Todo>();
            bool _isSelected = !string.IsNullOrEmpty(cur_Todo.id) ? cur_Todo.id.Equals(_todo.id) : false;
            if (_isSelected)
                cur_Select_Prefab_Todo = todo;
            bool _isDone = !Manager.instance.manager_Data.data_ToDo_Dones.FindIndex(n => n.tdid.Equals(_todo.id) && n.date.Year.Equals(date_Year_Month_Day.Year) && n.date.Month.Equals(date_Year_Month_Day.Month) && n.date.Day.Equals(date_Year_Month_Day.Day)).Equals(-1);
            todo.Set_Data(_todo, _isSelected, _isDone);

            prefab_Todos.Add(todo);
        }

        /// <summary>
        /// 할 일 설정
        /// </summary>
        private void Set_Todo()
        {
            List<Manager_Data.Data_Todo_Done> data_ToDo_Dones = Manager.instance.manager_Data.data_ToDo_Dones;
            for (int i = 0; i < prefab_Todos.Count; i++)
            {
                Prefab_Todo todo = prefab_Todos[i];
                bool _isSelected = cur_Todo.id.Equals(todo.Get_Data.id);
                bool _isDone = !data_ToDo_Dones.FindIndex(n => n.tdid.Equals(todo.Get_Data.id) && n.date.Equals(date_Year_Month_Day)).Equals(-1);
                todo.Set_Data(todo.Get_Data, _isSelected, _isDone);
                prefab_Todos[i] = todo;
            }
            data_ToDo_Dones = null;
        }

        /// <summary>
        /// 할 일 설정
        /// </summary>
        public void Set_Todo(Manager_Data.Data_Todo todo)
        {
            Prefab_Todo ptd = prefab_Todos.Find(n => n.Get_Data.id.Equals(todo.id));
            ptd.Set_Data(todo);
            if (ptd.isSelected)
            {
                cur_Todo = todo;
                Set_Month_Todo_Text();
            }
        }

        /// <summary>
        /// 할 일 편집
        /// </summary>
        public void OnClickButton_Edit_Todo()
        {
            b_Todo_Edit = !b_Todo_Edit;
            for (int i = 0; i < prefab_Todos.Count; i++)
            {
                prefab_Todos[i].Show_Delete_Icon(b_Todo_Edit);
            }
        }

        public void End_Edit_Todo() {
            if(b_Todo_Edit)
            {
                OnClickButton_Edit_Todo();
            }
        }

        /// <summary>
        /// 할 일 페이지 열기
        /// </summary>
        public void OnClickButton_Add_Todo()
        {
            Page_Todo _todo = Manager.instance.page_Todo;
            _todo.Active(true, () => { _todo.Set_Mode(Page_Todo.Mode.Insert); _todo.Set_Todo(new Manager_Data.Data_Todo()); });
            End_Edit_Todo();
        }

        /// <summary>
        /// 할 일 선택
        /// </summary>
        /// <param name="_todo"></param>
        public void Select_Todo(Prefab_Todo _todo)
        {
            if (cur_Select_Prefab_Todo)
                cur_Select_Prefab_Todo.Selected(false);
            cur_Select_Prefab_Todo = _todo;
            Set_Cur_Todo(_todo.Get_Data);
            if(Get_Calender_Mode.Equals(Calender_Mode.Week)){
                Set_Week_Data();
            }
            _todo.Selected(true);

            Manager.instance.manager_Common.Save_Regedit("ssh_t13", cur_Todo.id);
        }

        /// <summary>
        /// 할 일 삭제
        /// </summary>
        /// <param name="_todo"></param>
        public void Delete_Todo(Prefab_Todo _todo)
        {
            Manager.instance.manager_Data.Todo_Delete(_todo.Get_Data.id);
            Destroy(_todo.gameObject);
            prefab_Todos.Remove(_todo);
            if (cur_Select_Prefab_Todo == _todo)
            {
                if (prefab_Todos.Count > 0)
                {
                    cur_Select_Prefab_Todo = prefab_Todos[0];
                    Set_Cur_Todo(cur_Select_Prefab_Todo.Get_Data);
                    cur_Select_Prefab_Todo.Selected(true);
                }
                else
                    Set_Cur_Todo(new Manager_Data.Data_Todo());
            }
        }

        #endregion
    }
}