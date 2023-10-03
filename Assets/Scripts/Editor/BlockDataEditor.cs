using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using TH.Core;

public class BlockDataEditor : OdinMenuEditorWindow
{
    [MenuItem("Tools/Block Data")]
    private static void OpenWindow()
    {
        GetWindow<BlockDataEditor>().Show();
    }

    private CreateNewBlockData _createNewBlockData;

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (_createNewBlockData != null)
        {
            DestroyImmediate(_createNewBlockData.blockData);
        }
    }
    
    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();

        _createNewBlockData = new CreateNewBlockData();
        tree.Add("Create New", new CreateNewBlockData());
        tree.AddAllAssetsAtPath("Block Data", "Assets/Resources/SO/Blocks", typeof(BlockDataSO));

        return tree;
    }

    protected override void OnBeginDrawEditors()
    {
        OdinMenuTreeSelection selected = this.MenuTree.Selection;

        SirenixEditorGUI.BeginHorizontalToolbar();
        {
            GUILayout.FlexibleSpace();

            if (SirenixEditorGUI.ToolbarButton("Delete Current"))
            {
                BlockDataSO asset = selected.SelectedValue as BlockDataSO;
                string path = AssetDatabase.GetAssetPath(asset);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();
    }

    public class CreateNewBlockData
    {
        public CreateNewBlockData()
        {
            blockData = ScriptableObject.CreateInstance<BlockDataSO>();
            blockData.blockName = "Block";
            blockData.BuildingType = EBuildingType.Factory;
            blockData.SourceType = ESourceType.Red;
            blockData.blockPrice = new PropertyData(0, 0, 0, 0);
        }

        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public BlockDataSO blockData;

        [Button("Add New Block SO")]
        private void CreateNewData()
        {
            // 새 SO 인스턴스 생성
            AssetDatabase.CreateAsset(blockData, "Assets/Resources/SO/Blocks/" + blockData.blockName + ".asset");
            AssetDatabase.SaveAssets();
        }
    }
}
