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

	public float sens = 1f;

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
		CinemachineCameraTarget.transform.rotation = rot;
	}

	private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
	{
		if (lfAngle < -360f) lfAngle += 360f;
		if (lfAngle > 360f) lfAngle -= 360f;
		return Mathf.Clamp(lfAngle, lfMin, lfMax);
	}
}
