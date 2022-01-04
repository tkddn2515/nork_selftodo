using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NORK
{
    public class Page : UI
    {
        internal override void Start()
        {
            base.Start();
        }

        internal override void OnEnable()
        {
            base.OnEnable();

            SetOnTop();
        }

        /// <summary>
        /// UI를 가장 위로 보내기
        /// </summary>
        private void SetOnTop()
        {
            transform.SetAsLastSibling();
        }
    }
}