using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MEC;
using System;
using UnityEngine.Events;

namespace NORK
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UI : MonoBehaviour
    {
#if UNITY_EDITOR
        public Manager manager;

        [ContextMenu("`, Get Color")]
        public void Get_Color()
        {
            set_Color = new Manager_Common.Data_Set_Color();
            set_Color.col_1 = new List<MaskableGraphic>();
            set_Color.col_2 = new List<MaskableGraphic>();
            set_Color.col_3 = new List<MaskableGraphic>();
            set_Color.col_4 = new List<MaskableGraphic>();
            set_Color.col_5 = new List<MaskableGraphic>();
            set_Color.col_6 = new List<MaskableGraphic>();
            set_Color.col_7 = new List<MaskableGraphic>();
            set_Color.col_8 = new List<MaskableGraphic>();
            Get_Color(transform);
        }

        private void Get_Color(Transform tf)
        {
            if (tf.GetComponent<Prefab_Day>() != null) return;
            MaskableGraphic m = tf.GetComponent<MaskableGraphic>();
            if (m)
            {
                switch (ColorUtility.ToHtmlStringRGB(m.color))
                {
                    case "FFBD41":
                        set_Color.col_1.Add(m);
                        break;
                    case "FCE6BE":
                        set_Color.col_2.Add(m);
                        break;
                    case "FCF6EA":
                        set_Color.col_3.Add(m);
                        break;
                    case "F7F7F7":
                        set_Color.col_4.Add(m);
                        break;
                    case "F5F5F5":
                        set_Color.col_5.Add(m);
                        break;
                    case "C7C7C7":
                        set_Color.col_7.Add(m);
                        break;
                    case "707070":
                        set_Color.col_7.Add(m);
                        break;
                    case "000000":
                        set_Color.col_8.Add(m);
                        break;
                }
            }
            for (int i = 0; i < tf.childCount; i++)
            {
                Get_Color(tf.GetChild(i));
            }
        }
#endif
        [Header("등장 효과")]
        private RectTransform rect_UI;
        private CanvasGroup cvg_UI;

        [SerializeField] private Manager_Common.UI_Dissolve show_Dissolve;
        [SerializeField] private Manager_Common.UI_Move show_Move;

        [SerializeField] private Manager_Common.UI_Dissolve hide_Dissolve;
        [SerializeField] private Manager_Common.UI_Move hide_Move;

        internal virtual void Start()
        {
            Set_Color();
        }

        internal virtual void OnEnable()
        {

        }

        /// <summary>
        /// UI 켜기 / 닫기
        /// </summary>
        /// <param name="_active"></param>
        public void Active(bool _active, UnityAction start_Action = null, UnityAction end_Action = null)
        {
            if (_active)
                Appear_Effect(show_Dissolve, show_Move, _active, start_Action, end_Action);
            else
                Appear_Effect(hide_Dissolve, hide_Move, _active, start_Action, end_Action);
        }

        #region 등장 효과
        CoroutineHandle cor_Dissolve;
        CoroutineHandle cor_Move;
        private void Appear_Effect(Manager_Common.UI_Dissolve dissolve, Manager_Common.UI_Move move, bool active, UnityAction start_Action = null, UnityAction end_Action = null)
        {
            Manager.instance.manager_Common.Appear_Effect(gameObject, ref cor_Dissolve, ref cor_Move, dissolve, move, active, start_Action, end_Action);
        }
        #endregion

        #region UI 색상
        
        [Header("색상")]
        [SerializeField] private Manager_Common.Data_Set_Color set_Color;

        /// <summary>
        /// UI 색 변경
        /// </summary>
        internal void Set_Color()
        {
            Manager.instance.manager_Common.Set_Color(set_Color);
        }
        #endregion
    }
}