using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class Attack : MonoBehaviour {

	public string TargetTag;
	public float MinSpeed;
	public float MaxSpeed;
	public float MinNeighborDistance;
	
	Transform _target;
	float _speed;
	float _turnSpeed;
	
	List<Transform> _closeAttackers = new List<Transform>();
	
	void Start()
	{
		_speed = Random.Range(MinSpeed,MaxSpeed);	
		_turnSpeed = _speed * .1f;
		
	}
	
	public GameObject GetTarget()
	{
		if (_target == null)
			return null;
		else
			return _target.gameObject;	
	}
	
	void Update()
	{
		
		transform.position += transform.TransformDirection(Vector3.forward) * _speed * Time.deltaTime;
		
		_target = Utils.FindClosestTransformWithTag(transform.position,TargetTag);	
		
		if (_target != null)
		{

			Quaternion rotation = Quaternion.LookRotation(GetMoveDir(_target.position));
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _turnSpeed);	
			
			// did they catch up to target?
			if (Vector3.Distance(_target.position, transform.position) < 2)
			{
				// rotate a little so that the new attacker won't be bunched up with this one
				_target.RotateAround(Vector3.up,Random.Range(-45,45));
				Instantiate(gameObject,transform.position,_target.rotation);
				
				Destroy(_target.gameObject);
				_target = null;
			}
			
		}
	}
	
	Vector3 GetMoveDir(Vector3 targetPos)
	{
		Vector3 moveDir = (targetPos - transform.position).normalized;
		
		float minDist = float.MaxValue;
		Transform closestNeighbor = null;
		
		foreach (Transform neighbor in _closeAttackers)
		{
			float dist = Vector3.Distance(neighbor.transform.position, transform.position);
			
			if (dist < minDist && dist < MinNeighborDistance)
			{
				closestNeighbor = neighbor;
				minDist = dist;
			}
				
		}
		
		
		if (closestNeighbor != null)
		{
			moveDir += (transform.position - closestNeighbor.position).normalized;	
		}
		
		return moveDir;
		
	}
	
	
	#region trigger functions
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == gameObject.tag)
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
