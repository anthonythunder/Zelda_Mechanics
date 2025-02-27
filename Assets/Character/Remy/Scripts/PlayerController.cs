using System;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, IPlayerController
{
    private CharacterController controller;
    public Animator PlayerAnim;

    public Transform GroundCheckTransform;
    private float GroundCheckRadius = 0.03f;
    public LayerMask GroundLayer;

    private Vector3 InputDir = new Vector3();
    private Vector3 playerVelocity = new Vector3();
    private bool isGrounded;
    private bool _isJumping;
    [Range(1, 10)]
    public float moveSpeed;

    [Header("UI")]
    public GameObject AbilityUI;

    PlayerInput playerInput;

    #region Interface
    public Vector3 InputVector => playerInput.InputDir;
    public Vector2 MouseInput => playerInput.MouseInput;
    public bool Jump => playerInput.Jump;
    public float MagnesisDistanceInput => playerInput.MagnesisDistanceInput;
    public bool isJumping => _isJumping;
    public bool isMagnesis => playerInput.isMagnesis;
    #endregion
    enum AbilityState {None, Magnesis}
    AbilityState Ability = AbilityState.None;
    public enum LocamotionState { Normal,zTarget,RuneActive}
    public LocamotionState _locmotionState;


    [Header("Cinemachine variables")]
    public CinemachineOrbitalFollow CineOF;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
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


                if (playerInput.Jump && isGrounded)
                {
                    _isJumping = true;
                }
                if (PlayerAnim.GetCurrentAnimatorStateInfo(0).IsName("Jump") && PlayerAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.96f && isGrounded)
                {
                    _isJumping = false;
                }

                break;
            case LocamotionState.zTarget:
                
                break;
            case LocamotionState.RuneActive:

                //playerFwdDir = -(Camera.main.transform.position - transform.position).normalized;
                //Vector3 dir = Quaternion.LookRotation(playerFwdDir).eulerAngles;
                //transform.eulerAngles = new Vector3(0, dir.y, 0);
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
            isMagnesis = Ability.Equals(AbilityState.Magnesis),
            Tab = Input.GetKeyDown(KeyCode.Tab),
            MagnesisDistanceInput = Input.GetAxis("MagnesisDistance")
        };
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //EnableCursor(AbilityUI.activeSelf ? false : true);
            AbilityUI.SetActive(!AbilityUI.activeSelf);
        }

        if (Jump && !Ability.Equals(AbilityState.None))
        {
            SetAbility(0);
        }
    }
    private void Gravity()
    {
        playerVelocity.y += -9.85f * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    //private void EnableCursor(bool value)
    //{
    //    Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
    //    Cursor.visible = value;
    //}
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(GroundCheckTransform.position, GroundCheckRadius);

        Gizmos.color = Color.green;
        Vector3 CenterPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f));
        Gizmos.DrawRay(Camera.main.transform.position,Camera.main.transform.forward * 19);
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
    public bool isMagnesis;
}
public interface IPlayerController
{
    public Vector3 InputVector { get; }
    public Vector2 MouseInput { get; }
    public bool Jump { get; }
    public float MagnesisDistanceInput { get; }
    public bool isJumping { get; }
    public bool isMagnesis { get; }
    
}
