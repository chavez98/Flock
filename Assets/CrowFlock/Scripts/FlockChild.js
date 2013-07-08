/**************************************									
	Copyright 2012 Unluck Software	
 	www.chemicalbliss.com													
***************************************/
#pragma strict
#pragma downcast
public var _spawner:FlockController;
private var _wayPoint : Vector3;
private var _speed= 10;
private var _dived:boolean =true;
private var _stuckCounter:float;	//prevents looping around a waypoint
private var _damping:float;

function Start(){
   Wander(0);
   var sc = Random.Range(_spawner._minScale, _spawner._maxScale);
   transform.localScale=Vector3(sc,sc,sc);
   transform.position = (Random.insideUnitSphere *_spawner._spawnSphere) + _spawner.transform.position;
   transform.position.y = Random.Range(-_spawner._spawnSphereHeight, _spawner._spawnSphereHeight*1.0) +_spawner.transform.position.y;

   
}

function Update() {
   transform.position += transform.TransformDirection(Vector3.forward)*_speed*Time.deltaTime;
    if((transform.position - _wayPoint).magnitude < _spawner._waypointDistance+_stuckCounter){
        Wander(0);	//create a new waypoint
        _stuckCounter=0;
    }else{
    	_stuckCounter+=Time.deltaTime;
    }
    var rotation = Quaternion.LookRotation(_wayPoint - transform.position);
	transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _damping);

	if(_spawner._childTriggerPos){
		if((transform.position - _spawner.transform.position).magnitude < 1){
			_spawner.randomPosition();
		}
	}
}

function Wander(delay:float){
	yield(WaitForSeconds(delay));
	_damping = Random.Range(_spawner._minDamping, _spawner._maxDamping);       
    _speed = Random.Range(_spawner._minSpeed, _spawner._maxSpeed);
    
    if(!_dived && _spawner._diveFrequency > 0 && Random.value < _spawner._diveFrequency){

   	 	_wayPoint.x= transform.position.x + Random.Range(-1, 1);
    	_wayPoint.z=	transform.position.z + Random.Range(-1, 1);

    	_dived = true;
		
	}else{

		_wayPoint= (Random.insideUnitSphere *_spawner._spawnSphere) + _spawner.transform.position;
		_dived = false;
	}
}