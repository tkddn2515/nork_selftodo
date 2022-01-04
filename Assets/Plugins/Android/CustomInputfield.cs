using MEC;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
namespace Hillis
{
    [RequireComponent(typeof(EventTrigger))]
    public class CustomInputfield : MonoBehaviour
    {
        [HideInInspector] public InputField inputField;

        public InputType inputType;
        public ImeOption imeOption;
        public FontType fontType;

        public UnityEvent onSelect;
        public UnityEvent onBeforeTextChanged;
        public UnityEvent<string> onTextChanged;
        public UnityEvent onAfterTextChanged;
        public UnityEvent onDone;
        
        void Start()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            inputField = GetComponent<InputField>();

            if (inputField != null)
                inputField.enabled = false;

            if(!GetComponent<Image>())
            {
                Image img = gameObject.AddComponent<Image>();
                img.color = new Color(0, 0, 0, 0);
            }

            EventTrigger trigger = GetComponent<EventTrigger>();
            if(!trigger)
                trigger = gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData) => { OnClick(); });
            trigger.triggers.Add(entry);
#endif
        }

        public void OnClick()
        {
            onSelect?.Invoke();
            AndroidInputfield.instance.Set_StatusBar_Height();
            AndroidInputfield.instance.OnClickCustomInputfield(this);
        }
    }
}

