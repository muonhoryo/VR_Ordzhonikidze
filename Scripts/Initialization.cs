using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VROrdzhonikidzevskii.Scene;

namespace VROrdzhonikidzevskii
{
    public sealed  class Initialization : MonoBehaviour
    {
        [SerializeField]
        private GameObject MainMenuObject;
        private void Awake()
        {
            Registry.MainMenuObject = MainMenuObject;
            Destroy(this);
        }
    }
}
