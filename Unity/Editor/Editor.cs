using UnityEditor;
using UnityEngine;
using System;
using System.IO;

namespace EFO.Unity.Editor {
    public static class EditorTools {
        /// <summary>
        /// Add copy-paste functionality to any text field
        /// Returns changed text or NULL.
        /// Usage: text = HandleCopyPaste (controlID) ?? text;
        /// </summary>
        public static string HandleCopyPaste(int controlID)
        {
            if (controlID == GUIUtility.keyboardControl)
            {
                if (Event.current.type == EventType.KeyUp && (Event.current.modifiers == EventModifiers.Control || Event.current.modifiers == EventModifiers.Command))
                {
                    if (Event.current.keyCode == KeyCode.C)
                    {
                        Event.current.Use();
                        TextEditor editor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
                        editor.Copy();
                    }
                    else if (Event.current.keyCode == KeyCode.V)
                    {
                        Event.current.Use();
                        TextEditor editor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
                        editor.Paste();
                        return editor.text;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// TextField with copy-paste support
        /// </summary>
        public static string TextFieldWithCopyPaste(string value, params GUILayoutOption[] options) {
            int textFieldID = GUIUtility.GetControlID("TextField".GetHashCode(), FocusType.Keyboard) + 1;
            if (textFieldID == 0)
                return value;

            // Handle custom copy-paste
            value = HandleCopyPaste(textFieldID) ?? value;

            return GUILayout.TextField(value);
        }


    }

    public static class AssetDatabaseTools {
        public static string GetSaveLocalPath(string dirPattern) {
            string root = Application.dataPath + '/';

            string dir = Directory.GetDirectories(root, dirPattern, SearchOption.AllDirectories)[0];
            dir = dir.Substring(dir.IndexOf("Assets/", StringComparison.CurrentCulture));
            dir = dir.Replace('\\', '/');

            return dir;
        }
    }
}