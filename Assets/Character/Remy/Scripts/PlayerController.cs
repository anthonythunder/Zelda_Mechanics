using System;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, IPlayerController
{
    private CharacterController controller;
    private PlayerAbilityManager playerAbilityMng;
    public Animator PlayerAnim;

    public Transform GroundCheckTransform;
    private float GroundCheckRadius = 0.03f;
    public LayerMask GroundLayer;

    private Vector3 InputDir = new Vector3();
    private Vector3 playerVelocity = new Vector3();
    private bool isGrounded;
    private bool _isJumping;

    private bool IsMagnesis;
    private bool IsBomb;
    [Range(1, 10)]
    public float moveSpeed;

    [Header("UI")]
    public GameObject AbilityUI;

    PlayerInput playerInput;

    [Header("Skeleton Transform")]
    public Transform PalmBone_R;

    #region Interface
    public Vector3 InputVector => playerInput.InputDir;
    public Vector2 MouseInput => playerInput.MouseInput;
    public bool Jump => playerInput.Jump;
    public float MagnesisDistanceInput => playerInput.MagnesisDistanceInput;
    public bool isJumping => _isJumping;
    public bool magnesisActive => IsMagnesis;
    public bool BombActive => IsBomb;
    #endregion
    public enum AbilityState {
        None,
        Magnesis, 
        SquareBomb,
        SphereBomb,
        Stasis,
        Cyonis
    }
    public AbilityState Ability = AbilityState.None;
    public enum LocamotionState { Normal,zTarget,RuneActive}
    public LocamotionState _locmotionState;


    [Header("Cinemachine variables")]
    public CinemachineOrbitalFollow CineOF;
    private void Start()
    {
        EnableCursor(false);
        controller = GetComponent<CharacterController>();
        playerAbilityMng = GetComponent<PlayerAbilityManager>();
        SetAbility(0);
    }
    private void Update()
    {
        HandleInput();

        isGrounded = Physics.CheckSphere(GroundCheckTransform.position, GroundCheckRadius, GroundLayer);

        Locomotion();

        if (!isGrounded && !_isJumping)
        {
            Gravity();
        }
    }
    private Vector3 playerFwdDir;
    private void Locomotion()
    {
        switch (_locmotionState)
        {
            case LocamotionState.Normal:

                if (playerInput.InputDir.magnitude > 0.1f && !isJumping)
                {
                    float angle = Mathf.Atan2(playerInput.InputDir.x, playerInput.InputDir.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                    transform.rotation = Quaternion.Euler(0, angle, 0);
                    controller.Move(transform.forward * playerInput.InputDir.magnitude * moveSpeed * Time.deltaTime);
                }


               
                if (PlayerAnim.GetCurrentAnimatorStateInfo(0).IsName("Jump") && PlayerAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.96f && isGrounded)
                {
                    _isJumping = false;
                }

                break;
            case LocamotionState.zTarget:
                
                break;
            case LocamotionState.RuneActive:

                if (playerInput.InputDir.magnitude > 0.1f)
                {
                    controller.Move(transform.forward * playerInput.InputDir.z * moveSpeed * Time.deltaTime + transform.right * playerInput.InputDir.x * moveSpeed * Time.deltaTime);
                }

                break;
        }
    }
    private void HandleInput()
    {
        playerInput = new PlayerInput
        {
            InputDir = new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical")).normalized,
            MouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")),
            Jump = Input.GetButtonDown("Jump"),
            Tab = Input.GetKeyDown(KeyCode.Tab),
            MagnesisDistanceInput = Input.GetAxis("MagnesisDistance")

        };
        IsMagnesis = Ability.Equals(AbilityState.Magnesis);
        IsBomb = Ability.Equals(AbilityState.SquareBomb) || Ability.Equals(AbilityState.SphereBomb);
        if (Input.GetKeyDown(KeyCode.Tab) && Ability == AbilityState.None)
        {
            //EnableCursor(AbilityUI.activeSelf ? false : true);
            AbilityUI.SetActive(!AbilityUI.activeSelf);
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetAbility(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetAbility(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetAbility(3);
        }

        if (playerInput.Jump && !Ability.Equals(AbilityState.None))
        {
            CancelAbility((int)Ability);
            Ability = AbilityState.None;
        }
        else if(playerInput.Jump && Ability.Equals(AbilityState.None) && isGrounded)
        {
            _isJumping = true;
        }
    }
    private void Gravity()
    {
        playerVelocity.y += -9.85f * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    private void EnableCursor(bool value)
    {
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = value;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(GroundCheckTransform.position, GroundCheckRadius);

        Gizmos.color = Color.green;
        Vector3 CenterPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f));
        Gizmos.DrawRay(Camera.main.transform.position,Camera.main.transform.forward * 19);
    }
    private void CancelAbility(int index)
    {
        playerAbilityMng.CancelAbility(index);
    }
    public void SetAbility(int ability)
    {
        AbilityUI.SetActive(false);
        Ability = (AbilityState)ability;
    }

    public void ReCenterCamera(bool Value)
    {
        CineOF.HorizontalAxis.Reset();
        CineOF.VerticalAxis.Reset();
        CineOF.HorizontalAxis.Recentering.Enabled = Value;
        CineOF.VerticalAxis.Recentering.Enabled = Value;
    }
}
public struct PlayerInput
{
    public Vector3 InputDir;
    public Vector2 MouseInput;
    public bool Jump;
    public bool Tab;
    public float MagnesisDistanceInput;
}
public interface IPlayerController
{
    public Vector3 InputVector { get; }
    public Vector2 MouseInput { get; }
    public bool Jump { get; }
    public float MagnesisDistanceInput { get; }
    public bool isJumping { get; }
    public bool magnesisActive { get; }
    public bool BombActive { get; }
}
