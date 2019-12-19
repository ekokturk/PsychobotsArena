// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;
#if UNITY_EDITOR
// Custom editor script for game settings scriptable object
[CustomEditor(typeof(GameSettings))]
public class GameSettingsEditor : Editor
{

    private int             _mainToolbarIndex       = 0;
    private int             _prefabsToolbarIndex    = 0;
    private GameSettings    _gameSettings           = null;

    // Editor styles
    private GUIStyle _headerLabelStyle  = null;
    private GUIStyle _imageStyle        = null;
    private GUIStyle _regularLabelStyle = null; 
    private GUIStyle _richLabelStyle    = null; 

    // Editor Images
    private string _robotIconPath       = "Editor/Robot_EditorIcon";
    private string _controllerIconPath  = "Editor/Controller_EditorIcon";

    private void SetupStyles() 
    {
        _headerLabelStyle = new GUIStyle(GUI.skin.label){
                                    alignment = TextAnchor.MiddleCenter,
                                    fontStyle = FontStyle.Bold,
                                    richText = true
                            };

        _regularLabelStyle = new GUIStyle(GUI.skin.label){
                                    alignment = TextAnchor.MiddleCenter,
                                    fontStyle = FontStyle.Bold,
                                    richText = true
                            };

        _richLabelStyle = new GUIStyle(GUI.skin.label){
                                    richText = true
                            };

        _imageStyle = new GUIStyle(GUI.skin.label){
                                    alignment = TextAnchor.MiddleCenter,
                            };
    }

    public override void OnInspectorGUI() 
    {
        serializedObject.Update();
        _gameSettings = (GameSettings) target;

        SetupStyles() ;
        string[] mainToolbarText = {"Inspect", "Prefabs", "Setup"};

        _mainToolbarIndex = GUILayout.Toolbar(_mainToolbarIndex,mainToolbarText);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        switch (_mainToolbarIndex)
        {
            case 0:
                CreateInspectTab();
                break;     
            case 1:
                CreatePrefabsTab();
                break;     
            case 2:
                CreateSetupTab();
                break;            
        }

        serializedObject.ApplyModifiedProperties();
    }

