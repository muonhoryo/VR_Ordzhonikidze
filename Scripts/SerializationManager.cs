using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VROrdzhonikidzevskii.Scene;

namespace VROrdzhonikidzevskii.Serialization
{
    [Serializable]
    public enum TeamType
    {
        Red,
        Blue,
        Green
    }
    [Serializable]
    public sealed class KeysMap
    {
        public Key[] Keys;
        public KeysMap(Key[] keys)
        {
            Keys = keys;
        }

        [Serializable]
        public sealed class Key
        {
            public string Word;
            public int Order;
            public Vector3 Position;
            public Vector3 Rotation;
            public static Comparison<Key> GetOrderComparison()=>
                (x,y)=> { return x.Order == y.Order ? 0 : x.Order < y.Order ? -1 : 1; };

            public Key(string word,int Order, Vector3 position, Vector3 rotation)
            {
                Word = word;
                this.Order = Order;
                Position = position;
                Rotation = rotation;
            }
        }
    }
    [Serializable]
    public sealed class TaskData
    {
        public TeamType Team;
        public KeysMap Map;

        public TaskData(TeamType team, KeysMap map)
        {
            Team = team;
            Map = map;
        }
        public TaskData() { }
    }
    public sealed class SerializationManager:MonoBehaviour
    {
        private static SerializationManager Instance;
        private const string SerializationPath = "Assets/Serialization";

        [SerializeField]
        private GameObject KeyPrefab;
        [SerializeField]
        private string KeysTag;
        [SerializeField]
        private Transform KeyCanvasTransform;

        public static string KeysTag_ { get => Instance.KeysTag; }
        public static TaskData CurrentData_ { get; private set; } = null;

        public static void SerializeTask(TaskData taskData)
        {
            string path = SerializationPath + $"/{taskData.Team}.json";
            if (!Directory.Exists(SerializationPath))
                Directory.CreateDirectory(SerializationPath);
            if (!File.Exists(path))
                File.Create(path).Close();
            string serializedTask=JsonUtility.ToJson(taskData,true);
            using(StreamWriter sw=new StreamWriter(path, false))
            {
                sw.Write(serializedTask);
                sw.Close();
            }
        }
        public static void DeserializeTask(TeamType teamType)
        {
#if UNITY_EDITOR
            if(Instance==null)
                Instance = GameObject.FindObjectOfType<SerializationManager>();
#endif
            TaskData data=new TaskData();
            using (StreamReader reader = new StreamReader(SerializationPath + $"/{teamType}.json"))
            {
                JsonUtility.FromJsonOverwrite(reader.ReadToEnd(), data);
            }
            CurrentData_ = data;
            foreach(var key in CurrentData_.Map.Keys)
            {
                Key keyComponent= Instantiate(Instance.KeyPrefab,Instance.KeyCanvasTransform).GetComponent<Key>();
                keyComponent.Initialize(key);
            }
        }
        public static void ResetScene()
        {
            TaskProgressShower.Instance_.ResetTask();
            var objs = GameObject.FindObjectsOfType<Key>();
            foreach(var obj in objs)
            {
                obj.DestroyKey();
            }
        }

        private void Awake()
        {
            Instance = this;
        }
    }
}
