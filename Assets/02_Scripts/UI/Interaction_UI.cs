using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NORK
{
    public class Interaction_UI : MonoBehaviour
    {
        public UnityEvent Trigger_Enter;
        public UnityEvent Trgger_Exit;

        public void OnTriggerEnter2D(Collider2D collision)
        {
            Trigger_Enter?.Invoke();
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            Trgger_Exit?.Invoke();
        }
    }
}