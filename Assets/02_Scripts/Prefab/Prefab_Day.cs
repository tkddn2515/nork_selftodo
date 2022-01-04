using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NORK
{
    public class Prefab_Day : UI, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Page_Calender.Calender_Mode calender_Mode;
        [Header("�� / ��")]
        [Tooltip("Done ������Ʈ")]
        [SerializeField] private GameObject go_Done;
        [Tooltip("��Ϲ� ������Ʈ")]
        [SerializeField] private GameObject go_Record;
        [Tooltip("�ܰ��� ������Ʈ")]
        [SerializeField] private GameObject go_Outline;

        [Header("��")]
        [Tooltip("Done ������Ʈ")]
        [SerializeField] private Image img_Done;
        [Tooltip("üũ")]
        [SerializeField] private Image img_Check;

        [Header("����")]
        /// <summary>
        /// �� �ؽ�Ʈ
        /// </summary>
        [SerializeField] private Text txt_Day;
        /// <summary>
        /// ��
        /// </summary>
        [SerializeField] private DateTime date; public DateTime Date { get { return date; } }

        [SerializeField] private bool isThisMonth; public bool IsThisMonth { get { return isThisMonth; } }
        [SerializeField] private bool isToday;     public bool IsToday { get { return isToday; } }
        [SerializeField] private bool isDone;      public bool IsDone { get { return isDone; } }
        [SerializeField] private bool isRecord;    public bool IsRecord { get { return isRecord; } }
        [SerializeField] private bool isSelect;    public bool IsSelect { get { return isSelect; } }
        [SerializeField] private int week;    public int Week { get { return week; } }

        [SerializeField] private string data_ToDo_Done_Id; public string Data_ToDo_Done_Id { get { return data_ToDo_Done_Id; } }
        [SerializeField] private string data_ToDo_Record_Id; public string Data_ToDo_Record_Id { get { return data_ToDo_Record_Id; } }

        internal override void Start()
        {
            base.Start();
        }


        public void Set_Data(DateTime _date, bool _isThisMonth, bool _isToday, string _isDone_Id, string _isRecord_Id, int _week = -1)
        {
            date = _date;
            txt_Day.text = $"{date.Day}";

            Set_Week(_week);
            Set_ThisMonth(_isThisMonth);
            Set_Today(_isToday);
            Set_Done(_isDone_Id);
            Set_Record(_isRecord_Id);
        }

        private void Set_Week(int _week)
        {
            week = _week;
            
            if(calender_Mode.Equals(Page_Calender.Calender_Mode.Year) || calender_Mode.Equals(Page_Calender.Calender_Mode.Month))
            {

            }
            else
            {
                string _weekstr = Manager_Language.lang.Equals(Manager_Language.language.kr) ? Manager.instance.manager_Common.Get_Day_Three_Char_Kor(week) : Manager.instance.manager_Common.Get_Day_Three_Char_Kor(week);
                txt_Day.text += $" / {_weekstr}";
            }
        }

        /// <summary>
        /// �̹������� üũ
        /// </summary>
        /// <param name="_isThisMonth"></param>
        private void Set_ThisMonth(bool _isThisMonth)
        {
            isThisMonth = _isThisMonth;
            
            if (calender_Mode.Equals(Page_Calender.Calender_Mode.Year) || calender_Mode.Equals(Page_Calender.Calender_Mode.Month))
                txt_Day.color = isThisMonth ? Manager.instance.col_707070 : Manager.instance.col_C7C7C7;
            else
            {

            }
        }

        /// <summary>
        /// �������� üũ
        /// </summary>
        private void Set_Today(bool _isToday)
        {
            isToday = _isToday;
        }

        /// <summary>
        /// �Ϸ�ƴ��� üũ
        /// </summary>
        /// <param name="_isDone"></param>
        public void Set_Done(string _isDone_Id)
        {
            data_ToDo_Done_Id = _isDone_Id;
            isDone = !data_ToDo_Done_Id.Equals("-1");
            Set_Done(isDone);
        }
        public void Set_Done(bool _enable)
        {
            if (calender_Mode.Equals(Page_Calender.Calender_Mode.Year) || calender_Mode.Equals(Page_Calender.Calender_Mode.Month))
            {
                if (go_Done.activeSelf.Equals(_enable)) return;
                go_Done.SetActive(_enable);
                txt_Day.color = isThisMonth ? (isToday ? (_enable ? Color.white : Manager.instance.col_Main) : Manager.instance.col_707070) : Manager.instance.col_C7C7C7;
            }
            else
            {
                Set_Day_Color();
            }
        }

        /// <summary>
        /// ��Ϲ� �ִ��� üũ
        /// </summary>
        public void Set_Record(string _isRecord_Id)
        {
            if (calender_Mode.Equals(Page_Calender.Calender_Mode.Year) || calender_Mode.Equals(Page_Calender.Calender_Mode.Month))
            {
                data_ToDo_Record_Id = _isRecord_Id;
                isRecord = !data_ToDo_Record_Id.Equals("-1");
                go_Record.SetActive(isRecord);
            }
            else
            {

            }
        }

        /// <summary>
        /// �� ����
        /// </summary>
        /// <param name="_isSelect"></param>
        public void Select(bool _isSelect)
        {
            isSelect = _isSelect;
            if (calender_Mode.Equals(Page_Calender.Calender_Mode.Year) || calender_Mode.Equals(Page_Calender.Calender_Mode.Month))
                go_Outline.SetActive(_isSelect);
            else
            {
                Set_Day_Color();
            }
        }

        #region ��
        CoroutineHandle cor_Img_Done;
        CoroutineHandle cor_Img_Check;
        /// <summary>
        /// ����
        /// </summary>
        private void Set_Day_Color()
        {
            if (isDone)
            {
                if (isSelect)
                {
                    Manager_Common.StartCoroutine(ref cor_Img_Done, Manager.instance.manager_Ui.Cor_Color(img_Done, Manager.instance.col_Main));
                    Manager_Common.StartCoroutine(ref cor_Img_Check, Manager.instance.manager_Ui.Cor_Color(new MaskableGraphic[2] { img_Check , txt_Day }, Color.white));
                }
                else
                {
                    Manager_Common.StartCoroutine(ref cor_Img_Done, Manager.instance.manager_Ui.Cor_Color(img_Done, Manager.instance.col_Main_Light_Sub));
                    Manager_Common.StartCoroutine(ref cor_Img_Check, Manager.instance.manager_Ui.Cor_Color(new MaskableGraphic[2] { img_Check, txt_Day }, Manager.instance.col_Main_Light));
                }
            }
            else
            {
                Manager_Common.StartCoroutine(ref cor_Img_Done, Manager.instance.manager_Ui.Cor_Color(img_Done, Manager.instance.col_F7F7F7));
                Manager_Common.StartCoroutine(ref cor_Img_Check, Manager.instance.manager_Ui.Cor_Color(new MaskableGraphic[2] { img_Check, txt_Day }, Color.white));
            }
        }
        #endregion

        public void OnPointerDown(PointerEventData eventData)
        {
            Manager.instance.manager_Common.PointDown();
            if(calender_Mode.Equals(Page_Calender.Calender_Mode.Month) && Manager.instance.page_Calender.Get_Calender_Mode.Equals(Page_Calender.Calender_Mode.Month) && isThisMonth)
                cor_LongClick = Timing.RunCoroutine(Cor_LongClick());
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Timing.KillCoroutines(cor_LongClick);
            if (Manager.instance.page_Calender.Get_Calender_Mode.Equals(Page_Calender.Calender_Mode.Year)) return;
            if(Manager.instance.manager_Common.PointUp_OneClick())
            {
                if (calender_Mode.Equals(Page_Calender.Calender_Mode.Month))
                {
                    if (isThisMonth)
                    {
                        if (Manager.instance.page_Calender.IsSelectDay(this))
                            Manager.instance.page_Calender.Check_Day(this);
                        else
                            Manager.instance.page_Calender.Select_Cur_Day(this);
                    }
                    else
                    {
                        Manager.instance.page_Calender.OnMoveMonth(this);
                    }
                }
                else if(calender_Mode.Equals(Page_Calender.Calender_Mode.Week))
                {
                    Manager.instance.page_Calender.Check_Day(this);
                }
            }
        }

        CoroutineHandle cor_LongClick;
        private IEnumerator<float> Cor_LongClick()
        {
            yield return Timing.WaitForSeconds(1);
            Manager.instance.page_Calender.Show_Menu(this);
        }
    }
}