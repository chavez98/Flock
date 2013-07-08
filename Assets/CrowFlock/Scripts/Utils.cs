using UnityEngine;
using System.Collections;

public class Utils : MonoBehaviour {

	public static Transform FindClosestTransformWithTag(Vector3 fromPos, string tag)
	{
		GameObject[] allTargets = GameObject.FindGameObjectsWithTag(tag);
		GameObject newTarget = null;
		
		float minDist = float.MaxValue;

		foreach (GameObject target in allTargets)
		{
			float dist = Vector3.Distance(target.transform.position, fromPos);
			if (dist < minDist)
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
