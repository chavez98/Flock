using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flee : MonoBehaviour {
	
	public string FleeFromTag;
	public float MinSpeed;
	public float MaxSpeed;
	public float MinFleeRadius;
	public float MaxFleeRadius;
	public float MaxWaypointFromSpawnerRadius;
	public float SensorDistance;	
	
	Transform _closestAttacker;
	Vector3 _waypoint;
	CellSpawner _spawner;
	float _startSpeed;
	float _turnSpeed;
	List<Transform> _closeAttackers = new List<Transform>();



	void Awake()
	{
		_startSpeed = Random.Range(MinSpeed,MaxSpeed);
		_turnSpeed = _startSpeed * .2f;
		GetComponent<SphereCollider>().radius = Random.Range(MinFleeRadius,MaxFleeRadius);
	}
	

	void SetSpawner(CellSpawner spawner)
	{
		_spawner = spawner;	
		
	}
	
	void SetNewWaypoint(Vector3 centerPos)
	{
		
		_waypoint = centerPos + new Vector3(Random.insideUnitCircle.x * MaxWaypointFromSpawnerRadius, 
			0, Random.insideUnitCircle.y * MaxWaypointFromSpawnerRadius);
	}
	
	// Update is called once per frame
	void Update () 
	{
		// tell spawner if we hit it
		if (_spawner != null && Vector3.Distance(_spawner.transform.position, transform.position) < 2)
			_spawner.OnCellHitSpawner();
		
		if (_closestAttacker == null)
			_closestAttacker = Utils.FindClosestTransformWithTag(transform.position, FleeFromTag);
		
		Vector3 moveDir;
		float speed;
		
		// if an enemy is close.. FLEE
		if (_closeAttackers.Count > 0)
		{

			moveDir = GetFleeDir();
	
			speed = _startSpeed * 2;
		}
		else // other just flock
		{
			moveDir = _waypoint - transform.position;
			speed = _startSpeed;
		}
		
		if (_spawner != null && (Vector3.zero - transform.position).magnitude >= _spawner.SpawnRadius)
		{
			moveDir.Normalize();
			moveDir += (Vector3.zero - transform.position).normalized;
			
		}

		// update position and rotation
		Quaternion rotation = Quaternion.LookRotation(moveDir);
		transform.rotation =  Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _turnSpeed);	
		transform.position += transform.TransformDirection(Vector3.forward) * speed * Time.deltaTime;
		
	}
	
	
	public Vector3 GetFleeDir()
	{
		
		Vector3 fleeDir = transform.forward;
		
		float maxDist = 0;

		
		for (float angle=0; angle < 360; angle += 30)
		{
		
			Quaternion rotation = Quaternion.AngleAxis(angle,Vector3.up);
			
			Vector3 pos = transform.position + (rotation * transform.forward);
			float dist = GetClosestAttackerDistFromPos(pos);
			
			if (dist > maxDist)
			{
				fleeDir = pos - transform.position;	
				maxDist = dist;
			}
		}
		
		return fleeDir;
		
	}
	
	float GetClosestAttackerDistFromPos(Vector3 pos)
	{
		float minDist = float.MaxValue;
		
		foreach (Transform attacker in _closeAttackers)
		{
			float dist = Vector3.Distance(pos, attacker.position);
			
			if (dist < minDist)
			{
				minDist = dist;	
			}
		}
		
		return minDist;
	}
	
	#region trigger functions
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == FleeFromTag)
		{
			_closeAttackers.Add(other.transform);	
			
		}
		
	}
	
	void OnTriggerExit(Collider other)
	{
		
		if (_closeAttackers.Contains(other.transform))
		{
			_closeAttackers.Remove(other.transform);

		}
	}
	
	#endregion
		
}
