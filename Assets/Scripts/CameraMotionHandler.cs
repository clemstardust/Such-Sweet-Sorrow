using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

public class CameraMotionHandler : MonoBehaviour
{

	[Header("Cinemachine")]
	public CinemachineVirtualCamera PlayerFollowCamera;
	public GameObject CinemachineCameraTarget;
	public GameObject CinemachineCameraLock;
	public float maxLockOnRange;
	public float TopClamp = 70.0f;
	public float BottomClamp = -30.0f;
	public float CameraAngleOverride = 0.0f;
	public bool LockCameraPosition = false;

	// cinemachine
	private float _cinemachineTargetYaw;
	private float _cinemachineTargetPitch;

	private PlayerInput input;
	private GameObject mainCamera;

	public float sens = 0.5f;

	public bool lockedOn = false;
	private EnemyAI closest = null;

	int layerMask = 1 << 8;

	private void Awake()
	{
		// get a reference to our main camera
		if (mainCamera == null)
		{
			mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		}
	}
	private void Start()
	{
		input = GetComponent<PlayerInput>();
		closest = FindNearestEnemy();
	}

	public EnemyAI FindNearestEnemy()
    {
		EnemyAI tMin = null;
		var enemies = FindObjectsOfType<EnemyAI>();
		float minDist = Mathf.Infinity;
		Vector3 currentPos = transform.position;
		foreach (EnemyAI t in enemies)
		{
			float dist = Vector3.Distance(t.gameObject.transform.position, currentPos);
			Vector3 dir = t.gameObject.transform.position - transform.position;
			dir.y = 0;
			if (dist < minDist && Physics.Raycast(transform.position, dir, out RaycastHit hit, Mathf.Infinity, layerMask))
			{
				Debug.DrawRay(transform.position, dir * hit.distance, Color.green);
				tMin = t;
				minDist = dist;
			}
		}
		return tMin;
	}

	// Update is called once per frame
	void FixedUpdate()
    {
		CameraRotation();
	}

	private void CameraRotation()
	{
		
		_cinemachineTargetYaw += input.look.x * Time.fixedDeltaTime * sens;
		_cinemachineTargetPitch += input.look.y * Time.fixedDeltaTime * sens;
		

		// clamp our rotations so our values are limited 360 degrees
		_cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
		_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

		Quaternion rot = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
		// Cinemachine will follow this target
		
		if (Input.GetKeyDown(KeyCode.R))
        {
			lockedOn = !lockedOn;
			if (closest)
				closest.lockIndicator.indicator.enabled = false;
			closest = FindNearestEnemy();
			if (closest)
				closest.lockIndicator.indicator.enabled = true;
		}
		if (lockedOn && closest)
        {
			closest.lockIndicator.indicator.enabled = true;
			CinemachineCameraTarget.transform.LookAt(closest.transform);
		}
		else
        {
			if (closest)
				closest.lockIndicator.indicator.enabled = false;
			CinemachineCameraTarget.transform.rotation = rot;
		}

	}

	private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
	{
		if (lfAngle < -360f) lfAngle += 360f;
		if (lfAngle > 360f) lfAngle -= 360f;
		return Mathf.Clamp(lfAngle, lfMin, lfMax);
	}
}
