using Sirenix.OdinInspector.Editor;
using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using TH.Core;
using UnityEngine;

namespace Editor.Legacy
{
public class DataManager : OdinMenuEditorWindow
{
    private static Type[] typesToDisplay = TypeCache.GetTypesWithAttribute<ManageableDataAttribute>()
        .OrderBy(m => m.Name)
        .ToArray();

    private Type selectedType;

    [MenuItem("Tools/Data Manager")]
    private static void OpenEditor() => GetWindow<DataManager>();

    protected override void OnGUI()
    {
        // draw menu tree for SOs and other assets
        if (EditorGUIUtils.SelectButtonList(ref selectedType, typesToDisplay))
            ForceMenuTreeRebuild();
        
        base.OnGUI();
    }
    
    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        tree.AddAllAssetsAtPath(selectedType.Name, "Assets/", selectedType, true, true);

        return tree;
    }
}
}