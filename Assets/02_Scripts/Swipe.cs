using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MEC;
using UnityEngine.Events;

namespace NORK
{
    public class Swipe : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler,IEndDragHandler
    {
        private RectTransform rect;
        public RectTransform m_Content;

        public UnityEvent<float> OnMoving;

        private void Start()
        {
            rect = GetComponent<RectTransform>();
        }

        private Vector2 prev_Pos;
        public void OnBeginDrag(PointerEventData eventData)
        {
            prev_Pos = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            float _dis = eventData.position.x - prev_Pos.x;
            m_Content.anchoredPosition = new Vector2(Mathf.Clamp(m_Content.anchoredPosition.x + _dis, -m_Content.rect.width + Screen.width, 0), 0);
            //GetPercent();
            prev_Pos = eventData.position;
        }

        CoroutineHandle cor_EndDrag;
        public void OnEndDrag(PointerEventData eventData)
        {
            float _percente = (eventData.pressPosition.x - eventData.position.x) / Screen.width;
            if (_percente > 0)
            {
                if(_percente > 0.2f)
                    Move("right");
                else
                    Move("rnone");
            }
            else
            {
                if(_percente < -0.2f)
                    Move("left");
                else
                    Move("lnone");
            }
        }
        
        private void Move(string _dir)
        {
            float cur_Position = m_Content.anchoredPosition.x;
            float arrive_Position = 0;
            float width = rect.rect.width;
            float remainder = cur_Position % width;
            switch (_dir)
            {
                case "right":
                case "lnone":
                    arrive_Position = cur_Position - (width + remainder);
                    break;
                case "left":
                case "rnone":
                    arrive_Position = cur_Position - remainder;
                    break;
            }
            Manager_Common.StartCoroutine(ref cor_EndDrag, Manager.instance.manager_Ui.Cor_Pos_Anchored(m_Content, new Vector2(arrive_Position, 0), 10, null, null, null));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (cor_EndDrag.IsRunning)
                Timing.KillCoroutines(cor_EndDrag);
        }

        private void GetPercent()
        {
            float _percent = -m_Content.anchoredPosition.x / m_Content.rect.width;
            OnMoving?.Invoke(_percent);
        }
    }
}
