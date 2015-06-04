/*=========================================================
	PARTICLE PRO FX volume one 
	PPFXAutodestruct.cs
	
	Simple auto destruct script. Destroys particle system
	after duration + lifetime is over.
	
	(c) 2014
=========================================================*/

using UnityEngine;
using System.Collections;

public class PPFXAutodestruct : MonoBehaviour {
	
	ParticleSystem ps;
	
	void Start () {
		ps = this.GetComponent<ParticleSystem>();
	 	if(ps)
        {         
            if (!ps.loop) {
		        Destroy(this.gameObject, ps.duration + ps.startLifetime);
		    }
        }
	}
	
	public void DestroyPSystem(GameObject _ps)
	{
		ParticleSystem _pss = _ps.GetComponent<ParticleSystem>();
		Destroy(_ps, _pss.duration + _pss.startLifetime);
	}
}
