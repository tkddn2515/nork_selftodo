using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MEC;
namespace NORK
{
    public class Prefab_Alart0 : UI
    {
        public RectTransform rect;
        public Text txt;

        CoroutineHandle cor_Show_Alert0;
        CoroutineHandle cor_Show_Alert0_Move;
        public void Start_Move(string _message, float _showtime)
        {
            gameObject.SetActive(true);
            txt.text = _message;
            rect.sizeDelta = new Vector2(rect.rect.width, txt.preferredHeight + 76);
            Manager_Common.StartCoroutine(ref cor_Show_Alert0, Cor_Show_Alert0(rect, _showtime));
        }

        public void Stop_Move()
        {
            if(cor_Show_Alert0.IsRunning)
                Timing.KillCoroutines(cor_Show_Alert0);
            if(cor_Show_Alert0_Move.IsRunning)
                Timing.KillCoroutines(cor_Show_Alert0_Move);
            Manager_Common.StartCoroutine(ref cor_Show_Alert0_Move, Manager.instance.manager_Ui.Cor_Pos_Anchored(rect, Vector2.zero, 10, null, null,() => { Manager.instance.manager_Common.Enqueue_Alart0(this); gameObject.SetActive(false); }));
        }

        /// <summary>
        /// 버튼 없는 알림창 사라지기
        /// </summary>
        /// <returns></returns>
        IEnumerator<float> Cor_Show_Alert0(RectTransform rect, float _showtime)
        {
            Manager_Common.StartCoroutine(ref cor_Show_Alert0_Move, Manager.instance.manager_Ui.Cor_Pos_Anchored(rect, new Vector2(0, -rect.rect.height - 50)));
            yield return Timing.WaitForSeconds(_showtime);
            Stop_Move();
        }
    }
}