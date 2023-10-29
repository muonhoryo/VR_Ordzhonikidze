using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using VROrdzhonikidzevskii.Scene;
using VROrdzhonikidzevskii.Serialization;

#if UNITY_EDITOR
namespace VROrdzhonikidzevskii.DevelopmentOnly
{
    public sealed class KeysSerializator : MonoBehaviour
    {
        [SerializeField]
        private TeamType teamType;
        [SerializeField]
        private string KeyTag;

        [ContextMenu("SerializeTask")]
        public void SerializeTask()
        {
            var keyComps = GameObject.FindGameObjectsWithTag(KeyTag);
            List<KeysMap.Key> keys=new List<KeysMap.Key>(keyComps.Select(
                (GameObject obj,int i)=>obj.GetComponent<Key>().GetKeyData()));

            keys.Sort(KeysMap.Key.GetOrderComparison());

            KeysMap map = new KeysMap(keys.ToArray());
            SerializationManager.SerializeTask(new TaskData(teamType, map));
        }
        [ContextMenu("DeserializeTask")]
        public void DeserializeTask()
        {
            SerializationManager.DeserializeTask(teamType);
        }
    }
}
#endif
