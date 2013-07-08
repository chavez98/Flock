using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
	
	public string InputReceiverTag;
	
	Transform _selection;
	
	void Update()
	{

		if (Input.GetMouseButton(0))
		{
			Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 
				Input.mousePosition.y,150));
			
			if (_selection == null)
				_selection = FindClosestTransformWithTag(pos,InputReceiverTag);

			_selection.SendMessage("OnInput",pos);
			
		}
		else if (Input.GetMouseButtonUp(0))
		{
			_selection = null;	
		}
		
		
		
	}
	
		
	// HACK: integrate into utils function
	public Transform FindClosestTransformWithTag(Vector3 fromPos, string tag)
	{
		GameObject[] allTargets = GameObject.FindGameObjectsWithTag(tag);
		GameObject newTarget = null;
		
		float minDist = float.MaxValue;

		foreach (GameObject target in allTargets)
		{
			float dist = Vector3.Distance(target.transform.position, fromPos);
			if (dist < minDist && target.GetComponent<Attack>().enabled)
			{
				newTarget = target;
				minDist = dist;
			}
			
		}
		
		if (newTarget == null)
			return null;
		else
			return newTarget.transform;
	}
		
}