    // ================================== INSPECT TAB ====================================
    private void CreateInspectTab()
    {


        EditorGUILayout.BeginVertical();
        // ----------------- AUDIO MIXER ----------------
        EditorGUILayout.LabelField("Audio", _headerLabelStyle);
        SerializedProperty audioMixer = serializedObject.FindProperty("_audioMixer");
        audioMixer.objectReferenceValue = EditorGUILayout.ObjectField(audioMixer.objectReferenceValue, 
                                                                    typeof(AudioMixer), true) as AudioMixer;
        EditorGUILayout.Space();

        // ----------------- CONTROLLERS -----------------
        SerializedProperty controllers = serializedObject.FindProperty("_controllers");
        EditorGUILayout.LabelField("Controller", _headerLabelStyle);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();

        for (int i = 0; i < controllers.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label($"Player {i+1}");
            EditorGUILayout.SelectableLabel(controllers.GetArrayElementAtIndex(i).stringValue, 
                                            EditorStyles.textField, 
                                            GUILayout.Height(EditorGUIUtility.singleLineHeight));
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        Texture2D controllerSprite = Resources.Load<Texture2D>(_controllerIconPath);
        GUILayout.Label(controllerSprite, GUILayout.MaxWidth(80));
        EditorGUILayout.EndHorizontal();

        // ----------------- TEAM COLORS -----------------

        SerializedProperty teamColors = serializedObject.FindProperty("_teamColors");

        EditorGUILayout.LabelField("Team Colors", _headerLabelStyle);
        EditorGUILayout.BeginVertical();
        for (int i = 0; i < teamColors.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label($"Team {i+1}");
            teamColors.GetArrayElementAtIndex(i).colorValue = EditorGUILayout.ColorField(teamColors.GetArrayElementAtIndex(i).colorValue);
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndVertical();
    }

    // ================================== PREFABS TAB ====================================

    private void CreatePrefabsTab()
    {
        string[] prefabsToolbarText = {"Robots", "Components", "Layouts", "Environments"};    // Tab names
        _prefabsToolbarIndex = GUILayout.Toolbar(_prefabsToolbarIndex, prefabsToolbarText);   // Create tabs
        EditorGUILayout.Space();
        if(_prefabsToolbarIndex == 0)
            CreateRobotsList();
        else if(_prefabsToolbarIndex == 1)
            CreateComponentsList();
        else if(_prefabsToolbarIndex == 2)
            CreateLayoutsList();
        else if(_prefabsToolbarIndex == 3)
            CreateEnvironmentsList();
    }

    private void CreateComponentsList()
    {
        SerializedProperty components = serializedObject.FindProperty("_components");
        for (int i = 0; i < components.arraySize; i++)
        {
            SerializedProperty prefab = components.GetArrayElementAtIndex(i).FindPropertyRelative("prefab");
            SerializedProperty name = components.GetArrayElementAtIndex(i).FindPropertyRelative("name");
            EditorGUILayout.BeginHorizontal("HelpBox");
                name.stringValue = EditorGUILayout.TextField(name.stringValue);
                prefab.objectReferenceValue = EditorGUILayout.ObjectField(prefab.objectReferenceValue, 
                                                                          typeof(GameObject), true) as GameObject;

                if(GUILayout.Button("Remove"))
                {
                    components.DeleteArrayElementAtIndex(i);
                }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }
        if(GUILayout.Button("New Component"))
        {
            components.InsertArrayElementAtIndex(components.arraySize);
        }
    }

    private void CreateRobotsList()
    {
        SerializedProperty robots = serializedObject.FindProperty("_robots");
        for (int i = 0; i < robots.arraySize; i++)
        {
            GUILayout.BeginVertical("HelpBox");
                CreateRobot(robots.GetArrayElementAtIndex(i));
                if(GUILayout.Button("Remove"))
                {
                    robots.DeleteArrayElementAtIndex(i);
                }
            EditorGUILayout.Space();
            GUILayout.EndVertical();
            EditorGUILayout.Space();
        }
        EditorGUILayout.Space();
        if(GUILayout.Button("New Robot"))
        {
            robots.InsertArrayElementAtIndex(robots.arraySize);
        }
    }

    private void CreateLayoutsList()
    {
        SerializedProperty layouts = serializedObject.FindProperty("_layouts");
        for (int i = 0; i < layouts.arraySize; i++)
        {
            SerializedProperty layout = layouts.GetArrayElementAtIndex(i);
            GUILayout.BeginVertical("HelpBox");
                layout.objectReferenceValue = EditorGUILayout.ObjectField(layout.objectReferenceValue, 
                                                                          typeof(GameObject), true) as GameObject;

                if(GUILayout.Button("Remove"))
                {
                    layouts.DeleteArrayElementAtIndex(i);
                }
            EditorGUILayout.Space();
            GUILayout.EndVertical();
            EditorGUILayout.Space();
        }
        EditorGUILayout.Space();
        if(GUILayout.Button("New Layout"))
        {
            layouts.InsertArrayElementAtIndex(layouts.arraySize);
        }
    }

    private void CreateEnvironmentsList()
    {
        SerializedProperty environments = serializedObject.FindProperty("_environments");
        for (int i = 0; i < environments.arraySize; i++)
        {
            SerializedProperty environment = environments.GetArrayElementAtIndex(i);
            GUILayout.BeginVertical("HelpBox");
                environment.objectReferenceValue = EditorGUILayout.ObjectField(environment.objectReferenceValue, 
                                                                          typeof(GameObject), true) as GameObject;

                if(GUILayout.Button("Remove"))
                {
                    environments.DeleteArrayElementAtIndex(i);
                }
            EditorGUILayout.Space();
            GUILayout.EndVertical();
            EditorGUILayout.Space();
        }
        EditorGUILayout.Space();
        if(GUILayout.Button("New Environment"))
        {
            environments.InsertArrayElementAtIndex(environments.arraySize);
        }
    }


    // Create a robot with components in the inspector
    private void CreateRobot(SerializedProperty robotStruct)
    {
        // Properties of the robot
        SerializedProperty prefab                   = robotStruct.FindPropertyRelative("prefab");           // Robot prefab
        SerializedProperty name                     = robotStruct.FindPropertyRelative("name");             // Robot Name
        SerializedProperty leftComponent            = robotStruct.FindPropertyRelative("leftComponent");    // Left Component Gameobject
        SerializedProperty leftComponentPrefab      = leftComponent.FindPropertyRelative("prefab");         // Left Component Prefab
        SerializedProperty leftComponentName        = leftComponent.FindPropertyRelative("name");           // Left Component Name
        SerializedProperty rightComponent           = robotStruct.FindPropertyRelative("rightComponent");   // Right Component GameObject
        SerializedProperty rightComponentPrefab     = rightComponent.FindPropertyRelative("prefab");        // RIght Component Prefab
        SerializedProperty rightComponentName       = rightComponent.FindPropertyRelative("name");          // RIght Component Name


        // Main robot prefab
        EditorGUILayout.LabelField(name.stringValue, _headerLabelStyle);
        EditorGUILayout.BeginVertical("box");
        
        name.stringValue = EditorGUILayout.TextField(name.stringValue);
        prefab.objectReferenceValue = EditorGUILayout.ObjectField(prefab.objectReferenceValue,
                                                                  typeof(GameObject), true) as GameObject;

        Texture2D robotSprite = Resources.Load<Texture2D>(_robotIconPath);
        GUILayout.Label(robotSprite, _imageStyle);
        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("Left Component", _regularLabelStyle, GUILayout.MinWidth(80));
                leftComponentName.stringValue = EditorGUILayout.TextField(leftComponentName.stringValue, GUILayout.MinWidth(80));
                leftComponentPrefab.objectReferenceValue = EditorGUILayout.ObjectField(leftComponentPrefab.objectReferenceValue,
                                                                        typeof(GameObject), true, GUILayout.MinWidth(80)) as GameObject;
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("Right Component", _regularLabelStyle, GUILayout.MinWidth(80));
                rightComponentName.stringValue = EditorGUILayout.TextField(rightComponentName.stringValue, GUILayout.MinWidth(80));
                rightComponentPrefab.objectReferenceValue = EditorGUILayout.ObjectField(rightComponentPrefab.objectReferenceValue, 
                                                                            typeof(GameObject), true, GUILayout.MinWidth(80)) as GameObject;
            EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    

    }
   
    // ================================== SETUP TAB ====================================
    private void CreateSetupTab()
    {
        SerializedProperty players = serializedObject.FindProperty("_players");
        for(int i = 0; i < players.arraySize; i++)
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField($"Player{i+1}", _headerLabelStyle);
            EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"<b>Index</b>", _richLabelStyle);
                EditorGUILayout.LabelField(players.GetArrayElementAtIndex(i).FindPropertyRelative("index").intValue.ToString());
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"<b>Team</b>", _richLabelStyle);
                EditorGUILayout.LabelField(players.GetArrayElementAtIndex(i).FindPropertyRelative("team").intValue.ToString());
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"<b>Robot</b>", _richLabelStyle);
                SerializedProperty robotPrefab = players.GetArrayElementAtIndex(i).FindPropertyRelative("robotPrefab");
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.ObjectField(robotPrefab.objectReferenceValue,typeof(GameObject), true);
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"<b>Left Component</b>", _richLabelStyle);
                SerializedProperty leftPrefab = players.GetArrayElementAtIndex(i).FindPropertyRelative("leftComponentPrefab");
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.ObjectField(leftPrefab.objectReferenceValue,typeof(GameObject), true);
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"<b>Right Component</b>", _richLabelStyle);
                SerializedProperty rightPrefab = players.GetArrayElementAtIndex(i).FindPropertyRelative("rightComponentPrefab");
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.ObjectField(rightPrefab.objectReferenceValue,typeof(GameObject), true);
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

        }

    }
}
#endif
