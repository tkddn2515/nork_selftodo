using MEC;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NORK
{
    using Data_Todo = Manager_Data.Data_Todo;

    public class Prefab_Todo : UI, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        /// <summary>
        /// 할 일 데이터
        /// </summary>
        [SerializeField] private Data_Todo data;
        public Data_Todo Get_Data { get { return data; } }

        [SerializeField] private RectTransform rect_Delete_Icon;
        [SerializeField] private Text text_ToDo;
        [SerializeField] private RectTransform rect_ToDo;
        [SerializeField] private RectTransform rect_ToDo_Block;
        [SerializeField] private RectTransform rect_Done;
        [SerializeField] private RectTransform rect_Time;
        [SerializeField] private Text text_Day;
        [SerializeField] private Text text_Time;

        [SerializeField] private RectTransform rect_Button;

        public bool isSelected;
        public bool isDone;

        private void OnDestroy()
        {
            Timing.KillCoroutines(GetInstanceID().ToString());
        }

        /// <summary>
        /// 할 일 데이터 적용
        /// </summary>
        public void Set_Data(Data_Todo _data, bool _isSelected = false, bool _isDone = false)
        {
            data = _data;

            Set_Text();
            Manage();
            Selected(_isSelected);
            Done(_isDone);
        }

        /// <summary>
        /// 할 일 데이터 적용
        /// </summary>
        public void Set_Data(Data_Todo _data)
        {
            data = _data;
            Set_Text();
            Manage();
            Done(isDone);
        }

        /// <summary>
        /// 텍스트 적용
        /// </summary>
        private void Set_Text()
        {
            text_ToDo.text = data.name;
            Vector2 text_Size = new Vector2(text_ToDo.preferredWidth, 0);
            rect_ToDo_Block.sizeDelta = rect_ToDo.sizeDelta = text_Size + new Vector2(10, 0);
        }

        /// <summary>
        /// 선택 되었을 시
        /// </summary>
        /// <param name="_isSelected"></param>
        public void Selected(bool _isSelected = false)
        {
            isSelected = _isSelected;
            if (isSelected)
            {
                text_ToDo.font = Manager.instance.font_ExtraLight;
                text_ToDo.color = Manager.instance.col_Main;
            }
            else
            {
                text_ToDo.font = Manager.instance.font_ExtraLight;
                text_ToDo.color = Manager.instance.col_000000;
            }
        }

        /// <summary>
        /// 완료 시
        /// </summary>
        /// <param name="_isDone"></param>
        private void Done(bool _isDone)
        {
            isDone = _isDone;
            rect_Done.gameObject.SetActive(isDone);
            if (isDone)
            {
                rect_Done.sizeDelta = new Vector2(text_ToDo.preferredWidth, 2);
            }
        }

        /// <summary>
        /// 시간
        /// </summary>
        private void Manage()
        {
            //usemanage
            if (data.usemanage.Equals("1"))
            {
                rect_Time.gameObject.SetActive(true);

                List<string> _days = Manager.instance.manager_Common.Days_To_StringList(data);

                text_Day.text = string.Join(" / ", _days);
                text_Time.text = data.managetime.ToString("HH:mm");

                _days = null;
            }
            else
            {
                rect_Time.gameObject.SetActive(false);
            }
        }

        CoroutineHandle cor_Show_Delete_Icon_Text;
        CoroutineHandle cor_Show_Delete_Icon_Block;
        CoroutineHandle cor_Show_Delete_Icon_Scale;
        /// <summary>
        /// 삭제 아이콘 표시
        /// </summary>
        /// <param name="_show"></param>
        public void Show_Delete_Icon(bool _show)
        {
            Vector2 arrivePos = new Vector2(rect_Delete_Icon.anchoredPosition.x + (_show ? rect_Delete_Icon.sizeDelta.x + 30 : 0), 0);

            Manager_Common.StartCoroutine(ref cor_Show_Delete_Icon_Text, Manager.instance.manager_Ui.Cor_Pos_Anchored(rect_ToDo, arrivePos, 10), GetInstanceID().ToString());
            Manager_Common.StartCoroutine(ref cor_Show_Delete_Icon_Block, Manager.instance.manager_Ui.Cor_Pos_Anchored(rect_ToDo_Block, arrivePos, 10), GetInstanceID().ToString());
            Manager_Common.StartCoroutine(ref cor_Show_Delete_Icon_Scale, Manager.instance.manager_Ui.Cor_Scale_Anchored(rect_Delete_Icon, _show ? Vector3.one : new Vector3(0, 0, 1), 10), GetInstanceID().ToString());

            if (!_show)
                Buttons_Size(false);
        }

        /// <summary>
        /// 할 일 클릭
        /// </summary>
        public void OnPointerDown(PointerEventData eventData)
        {
            Manager.instance.manager_Common.PointDown();
        }

        /// <summary>
        /// 할 일 클릭
        /// </summary>
        public void OnPointerUp(PointerEventData eventData)
        {
            if (Manager.instance.manager_Common.PointUp_OneClick())
            {
                if(!Manager.instance.page_Calender.B_Todo_Edit)
                    Manager.instance.page_Calender.Select_Todo(this);
                else
                {
                    Page_Todo _todo = Manager.instance.page_Todo;
                    _todo.Active(true, () => { _todo.Set_Mode(Page_Todo.Mode.Update); Manager.instance.page_Todo.Set_Todo(data); });
                }
            }
        }

        #region 드래그_버튼 켜기
        private Vector2 prev_Pos;
        /// <summary>
        /// 드래그 시작
        /// </summary>
        public void OnBeginDrag(PointerEventData eventData)
        {
            prev_Pos = eventData.position;
        }

        /// <summary>
        /// 드래그
        /// </summary>
        public void OnDrag(PointerEventData eventData)
        {
            if (!Manager.instance.page_Calender.B_Todo_Edit) return;
            float _dis = prev_Pos.x - eventData.position.x;
            rect_Button.sizeDelta = new Vector2(Mathf.Clamp(rect_Button.sizeDelta.x + _dis, 0, 250), 0);
            prev_Pos = eventData.position;
            Update_Button();
        }

        CoroutineHandle cor_EndDrag;
        /// <summary>
        /// 드래그 끝
        /// </summary>
        public void OnEndDrag(PointerEventData eventData)
        {
            if (!Manager.instance.page_Calender.B_Todo_Edit) return;
            float _percente = (eventData.pressPosition.x - eventData.position.x) / Screen.width;
            if(_percente > 0.2f)
            {
                Buttons_Size(true);
                return;
            }
            else if(_percente < -0.2f)
            {
                Buttons_Size(false);
                return;
            }

            if (rect_Button.rect.width > 150)
            {
                Buttons_Size(true);
            }
            else
            {
                Buttons_Size(false);
            }
        }

        private void Buttons_Size(bool _enable)
        {
            if(_enable)
                Manager_Common.StartCoroutine(ref cor_EndDrag, Manager.instance.manager_Ui.Cor_Rect_Anchored(rect_Button, new Vector2(201, 0), 5, Update_Button, Update_Button, Update_Button), GetInstanceID().ToString());
            else
                Manager_Common.StartCoroutine(ref cor_EndDrag, Manager.instance.manager_Ui.Cor_Rect_Anchored(rect_Button, new Vector2(0, 0), 5, Update_Button, Update_Button, Update_Button), GetInstanceID().ToString());
        }

        private void Update_Button()
        {
            rect_Time.anchoredPosition = new Vector2(-rect_Button.sizeDelta.x - 102, 0);
        }
        
        /// <summary>
        /// 삭제버튼 클릭
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerDown_Delete(BaseEventData eventData)
        {
            Manager.instance.manager_Common.PointDown();
        }

        /// <summary>
        /// 삭제버튼 클릭
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerUp_Delete(BaseEventData eventData)
        {
            if (Manager.instance.manager_Common.PointUp_OneClick())
                Manager.instance.page_Calender.Delete_Todo(this);
        }
        #endregion
    }
}