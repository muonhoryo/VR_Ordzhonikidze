using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VROrdzhonikidzevskii.Scene
{
    public sealed class FindKeyMessage : MonoBehaviour
    {
        [SerializeField]
        private float HidingDelayTime;
        private static GameObject CurrentMessage;
        private void Awake()
        {
            StartCoroutine(HidingDelay());
        }
        private IEnumerator HidingDelay()
        {
            if (CurrentMessage != null)
                Destroy(CurrentMessage);
            CurrentMessage = gameObject;
            yield return new WaitForSeconds(HidingDelayTime);
            CurrentMessage = null;
            Destroy(gameObject);
        }
    }
}
