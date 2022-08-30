using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
	private void Awake()
	{
		FirstObjectNotifier.OnFirstObjectSpawned += FirstObjectNotifier_OnFirstObjectSpawned;
	}
	
	private void OnDestroy()
	{
		FirstObjectNotifier.OnFirstObjectSpawned -= FirstObjectNotifier_OnFirstObjectSpawned;
	}
	
	private void FirstObjectNotifier_OnFirstObjectSpawned(Transform obj)
	{
		//CinemachineVirtualCamera vc = GetComponent<CinemachineVirtualCamera>();
		GameObject.Find("CM vcam3").GetComponent<CinemachineVirtualCamera>().Follow = obj.Find("ArcherSkill").transform;
		GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().Follow = obj;
		//vc.Follow = obj;
	}
}
