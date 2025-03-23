using UnityEngine;


[CreateAssetMenu(fileName = "Stasis", menuName = "Scriptable Objects/Stasis")]
public class Stasis : AbilityPropertise
{
    private PlayerController playerController;
    private PlayerCameraMod _playerCameraMod;
    private Rigidbody StasisObject;

    private bool isStasisActive = false;
    private float StasisTime = 5;

    float _time;

    private Vector3 rayEnd;
    private RaycastHit _hitGetObject;
    private float rayDistance = 20;
    public LayerMask StasisLayer;

    public override void Activate(Transform player, GameObject statis)
    {
        base.Activate(player, statis);

        if(playerController == null){ playerController = player.GetComponent<PlayerController>();}
        if(_playerCameraMod == null){ _playerCameraMod = player.GetComponent<PlayerCameraMod>(); }

        if (Input.GetMouseButtonDown(0))
        {
            rayEnd = Camera.main.ScreenToViewportPoint(new Vector3(Screen.width * 0.5f, 0, Screen.height * 0.5f));
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out _hitGetObject, rayDistance, StasisLayer))
            {
                StasisObject = _hitGetObject.transform.GetComponent<Rigidbody>();
                isStasisActive = true;
                isActive = true;
                Debug.Log("Stasis : " + StasisObject.name);
            }

        }
        if (isStasisActive)
        {
            StasisObject.isKinematic = true;
            StasisObject.freezeRotation = true;
        }
        if(StasisObject != null)
        {
            _time += Time.deltaTime;
            if (_time >= StasisTime)
            {
                StasisObject.isKinematic = false;
                StasisObject.freezeRotation = false;
                isStasisActive = false;
                _time = 0;
                StasisObject = null;
            }
        }
        _playerCameraMod.SetCameraTrackimgTarget(_playerCameraMod._OTS_Target);
    }
    public override void CancelAbility(Transform player)
    {
        base.CancelAbility(player);

        _playerCameraMod.SetCameraTrackimgTarget(_playerCameraMod._OTS_Target);
        isStasisActive = false;
    }
}