﻿/*=========================================================
	PARTICLE PRO FX volume one 
	PPFXMeteor.cs

	
	(c) 2014
=========================================================*/

using UnityEngine;
using System.Collections;

public class PPFXMeteor : MonoBehaviour {
	
	Vector3 groundPos = new Vector3(0,0,0);
	public Vector3 spawnPosOffset = new Vector3(0,0,0);
	
	public float speed = 10f;
	public GameObject detonationPrefab;
	
	public bool destroyOnHit;
	public bool setRateToNull;
	
	float dist = 0f;
	float radius = 2f;
	
	ParticleSystem[] psystems;
	
	void Start () {
		
		//ground pos is spawn position
		groundPos = this.transform.position;
		//apply position offset
		this.transform.position = this.transform.position + spawnPosOffset;
		 
		//store current distance to ground
		dist = Vector3.Distance(transform.position, groundPos);	
		
		StartCoroutine(Move());
	}
	
	
	IEnumerator Move () {
		
		//quarter up emission rate during flight
		psystems = GetComponentsInChildren<ParticleSystem>(); 
		foreach(ParticleSystem _system in psystems)
		{
			_system.emissionRate *= speed / 10; 
		}
		
		
		while(dist > radius)
        { 
			
        	float step = speed * Time.deltaTime;
    		transform.position = Vector3.MoveTowards(transform.position,groundPos, step);//ref velocity,
        	
        	dist = Vector3.Distance(transform.position, groundPos);	
        	
        	
        	yield return null;
        
        }
        
        
        if(destroyOnHit)
        {
        	Destroy(this.gameObject);
        }
        else if(setRateToNull)
        {
        	//set emission rate to zero
			foreach(ParticleSystem _system in psystems)
			{
				_system.emissionRate = 0f;
				
				
			}
			//destroy this ParticleSystem after duration
			PPFXAutodestruct _ad = this.GetComponent<PPFXAutodestruct>();
			_ad.DestroyPSystem(this.gameObject);
        }
        else
        {
	         //set emission rate back to normal
			foreach(ParticleSystem _system in psystems)
			{
				_system.emissionRate = _system.emissionRate / (speed / 10); 
			}
        }
        
        if(detonationPrefab!=null)
        {
        	Instantiate(detonationPrefab, this.transform.position, detonationPrefab.transform.rotation);
        }
        
        
       
	}
}
