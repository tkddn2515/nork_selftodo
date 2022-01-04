using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NORK
{
    public class Alert : UI
    {
        [SerializeField] private Manager_Common.Alert alert;
        internal override void Start()
        {
            base.Start();
        }

        internal override void OnEnable()
        {
            base.OnEnable();
        }
    }
}