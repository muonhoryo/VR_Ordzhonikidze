using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using VROrdzhonikidzevskii.Serialization;

namespace VROrdzhonikidzevskii.Scene
{
    public sealed class TaskProgressShower : MonoBehaviour
    {
        public static TaskProgressShower Instance_ { get; private set; }
        [SerializeField]
        private Text TaskProgressText;
        [SerializeField]
        private WinMessage winMessage;
        [SerializeField]
        private GameObject FindKeyMessageParent;
        [SerializeField]
        private GameObject FindKeyMessagePrefab;

        private List<KeysMap.Key> FoundKeys=new List<KeysMap.Key>();
        public void AddKey(KeysMap.Key key)
        {
            foreach(var k in FoundKeys)
            {
                if (k.Order == key.Order)
                {
                    return;
                }
            }
            FoundKeys.Add(key);
            FoundKeys.Sort(KeysMap.Key.GetOrderComparison());
            IEnumerable<string> text = FoundKeys.Select((key) => key.Word);
            int length = text.Sum(k => k.Length) + text.Count();
            StringBuilder sb = new StringBuilder(length);
            foreach (var k in text)
            {
                sb.Append(k);
                if (sb.Length < sb.Capacity)
                {
                    sb.Append(" ");
                }
            }
            if (FoundKeys.Count >= SerializationManager.CurrentData_.Map.Keys.Length)
            {
                winMessage.ShowMessage(sb.ToString());
                ResetTask();
            }
            else
            {
                TaskProgressText.text = sb.ToString();
                Instantiate(FindKeyMessagePrefab, FindKeyMessageParent.transform);
            }
        }
        public void ResetTask()
        {
            FoundKeys = new List<KeysMap.Key>();
            TaskProgressText.text = "";
        }
        private void Awake()
        {
            Instance_ = this;
        }
    }
}
