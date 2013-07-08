using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointMovement : MonoBehaviour {
	
	public float Speed = 200;
	public float TurnSpeed = 6;
	public float MinDistBetweenWaypoints = 3;
	
	List<Vector3> waypoints = new List<Vector3>();
	
	void OnInput(Vector3 pos)
	{
		if (waypoints.Count == 0 || Vector3.Distance(waypoints[waypoints.Count-1],pos) > MinDistBetweenWaypoints)
		{
			waypoints.Add(pos);	
			GetComponent<Attack>().enabled = false;
		}
	}
	
	void OnDrawGizmos() 
	{
		if (waypoints.Count > 0)
			Gizmos.DrawLine(transform.position,waypoints[0]);
		
		for (int i=0; i < waypoints.Count-1; i++)
		{
			Gizmos.DrawLine(waypoints[i],waypoints[i+1]);
		}
		
	}
	
	
	void Update()
	{

		if (waypoints.Count == 0)
		{
			GetComponent<Attack>().enabled = true;
			return;
		}
		
		Vector3 dir = (waypoints[0] - transform.position).normalized;
		transform.position += dir * Speed * Time.deltaTime;
		
		Quaternion rotation = Quaternion.LookRotation(dir);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * TurnSpeed);	
			

		if (Vector3.Distance(waypoints[0],transform.position) < 2)
			waypoints.RemoveAt(0);
		
		// OPTIMIZE: hack remove later
		Transform closestRedCell = Utils.FindClosestTransformWithTag(transform.position,"RedCell");
		
			// did they catch up to target?
			if (Vector3.Distance(closestRedCell.position, transform.position) < 2)
			{
				Instantiate(gameObject,transform.position,Quaternion.identity);
				
				Destroy(closestRedCell.gameObject);
			}
	}
	

	
}
