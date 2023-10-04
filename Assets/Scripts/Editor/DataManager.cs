using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using TH.Core;
using UnityEngine;

public class DataManager : OdinMenuEditorWindow
{
    [OnValueChanged("StateChange")]
    [LabelText("Manager View")]
    [LabelWidth(100f)]
    [EnumToggleButtons]
    [ShowInInspector]
    private ManagerState managerState;
    private int enumIndex = 0;
    private bool treeRebuild = false;
    
    private DrawSelected<BlockDataSO> drawBlockData = new DrawSelected<BlockDataSO>();
    private DrawSelected<PriceDataSO> drawPillarPrice = new DrawSelected<PriceDataSO>();
    private DrawSelected<WaveDataSO> drawWaveData = new DrawSelected<WaveDataSO>();
    private DrawSelected<SpawnDataSO> drawSpawnData = new DrawSelected<SpawnDataSO>();
    
    //paths to SOs in project
    private string blockDataPath = "Assets/Resources/SO/Blocks";
    private string pillarPricePath = "Assets/Resources/SO/Pillar";
    private string waveDataPath = "Assets/Resources/SO/Wave";
    private string spawnDataPath = "Assets/Resources/SO/Spawner";

    [MenuItem("Tools/Data Manager")]
    private static void OpenWindow() => GetWindow<DataManager>();
    
    private void StateChange()
    {
        treeRebuild = true;
    }

    protected override void Initialize()
    {
        drawBlockData.SetPath(blockDataPath);
        drawPillarPrice.SetPath(pillarPricePath);
        drawWaveData.SetPath(waveDataPath);
        drawSpawnData.SetPath(spawnDataPath);
    }

    protected override void OnGUI()
    {
        if(treeRebuild && Event.current.type == EventType.Layout)
        {
            ForceMenuTreeRebuild();
            treeRebuild = false;
        }
        
        switch (managerState)
        {

            case ManagerState.blockData:
            case ManagerState.pillarPrice:
            case ManagerState.waveData:
            case ManagerState.spawnData:
                DrawEditor(enumIndex);
                break;
            default:
                break;
        }
        EditorGUILayout.Space();

        base.OnGUI();
    }
    
    protected override void DrawEditors()
    {
        switch (managerState)
        {
            case ManagerState.blockData:
                drawBlockData.SetSelected(this.MenuTree.Selection.SelectedValue);
                break;
            case ManagerState.pillarPrice:
                drawPillarPrice.SetSelected(this.MenuTree.Selection.SelectedValue);
                break;
            case ManagerState.waveData:
                drawWaveData.SetSelected(this.MenuTree.Selection.SelectedValue);
                break;
            case ManagerState.spawnData:
                drawSpawnData.SetSelected(this.MenuTree.Selection.SelectedValue);
                break;
            /*case ManagerState.sfx:
                DrawEditor(enumIndex);
                break;*/
            default:
                break;
        }

        DrawEditor((int)managerState);
    }

    protected override IEnumerable<object> GetTargets()
    {
        List<object> targets = new List<object>();
        targets.Add(drawBlockData);
        targets.Add(drawPillarPrice);
        targets.Add(drawWaveData);
        targets.Add(drawSpawnData);
        targets.Add(base.GetTarget());

        enumIndex = targets.Count - 1;

        return targets;
    }
    
    protected override void DrawMenu()
    {
        switch (managerState)
        {

            case ManagerState.blockData:
            case ManagerState.pillarPrice:
            case ManagerState.waveData:
            case ManagerState.spawnData:
                base.DrawMenu();
                break;
            default:
                break;
        }
    }
    
    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        
        switch (managerState)
        {

            case ManagerState.blockData:
                tree.AddAllAssetsAtPath("Block Data", blockDataPath, typeof(BlockDataSO));
                break;
            case ManagerState.pillarPrice:
                tree.AddAllAssetsAtPath("Pillar Price", pillarPricePath, typeof(PriceDataSO));
                break;
            case ManagerState.waveData:
                tree.AddAllAssetsAtPath("Wave Data", waveDataPath, typeof(WaveDataSO));
                break;
            case ManagerState.spawnData:
                tree.AddAllAssetsAtPath("Spawn Data", spawnDataPath, typeof(SpawnDataSO));
                break;
            default:
                break;
        }
        
        return tree;
    }
    
    public enum ManagerState
    {
        blockData,
        pillarPrice,
        waveData,
        spawnData
    }
}

public class DrawSelected<T> where T : ScriptableObject
{
    [InlineEditor(InlineEditorObjectFieldModes.CompletelyHidden)]
    public T selected;

    [LabelWidth(100)]
    [PropertyOrder(-1)]
    [ColorGroupAttribute("CreateNew", 1f,1f,1f)]
    [HorizontalGroup("CreateNew/Horizontal")]
    public string nameForNew;

    private string path;

    [HorizontalGroup("CreateNew/Horizontal")]
    [GUIColor(0.7f,0.7f,1f)]
    [Button]
    public void CreateNew()
    {
        if (nameForNew == "")
            return;

        T newItem = ScriptableObject.CreateInstance<T>();
        //newItem.name = "New " + typeof(T).ToString();

        if (path == "")
            path = "Assets/";

        AssetDatabase.CreateAsset(newItem, path + "\\" + nameForNew + ".asset");
        AssetDatabase.SaveAssets();

        nameForNew = "";
    }

    [HorizontalGroup("CreateNew/Horizontal")]
    [GUIColor(1f, 0.7f, 0.7f)]
    [Button]
    public void DeleteSelected()
    {
        if(selected != null)
        {
            string _path = AssetDatabase.GetAssetPath(selected);
            AssetDatabase.DeleteAsset(_path);
            AssetDatabase.SaveAssets();
        }
    }

    public void SetSelected(object item)
    {
        var attempt = item as T;
        if (attempt != null)
            this.selected = attempt;
    }

    public void SetPath(string path)
    {
        this.path = path;
    }
}