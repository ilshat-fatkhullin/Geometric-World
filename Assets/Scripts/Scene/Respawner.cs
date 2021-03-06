﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Respawner : NetworkBehaviour {
    Spawner spawner;
    SynchronizeManager synchronizeManager;
    public Vector3 spawn;
    bool isAI = false;
    public int AILevel = 0;

	void Start () {
        spawner = GameObject.Find("SceneManager").GetComponent<Spawner>();
        synchronizeManager = gameObject.GetComponent<SynchronizeManager>();
        isAI = gameObject.tag == "AI";
    }
	
	void Update () {
        if (isServer)
        {
            if (transform.position.y < - 10)
            {
                gameObject.GetComponent<Health>().Death();
            }
        }
	}

    public void Respawn()
    {        
        if (!isAI)
        {
            spawn = spawner.GenerateVoidPlace(0);
            synchronizeManager.UpdatePosition(spawn, true);
            gameObject.GetComponent<Health>().HP = Health.MAXHP;
            gameObject.GetComponent<Health>().ARMOR = Health.MAXARMOR;
            gameObject.GetComponent<Health>().startTime = Time.time;
        }
        else
        {
            transform.position = spawner.GenerateVoidPlace(AILevel);
        }
    }
}
