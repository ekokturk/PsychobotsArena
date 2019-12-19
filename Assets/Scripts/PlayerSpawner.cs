// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Spawn a player character with a specific configuration and keep track of it during it's lifetime </summary>
public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _spawnedObject  = null;     // GameObject that spawner spawns
    [SerializeField] private GameObject _spawnEffect    = null;     // Particle effect
    // [SerializeField] private int        _Team           = 1;        // Tag index for team            // For team games

    [SerializeField] private PlayerController.PlayerInputTypes    _inputType  = 0;  // Player no for input

    private float _particleDestroyTime = 2.0f;  // Destroy particle on timer

    // =============================== GETTER/SETTERS ===================================

    public GameObject GetSpawned() { return _spawnedObject; }

    // =============================== SPAWN OPERATION ===================================

    public void Spawn(GameObject robot, GameObject leftComp = null, GameObject rightComp = null)
    {
        if(_spawnedObject != null) Destroy(_spawnedObject);     // Destroy spawned object if new will be spawned
        if(_spawnEffect   != null)                              // Check if particle exists                                                                         
        {
            // Spawn particle
            GameObject particle = Instantiate( _spawnEffect, transform.position, _spawnEffect.transform.rotation);   
            Destroy(particle, _particleDestroyTime);           // Destroy particle on timer                                       
        }
        // Spawn player robot
        _spawnedObject = Instantiate(robot, transform.position, transform.rotation);                             

        // Get player controller for configuration
        PlayerController controller = _spawnedObject.GetComponent<PlayerController>();
        if(controller == null) return; 

        controller.InputType = (int)_inputType;
        if(leftComp != null)                        
            controller.AttachLeftComponent(leftComp);           // Add left component if it exists
        if(rightComp != null)
            controller.AttachRightComponent(rightComp);         // Add right component if it exists
        
    }

    // =============================== SPAWN SETUP ===================================
    
    /// <summary> Disable all spawned object motion but yaw rotation </summary>
    public void FreezeSpawned()
    {
        if(_spawnedObject == null) return;                                  // Check if spawned object exists
        Rigidbody rigidbody = _spawnedObject.GetComponent<Rigidbody>();     // Get rigidbody of the spawned
        if( rigidbody != null)                                              // If rigidbody is available freeze transforms
            _spawnedObject.GetComponent<Rigidbody>().constraints =   RigidbodyConstraints.FreezePosition         
                                                                   | RigidbodyConstraints.FreezeRotationX 
                                                                   | RigidbodyConstraints.FreezeRotationZ;  
    }

    /// <summary> Set player tag according to team index (teamNo = 1-4, ex. 'Team4') </summary>
    public void SetTeam(int teamNo)
    {
        if(teamNo < 1)
            teamNo = 1;
        else if(teamNo > 4)
            teamNo = 4;
        _spawnedObject.tag = $"Team{teamNo}";
        
        PlayerController controller = _spawnedObject.GetComponent<PlayerController>();
        if(controller == null) return;
        controller.LeftComponent.gameObject.tag  = $"Team{teamNo}";
        controller.RightComponent.gameObject.tag = $"Team{teamNo}";
    }

    public void SetIndicator(Camera camera, int playerIndex, Color teamColor)
    {
        PlayerIndicatorDisplay display = _spawnedObject.GetComponent<PlayerIndicatorDisplay>();
        if(display == null) return;
        display.ShowDisplay();
        display.CameraToLookAt = camera;
        display.SetPlayerIndicator(playerIndex, teamColor);
    }


}
