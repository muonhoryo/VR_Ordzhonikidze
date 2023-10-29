using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VROrdzhonikidzevskii.Serialization;

namespace VROrdzhonikidzevskii.Scene
{
    public sealed class WinMessage : MonoBehaviour
    {
        [SerializeField]
        private Text MessageText;
        [SerializeField]
        private float ExitTimer;
        public void ShowMessage(string task)
        {
            MessageText.text = task;
            gameObject.SetActive(true);
            StartCoroutine(ExitDelay());
        }
        public void ExitToMainMenu()
        {
            StopAllCoroutines();
            gameObject.SetActive(false);
            SerializationManager.ResetScene();
            Registry.MainMenuObject.SetActive(true);
        }
        private IEnumerator ExitDelay()
        {
            yield return new WaitForSeconds(ExitTimer);
            ExitToMainMenu();
        }
    }
}
