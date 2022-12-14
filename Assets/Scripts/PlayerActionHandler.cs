using Cinemachine;
using System;
using System.Collections;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerActionHandler : MonoBehaviour
{
	[Header("Player")]
	public Transform RigtHand;
	public Transform LeftHand;
	public float MoveSpeed = 2.0f;
	public float SprintSpeed = 5.335f;
	public static bool isInvul;
	[Range(0.0f, 0.3f)]
	public float RotationSmoothTime = 0.12f;
	public float SpeedChangeRate = 10.0f;

	[Space(10)]
	public float JumpHeight = 1.2f;
	public float Gravity = -15.0f;

	[Space(10)]
	public float JumpTimeout = 0.50f;
	public float FallTimeout = 0.15f;

	[Header("Player Grounded")]
	public bool Grounded = true;
	public float GroundedOffset = -0.14f;
	public float GroundedRadius = 0.28f;
	public LayerMask GroundLayers;

	public bool staminaRegenDelay;

	[Header("Animator Overrides")]
	public AnimatorOverrideController axeOverride;
    public AnimatorOverrideController maceOverride;

	[Header("Weapons Materials")]
	public Material defaultMaterial;
	public Material buffedMaterial;
    // player
    private float _speed;
	private float _animationBlend;
	private float _targetRotation = 0.0f;
	private float _rotationVelocity;
	private float _verticalVelocity;
	private const float _terminalVelocity = 53.0f;

	// timeout deltatime
	private float _jumpTimeoutDelta;
	private float _fallTimeoutDelta;

	private Animator animator;
	private CharacterController controller;
	private PlayerInput input;
	private GameObject mainCamera;
	private PlayerStats playerStats;
	private AttackHitboxObject attackHitbox;
	private PlayerEquipment playerEquipment;
	private AudioManager audioManager;
	private PlayerUpgradeHandler playerUpgradeHandler;

	private int attackStaminaCost;

	public float attackMultiplier = 1;

	public bool weaponBuffActive = false;

	public GameObject altForm;
	public GameObject defaultForm;
	public Avatar altAvatar;
	public Avatar defaultAvatar;
	public AnimatorController defaultController;
	public AnimatorOverrideController altController;
	public AttackHitboxObject defaultAttackHitboxObject;
	public AttackHitboxObject altAttackHitboxObject;
	public Collider altaltAttackCollider;
	public GameObject hand1;
	public GameObject hand2;
	public bool transformed = false;

    private bool canCast = false;
	
    //private const float _threshold = 0.01f;

    //public bool combo = false;

    private Vector3 targetDirection;

	private LoadSave loadSave;
	private void Awake()
	{
		loadSave = FindObjectOfType<LoadSave>();
		// get a reference to our main camera
		if (mainCamera == null)
		{
			mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		}
	}
	private void Start()
	{
        Cursor.lockState = CursorLockMode.Locked;

        animator = gameObject.GetComponent<Animator>();
		controller = GetComponent<CharacterController>();
		input = GetComponent<PlayerInput>();
		playerStats = GetComponent<PlayerStats>();
		//attackHitbox = GetComponentInChildren<AttackHitboxObject>();
		playerEquipment = GetComponent<PlayerEquipment>();
		audioManager = GetComponentInChildren<AudioManager>();
		playerUpgradeHandler = GetComponent<PlayerUpgradeHandler>();
		playerEquipment.currentWeapon = GameManager.selectedStartingWeaponData;
		playerEquipment.currentRelic = GameManager.selectedStartingRelicData;

		SendMessage(playerEquipment.currentRelic.RelicFunction);

		GameObject[] playerWeapons = FindObjectOfType<WeaponEquipHandler>().UpdateWeaponItems();
		foreach (GameObject weapon in playerWeapons)
		{
			weapon.SetActive(false);
		}
		playerWeapons[GameManager.selectedStartingWeapon].SetActive(true);
        attackHitbox = playerWeapons[GameManager.selectedStartingWeapon].GetComponent<AttackHitboxObject>();

        // reset our timeouts on start
        _jumpTimeoutDelta = JumpTimeout;
		_fallTimeoutDelta = FallTimeout;

		animator.applyRootMotion = true;
		/*
		if (GameManager.loadFromSave)
		{
			transform.position = loadSave.LoadPlayerPos();
		}
		*/

		animator.runtimeAnimatorController = playerEquipment.currentWeapon.animations != null ? playerEquipment.currentWeapon.animations : animator.runtimeAnimatorController;


		//Debug.Log(Application.persistentDataPath);
		defaultAttackHitboxObject = attackHitbox;
		altForm.SetActive(false);
	}
	private void Update()
	{
		test();
        if (PlayerStats.isDead) return;
		if (!GameManager.spawnedEndRoom) return;

		animator.applyRootMotion = (animator.GetBool("isInteracting"));
		//loadSave.SaveGame(playerStats, this);
		JumpAndGravity();
		GroundedCheck();
		if (!animator.GetBool("isInteracting"))
		{
			RigtHand.GetComponent<MeleeWeaponTrail>().Emit = false;
			DisableCollider();
			Move();
			staminaRegenDelay = false;
		}
		Buffs();
        Attack();
		Roll();
		RegenHealth();
		Cast();
	}

	public bool damageAfterDodgeIsActive = false;
	public float damageAfterDodgeCountdown = 0;
	private void Buffs()
	{
		if (damageAfterDodgeIsActive)
		{
            damageAfterDodgeCountdown -= Time.deltaTime;
			if (damageAfterDodgeCountdown <= 0)
			{
				damageAfterDodgeCountdown = 0;
				damageAfterDodgeIsActive = false;
            }
		}
	}
	public void Transition()
	{
        if (defaultForm.activeSelf == true)
        {
            attackHitbox = altAttackHitboxObject;
            defaultForm.SetActive(false);
            animator.avatar = altAvatar;
            animator.runtimeAnimatorController = altController;
            altForm.SetActive(true);
            hand1.SetActive(true);
            hand2.SetActive(true);
			playerStats.Heal(playerStats.maxHealth - (int) playerStats.currentHealth);
            transformed = true;
        }
        else
        {
            attackHitbox = defaultAttackHitboxObject;
            defaultForm.SetActive(true);
            animator.avatar = defaultAvatar;
            animator.runtimeAnimatorController = defaultController;
            altForm.SetActive(false);
            hand1.SetActive(false);
            hand2.SetActive(false);
            playerStats.Heal(playerStats.maxHealth - (int)playerStats.currentHealth);
            transformed = false;
        }
    }
	private void test()
	{
		//print("action handler running");
		if (Input.GetKeyDown(KeyCode.E))
		{
		
			//print("Input working");
			
            Scene scene = SceneManager.GetActiveScene(); 
			SceneManager.LoadScene(scene.name);
        }

    }

	
	private void Cast()
	{
        if (Input.GetKeyDown(KeyCode.Q) && playerStats.currentSoul > 1 && canCast)
		{
			weaponBuffActive = !weaponBuffActive;
			animator.SetBool("Cast", true);
			animator.SetBool("isInteracting", true);
        }
		else
		{
            animator.SetBool("Cast", false);
        }
		if (weaponBuffActive && playerStats.currentSoul > 1)
		{
			playerStats.currentSoul -= (3 * playerUpgradeHandler.spellCostDownPercent) * Time.deltaTime;
            if (gameObject.GetComponentInChildren<AttackHitboxObject>().gameObject.GetComponent<MeshRenderer>() != null)
                gameObject.GetComponentInChildren<AttackHitboxObject>().gameObject.GetComponent<MeshRenderer>().material = buffedMaterial;
			attackMultiplier = 2f + playerUpgradeHandler.spellDamageMuliplier;
		}
		else
		{
			weaponBuffActive = false;
			if (gameObject.GetComponentInChildren<AttackHitboxObject>().gameObject.GetComponent<MeshRenderer>() != null)
				gameObject.GetComponentInChildren<AttackHitboxObject>().gameObject.GetComponent<MeshRenderer>().material = defaultMaterial;
            attackMultiplier = 1f;
        }

	}

	private void GroundedCheck()
	{
		// set sphere position, with offset
		Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
		Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

		// update animator if using character
		animator.SetBool("Grounded", Grounded);
		
	}
	private void Move()
	{
		if (input.sprint && playerStats.currentStamina > 0)
        {
			//playerStats.currentStamina -= 0.025f * (1 + Time.deltaTime);
			playerStats.RemoveStamina(0.025f * (1 + Time.deltaTime));
			staminaRegenDelay = true;
			playerStats.regenTimer = 0;
        }
		else if (playerStats.currentStamina <= 0)
        {
			input.sprint = false;

		}
		// set target speed based on move speed, sprint speed and if sprint is pressed
		float targetSpeed = input.sprint ? SprintSpeed : MoveSpeed;
		// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

		// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
		// if there is no input, set the target speed to 0
		if (input.move == Vector2.zero) targetSpeed = 0.0f;

		// a reference to the players current horizontal velocity
		float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

		float speedOffset = 0.1f;
		float inputMagnitude = input.analogMovement ? input.move.magnitude : 1f;

		// accelerate or decelerate to target speed
		if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
		{
			// creates curved result rather than a linear one giving a more organic speed change
			// note T in Lerp is clamped, so we don't need to clamp our speed
			_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

			// round speed to 3 decimal places
			_speed = Mathf.Round(_speed * 1000f) / 1000f;
		}
		else
		{
			_speed = targetSpeed;
		}
		_animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);

		// normalise input direction
		Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;

		// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
		// if there is a move input rotate player when the player is moving
		/*if (lockedOn)
		{
			_targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + selectedEnemy.transform.parent.transform.parent.transform.rotation.y;
		}
		else */
		if (input.move != Vector2.zero)
		{
			_targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
		}

		float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);
		// rotate to face input direction relative to camera position
		transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

		targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

		// move the player
		controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
		// update animator if using character
		
		animator.SetFloat("Speed", _animationBlend);
		animator.SetFloat("MotionSpeed", inputMagnitude);

		switch (Mathf.RoundToInt(input.move.x), false)
		{
			case (1, true):
				animator.SetFloat("WalkDirection", 0);
				break;
			case (-1, true):
				animator.SetFloat("WalkDirection", 1);
				break;
			default:
				animator.SetFloat("WalkDirection", -1);
				break;
		}

	}
	private void JumpAndGravity()
	{
		if (Grounded)
		{
			// reset the fall timeout timer
			_fallTimeoutDelta = FallTimeout;

			// update animator if using character
			
			animator.SetBool("Jump", false);
			animator.SetBool("FreeFall", false);
			

			// stop our velocity dropping infinitely when grounded
			if (_verticalVelocity < 0.0f)
			{
				_verticalVelocity = -2f;
			}

			// Jump
			if (input.jump && _jumpTimeoutDelta <= 0.0f)
			{
				// the square root of H * -2 * G = how much velocity needed to reach desired height
				_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

				// update animator if using character
				
				animator.SetBool("Jump", true);
				
			}

			// jump timeout
			if (_jumpTimeoutDelta >= 0.0f)
			{
				_jumpTimeoutDelta -= Time.deltaTime;
			}
		}
		else
		{
			// reset the jump timeout timer
			_jumpTimeoutDelta = JumpTimeout;

			// fall timeout
			if (_fallTimeoutDelta >= 0.0f)
			{
				_fallTimeoutDelta -= Time.deltaTime;
			}
			else
			{
				// update animator if using character
				animator.SetBool("FreeFall", true);
				
			}

			// if we are not grounded, do not jump
			input.jump = false;
		}

		// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
		if (_verticalVelocity < _terminalVelocity)
		{
			_verticalVelocity += Gravity * Time.deltaTime;
		}
	}
	private void Roll()
	{
		if (animator.GetBool("isInteracting"))
			return;

		if (Input.GetKeyDown(KeyCode.Space) && playerStats.currentStamina > 0)
		{
			audioManager.RollSound();
			//playerStats.currentStamina -= 10;
			playerStats.RemoveStamina(10);
			playerStats.regenTimer = 0;
			staminaRegenDelay = true;
			isInvul = true;
			if (input.move.x == 0 && input.move.y == 0)
			{
				animator.SetFloat("WalkDirection", -1);
				animator.SetBool("Roll", true);
				animator.SetBool("isInteracting", true);
			}
			else
			{
				targetDirection.y = 0;

				Quaternion rollRotation = Quaternion.LookRotation(targetDirection);
				transform.rotation = rollRotation;

				animator.SetBool("Roll", true);
				animator.SetBool("isInteracting", true);

				
			}
			
		}
		input.roll = false;

	}
	private void Attack()
	{ 
        if (Input.GetKeyDown(KeyCode.Mouse0) && animator.GetBool("AttackR1"))
        {
            animator.SetBool("Combo", true);
            attackStaminaCost = playerEquipment.currentWeapon.R1StaminaCost;
			attackStaminaCost *= (int) playerUpgradeHandler.extraStaminaToDamageMultiplier;
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && playerStats.currentStamina > 0)
        {
            attackStaminaCost = playerEquipment.currentWeapon.R1StaminaCost;
            attackStaminaCost *= (int)playerUpgradeHandler.extraStaminaToDamageMultiplier;
            animator.SetBool("AttackR1", true);
        } 
        else if (Input.GetKeyDown(KeyCode.Mouse1) && playerStats.currentStamina > 0)
		{
            attackStaminaCost = playerEquipment.currentWeapon.R1StaminaCost * 2;
            attackStaminaCost *= (int)playerUpgradeHandler.extraStaminaToDamageMultiplier;
            animator.SetBool("AttackR2", input.attack2);
        }
    }
	private void RegenHealth()
    {
		if (Input.GetKey(KeyCode.R) && playerStats.currentSoul > 0)
        {
			FindObjectOfType<HealthBarShrink>().Heal(playerStats.currentHealth, playerStats.maxHealth);
			playerStats.currentHealth += 1;
			playerStats.currentSoul -= 1;
			animator.SetBool("Heal", true);
            Invoke("resetHealInput", 2);
        }
		else
        {
            animator.SetBool("Heal", false);
        }
    }
	public void DisableAttack()
	{
		RigtHand.GetComponent<MeleeWeaponTrail>().Emit = false;
		animator.SetBool("AttackR1", false);
		animator.SetBool("AttackR2", false);
		staminaRegenDelay = false;
		DisableCollider();
	}
	public void EnableCollider()
	{
		attackHitbox.dmgCollider.enabled = true;
		altaltAttackCollider.enabled = true;
    }
	public void DisableCollider()
	{
		attackHitbox.dmgCollider.enabled = false;
        altaltAttackCollider.enabled = false;
    }
	public void BeginAttack()
    {
        _speed = 0;
		playerStats.RemoveStamina(attackStaminaCost);
        //playerStats.currentStamina -= attackStaminaCost;
        animator.SetBool("isInteracting", true);
        RigtHand.GetComponent<MeleeWeaponTrail>().Emit = true;
        playerStats.regenTimer = 0;
        audioManager.AttackSound();
    }

    public void resetHealInput()
	{
        input.heal = false;
    }

	public void SoulfireWeapon()
	{
		canCast = true;
	}

    
}
