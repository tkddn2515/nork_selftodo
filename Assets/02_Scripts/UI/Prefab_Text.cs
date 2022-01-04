using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NORK
{
    public class Prefab_Text : MonoBehaviour
    {
        public Text text;
        public RectTransform rect;
        bool StartSet;
        public string kr;
        public string en;
        public bool m_Rect;
        private void Awake()
        {
            if (!text)
                text = GetComponent<Text>();
            if (!rect)
                rect = GetComponent<RectTransform>();
            if (string.IsNullOrWhiteSpace(kr))
                kr = text.text;
            StartSet = true;
        }

#if UNITY_EDITOR
        [ContextMenu("`")]
        public void Get_Component()
        {
            text = GetComponent<Text>();
            rect = GetComponent<RectTransform>();
            kr = text.text;
            en = Manager_Language.Get_Text(kr, Manager_Language.language.en);
            Debug.Log(kr + " : " + en);
        }
#endif

        private void Start()
        {
            GetText();
            ChangeText();
            StartSet = false;
        }

        
        private void OnEnable()
        {
            if (StartSet)
                return;
            ChangeText();
        }

        public void GetText()
        {
            if (kr.Equals(""))
                kr = Manager_Language.Get_Text(kr, Manager_Language.language.kr);
            if (en.Equals(""))
                en = Manager_Language.Get_Text(kr, Manager_Language.language.en);
        }

        public void ChangeText()
        {
            switch (Manager_Language.lang)
            {
                case Manager_Language.language.kr:
                    text.text = kr;
                    break;
                case Manager_Language.language.en:
                    text.text = en;
                    break;
            }
            if(m_Rect)
            {
                rect.sizeDelta = new Vector2(text.preferredWidth, text.preferredHeight);
            }
        }
    }
}

