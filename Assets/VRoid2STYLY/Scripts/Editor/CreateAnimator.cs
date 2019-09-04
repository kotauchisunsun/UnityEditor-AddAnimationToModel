﻿using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine.Networking;
using System.Collections.Generic;

// Create a menu item that causes a new controller and statemachine to be created.

public class CreateAnimator : MonoBehaviour
{
    static string pathAnimationClip = "Assets/Mixamo/Walking.anim";
    static string modelPath = "Assets/vroid/MyModel.vrm";
    static string prefabPath = "Assets/vroid/MyModel.prefab";
    static string controllerPath = "Assets/vroid/animator.controller";
    static string prefabForUploadPath = "Assets/ForUpload/Prefab.prefab";
    static AnimatorController controller;
    static List<string> fileList = new List<string>();
    static GameObject goFromPrefab;

    public static List<string> FileList
    {
        get
        {
            return fileList;
        }

        set
        {
            fileList = value;
        }
    }

    public static void FindAnimationClips()
    {
        FileList.Clear();
        string[] foldersToSearch = new string[] { "Assets/AnimationClips" };

        string[] guids = AssetDatabase.FindAssets("t:animationclip", foldersToSearch);
        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Debug.Log(path);
            FileList.Add(path);
        }
        //Debug.Log(fileList);
    }

    public static void SetAnimationClipPath(int index)
    {
        if (null != FileList[index])
        {
            pathAnimationClip = FileList[index];
            Debug.Log("Set AnimationClip to:" + pathAnimationClip);
        }
    }


    //[MenuItem("MyMenu/CreateAnimatorController")]
    public static void Execute()
    {
        CreateAnimatorController();
        Import();
        //Load();
        //SetAnimatorController();
    }

    public static void CreateAnimatorController()
    {
        AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(pathAnimationClip);
        Debug.Log("clip:"+clip);
        // Creates the controller
        controller = AnimatorController.CreateAnimatorControllerAtPathWithClip(controllerPath, clip);
        Debug.Log("" + controller);
        //var rootStateMachine = controller.layers[0].stateMachine;
        //var stateA1 = rootStateMachine.AddState("stateA1");
    }

    //[MenuItem("MyMenu/Import file")]
    static void Import()
    {
        string path = EditorUtility.OpenFilePanel("Select file to import", "", "*");
        //FileUtil.CopyFileOrDirectory("D:/Users/yosh8/Documents/STYLY.vrm", modelPath);
        FileUtil.CopyFileOrDirectory(path, modelPath);
        AssetDatabase.Refresh();
        Debug.Log("Now Loading...");
    }

    //[MenuItem("MyMenu/Load prefab to Scene")]
    public static void Load()
    {
        RemoveInstance();

        Debug.Log("in Load() prefabPath: " + prefabPath);
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

        Debug.Log("in Load() prefab: " + prefab);
        goFromPrefab = GameObject.Instantiate(prefab);
    }

    //[MenuItem("MyMenu/Set Animator Controller")]
    public static void SetAnimatorController()
    {
        Animator animator = goFromPrefab.GetComponent<Animator>();
        controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath);
        Debug.Log("controller name:" + controller.name);
        animator.runtimeAnimatorController = controller as RuntimeAnimatorController;

        PrefabUtility.CreatePrefab(prefabForUploadPath, goFromPrefab);
    }

    static void RemoveInstance()
    {
        GameObject go = GameObject.Find("MyModel(Clone)");
        DestroyImmediate(go);
    }
}