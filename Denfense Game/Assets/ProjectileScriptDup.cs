using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProjectileScriptDup : MonoBehaviour
{
	public bool NoTrail = false;
	public GameObject impactParticle;
	public GameObject projectileParticle;
	public GameObject[] trailParticles;
	public float dmg = 1;
	[HideInInspector]
	public Vector3
		impactNormal; //Used to rotate impactparticle.
	
	
	public float speed = 10f;
	public Transform target;
	
	
	float startTime;
	float dist;
	GameObject Player;
	
	public bool autoDestory = false;
	// Use this for initialization
	void Start ()
	{
		projectileParticle = Instantiate (projectileParticle, transform.position, transform.rotation) as GameObject;
		projectileParticle.transform.parent = transform;
		
		
		startTime = Time.time;
		
		Player = GameObject.FindGameObjectWithTag ("Player");
		
	}
	
	void Update ()
	{
		if (target != null) {
			Vector3 targetDir = target.position - transform.position;
			float step = speed * Time.deltaTime;
			Vector3 newDir = Vector3.RotateTowards (transform.forward, targetDir, step, 0.0F);
			//Debug.DrawRay (transform.position, newDir, Color.red);
			transform.rotation = Quaternion.LookRotation (newDir);
			
			dist = Vector3.Distance (target.position, transform.position);
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / dist;
			transform.position = Vector3.Lerp (transform.position, target.position, fracJourney);
			//transform.position = Vector3.MoveTowards (transform.position, target.position, speed * Time.deltaTime);
			//transform.LookAt (target.position);
			if (autoDestory && transform.position == target.position) {
				Destroy (gameObject);
			}
		}

	}

	void OnTriggerEnter (Collider other)
	{
		if (tag == "Player_weapons") {
			if (other.tag == "Minions") {
				other.gameObject.SendMessage ("DoDamage", dmg);
				impactParticle = Instantiate (impactParticle, transform.position, Quaternion.FromToRotation (Vector3.up, impactNormal)) as GameObject;
				//Debug.DrawRay(hit.contacts[0].point, hit.contacts[0].normal * 1, Color.yellow);
				
				//yield WaitForSeconds (0.05);
				if (!NoTrail) {//yield WaitForSeconds (0.05);
					foreach (GameObject trail in trailParticles) {
						GameObject curTrail;
						if (curTrail = (GameObject)transform.Find (projectileParticle.name + "/" + trail.name).gameObject) {
							curTrail.transform.parent = null;
							Destroy (curTrail, 3f); 
						}
					}
				}
				Destroy (projectileParticle, 3f);
				Destroy (impactParticle, 0.5f);
				Destroy (gameObject);
				//Instantiate (Resources.Load (string.Format ("Hit_{0}", 0)), transform.position, Quaternion.identity);
			}
		}
		if (tag == "Minions_weapons") {
			if (other.tag == "Player") {
				other.gameObject.SendMessage ("DoDamage", dmg);
				impactParticle = Instantiate (impactParticle, transform.position, Quaternion.FromToRotation (Vector3.up, impactNormal)) as GameObject;
				//Debug.DrawRay(hit.contacts[0].point, hit.contacts[0].normal * 1, Color.yellow);


				if (!NoTrail) {//yield WaitForSeconds (0.05);
					foreach (GameObject trail in trailParticles) {
						GameObject curTrail;
						if (curTrail = (GameObject)transform.Find (projectileParticle.name + "/" + trail.name).gameObject) {
							curTrail.transform.parent = null;
							Destroy (curTrail, 3f); 
						}
					}
				}
				Destroy (projectileParticle, 3f);
				Destroy (impactParticle, 0.5f);
				Destroy (gameObject);
				//Instantiate (Resources.Load (string.Format ("Hit_{0}", 0)), transform.position, Quaternion.identity);
			}
		}
	}
	
}