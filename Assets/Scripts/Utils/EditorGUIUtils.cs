#if UNITY_EDITOR
using System;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace TH.Core
{
    public static class EditorGUIUtils
    {
        public static bool SelectButtonList(ref Type selectedType, Type[] typesToDisplay)
        {
            var rect = GUILayoutUtility.GetRect(0, 25);

            for (int i = 0; i < typesToDisplay.Length; i++)
            {
                var name = typesToDisplay[i].Name;
                var btnRect = rect.Split(i, typesToDisplay.Length);

                if (EditorGUIUtils.SelectButton(btnRect, name, typesToDisplay[i] == selectedType))
                {
                    selectedType = typesToDisplay[i];
                    return true;
                }
            }

            return false;
        }

        public static bool SelectButton(Rect rect, string name, bool selected)
        {
            if (GUI.Button(rect, GUIContent.none, GUIStyle.none))
                return true;

            if (Event.current.type == EventType.Repaint)
            {
                var style = new GUIStyle(EditorStyles.miniButtonMid);
                style.stretchHeight = true;
                style.fixedHeight = rect.height;
                style.Draw(rect, GUIHelper.TempContent(name), false, false, selected, false);
            }

            return false;
        }
    }
}
#endif