using UnityEngine;
using System.Collections;

public class ControllerCamera : MonoBehaviour
{

	public GameObject player;

	int DistanceAway = 17;

	void Start ()
	{
		
	}

	void LateUpdate ()
	{
		
		Vector3 PlayerPOS = player.transform.position;
		GameObject.Find ("Main Camera").transform.position = new Vector3 (PlayerPOS.x, PlayerPOS.y, PlayerPOS.z - DistanceAway);
	}
}
