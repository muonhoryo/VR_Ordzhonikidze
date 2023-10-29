using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using VROrdzhonikidzevskii.Serialization;

namespace VROrdzhonikidzevskii.Scene
{
#if UNITY_EDITOR
    [CustomEditor(typeof(Key))]
    public sealed class Key_Editor : Editor
    {
        /// <summary>
        /// First argument is old order num, second - new order num
        /// </summary>
        private static event Action<int, int> ChangeOrderEvent = delegate { };
        private static event Action<int> DestroyKeyEvent = delegate { };
        private static int NextNum = 0;
        private int Order = -1;
        private int Order_ { get => Order;set{ Order = value;ParsedOwner.Order_ = Order; } }
        private bool IsHandled = false;

        private static bool IsInitializedSystem = false;

        private void DestroyKeyAction(int destroyedNum)
        {
            if (destroyedNum < Order_)
            {
                Order_--;
            }
            else if (destroyedNum == Order_)
            {
                ChangeOrderEvent -= ChangeKeyAction;
                DestroyKeyEvent -= DestroyKeyAction;
            }
        }
        private void ChangeKeyAction(int oldOrder, int newOrder)
        {
            if (!IsHandled)
            {
                if (newOrder == Order_)
                {
                    Order_ = oldOrder;
                }
            }
        }
        private Key ParsedOwner;
        public override VisualElement CreateInspectorGUI()
        {
            ParsedOwner=target as Key;
            if (!IsInitializedSystem)
            {
                NextNum = GameObject.FindObjectsOfType<Key>().SkipWhile((key) => key.Order_ == -1).Count();
                IsInitializedSystem = true;
            }
            if (ParsedOwner.Order_ == -1)
            {
                Order_ = NextNum++;
                ChangeOrderEvent += ChangeKeyAction;
                DestroyKeyEvent += DestroyKeyAction;
                ParsedOwner.DestroyEvent += () =>
                {
                    DestroyKeyEvent(Order_);
                    NextNum--;
                };
            }
            else
            {
                Order_ = ParsedOwner.Order_;
            }
            return base.CreateInspectorGUI();
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.IntField("KeyOrder", Order_+1);
            EditorGUILayout.IntField("KeysInScene",NextNum);
        }

        [ContextMenu("MoveUp")]
        public void MoveUpInOrder()
        {
            if (Order_ < NextNum - 1)
            {
                IsHandled = true;
                ChangeOrderEvent(Order_, ++Order_);
                IsHandled = false;
            }
        }
        [ContextMenu("MoveDown")]
        public void MoveDownInOrder()
        {
            if (Order_ > 0)
            {
                IsHandled = true;
                ChangeOrderEvent(Order_, --Order_);
                IsHandled = false;
            }
        }
    }
#endif
#if UNITY_EDITOR
    [ExecuteInEditMode]
#endif
    public sealed class Key : MonoBehaviour
    {
#if UNITY_EDITOR
        public event Action DestroyEvent = delegate { };
#endif
        public int Order_ { get; set; } = -1;

        [SerializeField]
        private Text KeyText;
        [SerializeField]
        private BoxCollider Collider;

        private KeysMap.Key KeyInfo;
        public void Initialize(KeysMap.Key info)
        {
            transform.position = info.Position;
            transform.eulerAngles = info.Rotation;
            KeyText.text = info.Word;
            Order_ = info.Order;
            KeyInfo = info;
            Collider.size = new Vector3(info.Word.Length * Collider.size.x, Collider.size.y,Collider.size.z);
        }
        public KeysMap.Key GetKeyData()
        {
            return new KeysMap.Key(KeyText.text,Order_, transform.position, transform.eulerAngles);
        }
        public void DestroyKey()
        {
            Destroy(gameObject);
        }

        public void OnMouseDown()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                TaskProgressShower.Instance_.AddKey(KeyInfo);
                Destroy(gameObject);
            }
#else
            TaskProgressShower.Instance_.AddKey(KeyInfo);
            Destroy(gameObject);
#endif
        }
#if UNITY_EDITOR
        private void OnDestroy()
        {
            DestroyEvent();
        }
#endif
    }
}
