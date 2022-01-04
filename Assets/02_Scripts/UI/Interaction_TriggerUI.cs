using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NORK
{
    public class Interaction_TriggerUI : MonoBehaviour
    {
        public UnityEvent<Collider2D> Trigger_Enter;
        public UnityEvent<Collider2D> Trgger_Exit;

        public void OnTriggerEnter2D(Collider2D collision)
        {
            Trigger_Enter?.Invoke(collision);
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            Trgger_Exit?.Invoke(collision);
        }
    }
}