using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MEC;

namespace NORK
{
    public class CustomToggle : UI, IPointerClickHandler
    {
        [Header("컴포넌트")]
        /// <summary>
        /// 아웃라인
        /// </summary>
        [SerializeField] private RectTransform rect_Outline;
        /// <summary>
        /// 내부 Fill
        /// </summary>
        [SerializeField] private RectTransform rect_Fill;
        /// <summary>
        /// 가운데 원
        /// </summary>
        [SerializeField] private RectTransform rect_Circle;

        [Header("색")]
        public Manager_Common.Data_Toggle_Color col_Left;
        public Manager_Common.Data_Toggle_Color col_Right;

        [Header("함수")]
        [SerializeField] private UnityEvent event_Left;
        [SerializeField] private UnityEvent event_Right;

        public bool isOn = false;
        public bool IsOn
        {
            get
            {
                return isOn;
            }
            set
            {
                Select(value);
            }
        }
        private float pos_x = 0;

        internal override void Start()
        {
            base.Start();

            Set_Pos_X();
        }

        private void Set_Pos_X() 
        {
            if(pos_x.Equals(0))
                pos_x = (rect_Outline.rect.width - rect_Circle.rect.width - rect_Outline.rect.height + rect_Circle.rect.height) * 0.5f; 
        }


        CoroutineHandle cor_Select_Pos;
        CoroutineHandle cor_Select_Col_Outline;
        CoroutineHandle cor_Select_Col_Fill;
        CoroutineHandle cor_Select_Col_Circle;
        public void OnPointerClick(PointerEventData eventData)
        {
            isOn = !isOn;
            Select(isOn);
        }

        private void Select(bool _isOn)
        {
            isOn = _isOn;
            Set();
        }

        private void Set()
        {
            Set_Pos_X();
            if (!isOn)
            {
                Manager_Common.StartCoroutine(ref cor_Select_Pos, Manager.instance.manager_Ui.Cor_Pos_Anchored(rect_Circle, new Vector2(-pos_x, 0), 10)); ;
                Manager_Common.StartCoroutine(ref cor_Select_Col_Outline, Manager.instance.manager_Ui.Cor_Color(rect_Outline.GetComponent<Image>(), col_Left.col_Outline));
                Manager_Common.StartCoroutine(ref cor_Select_Col_Fill, Manager.instance.manager_Ui.Cor_Color(rect_Fill.GetComponent<Image>(), col_Left.col_Fill));
                Manager_Common.StartCoroutine(ref cor_Select_Col_Circle, Manager.instance.manager_Ui.Cor_Color(rect_Circle.GetComponent<Image>(), col_Left.col_Circle));
                event_Left?.Invoke();
            }
            else
            {
                Manager_Common.StartCoroutine(ref cor_Select_Pos, Manager.instance.manager_Ui.Cor_Pos_Anchored(rect_Circle, new Vector2(pos_x, 0)));
                Manager_Common.StartCoroutine(ref cor_Select_Col_Outline, Manager.instance.manager_Ui.Cor_Color(rect_Outline.GetComponent<Image>(), col_Right.col_Outline));
                Manager_Common.StartCoroutine(ref cor_Select_Col_Fill, Manager.instance.manager_Ui.Cor_Color(rect_Fill.GetComponent<Image>(), col_Right.col_Fill));
                Manager_Common.StartCoroutine(ref cor_Select_Col_Circle, Manager.instance.manager_Ui.Cor_Color(rect_Circle.GetComponent<Image>(), col_Right.col_Circle));
                event_Right?.Invoke();
            }
        }
        
    }
}