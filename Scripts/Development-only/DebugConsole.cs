using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VROrdzhonikidzevskii.Scene;
using VROrdzhonikidzevskii.Serialization;

namespace VROrdzhonikidzevskii.DevelopmentOnly
{
    public sealed class DebugConsole : MonoBehaviour
    {
        private const string Input_DebugConsole="DebugConsole";

        private abstract class ConsoleMode
        {
            public abstract void UpdateAction();
        }
        private sealed class HiddenConsoleMode : ConsoleMode
        {
            public override void UpdateAction()
            {
                if (Input.GetButtonDown(Input_DebugConsole))
                {
                    Instance.CurrentMode = new InputConsoleMode();
                    Instance.OpenConsole();
                }
            }
        }
        private sealed class InputConsoleMode : ConsoleMode
        {
            private void ShowDebugConsoleMessage(string message)
            {
                Debug.Log(message);
            }
            private void SetConfiguration(string[] syntx)
            {
                switch (syntx[0])
                {
                    default:
                        ShowDebugConsoleMessage("Unknown configuration parameter.");
                        break;
                }
            }
            private void ExecuteCommand()
            {
                string[] command = Instance.DebugConsoleField.text.Split(' ');
                switch (command[0])
                {
                    case "JumpMainMenu":
                        SerializationManager.ResetScene();
                        Registry.MainMenuObject.SetActive(true);
                        break;
                    case "Set":
                        SetConfiguration(command.Skip(1).ToArray());
                        break;
                    default:
                        ShowDebugConsoleMessage("Unknown command");
                        break;
                }
            }
            private void HideConsole()
            {
                Instance.CloseConsole();
                Instance.CurrentMode = new HiddenConsoleMode();
            }
            public override void UpdateAction()
            {
                if (Input.anyKeyDown)
                {
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        ExecuteCommand();
                        HideConsole();
                    }
                    else if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        HideConsole();
                    }
                    else if (Input.GetKeyDown(KeyCode.Backspace))
                    {
                        if (Instance.DebugConsoleField.text.Length > 0)
                        {
                            Instance.DebugConsoleField.text =
                                Instance.DebugConsoleField.text.Substring(0, Instance.DebugConsoleField.text.Length - 1);
                        }
                    }
                    else
                    {
                        Instance.DebugConsoleField.text += Input.inputString;
                    }
                }
            }
        }
        private ConsoleMode CurrentMode=new HiddenConsoleMode();

        private static DebugConsole Instance;
        [SerializeField]
        private Text DebugConsoleField;
        [SerializeField]
        private GameObject DebugConsoleObject;
        private void OpenConsole()
        {
            DebugConsoleObject.SetActive(true);
        }
        private void CloseConsole()
        {
            DebugConsoleObject.SetActive(false);
        }
        private void Update()
        {
            CurrentMode.UpdateAction();
        }
        private void Awake()
        {
            Instance = this;
        }
    }
}
