using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VROrdzhonikidzevskii.Scene;
using VROrdzhonikidzevskii.Serialization;

namespace VROrdzhonikidzevskii.MainMenu
{
    public sealed class TeamSelector : MonoBehaviour
    {
        [SerializeField]
        private TeamType teamType;
        public void SelectTeam()
        {
            Registry.MainMenuObject.SetActive(false);
            SerializationManager.DeserializeTask(teamType);
        }
    }
}
