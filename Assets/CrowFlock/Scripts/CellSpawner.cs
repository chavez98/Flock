using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CellSpawner : MonoBehaviour {
	
	public GameObject CellPrefab;
	public int NumCells;
	public float SpawnRadius;
	
	List<GameObject> _cells = new List<GameObject>();
		
	
	// Use this for initialization
	void Start () {
		
		OnCellHitSpawner();
		
		for (int i=0; i < NumCells; i++)
		{
			GameObject cell = (GameObject)Instantiate(CellPrefab, new Vector3(Random.insideUnitCircle.x * SpawnRadius, 
			0, Random.insideUnitCircle.y * SpawnRadius),Quaternion.identity);
			cell.SendMessage("SetSpawner",this,SendMessageOptions.DontRequireReceiver);
			_cells.Add(cell);
		}
		
		OnCellHitSpawner();
	}
	
	Vector3 GetRandomPosition()
	{
		return new Vector3(Random.insideUnitSphere.x * SpawnRadius, 
			0, Random.insideUnitSphere.y * SpawnRadius);
		
	}
	
	
	public void OnCellHitSpawner()
	{
		
		transform.position = GetRandomPosition();	
		
		for  (int i = _cells.Count-1; i >= 0; i--)
		{
			GameObject cell = _cells[i];
			
			if (cell == null)
				_cells.RemoveAt(i);
			else
				cell.SendMessage("SetNewWaypoint",transform.position,SendMessageOptions.DontRequireReceiver);
				
			
		}
	}

}
