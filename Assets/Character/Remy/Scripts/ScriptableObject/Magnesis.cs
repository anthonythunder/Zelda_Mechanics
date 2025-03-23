using Unity.Properties;
using UnityEngine;

[CreateAssetMenu(fileName = "Magnesis", menuName = "Scriptable Objects/Magnesis")]
public class Magnesis : AbilityPropertise
{
    private IPlayerController playerAction;
    private PlayerController playerController;
    private PlayerCameraMod _playerCameraMod;
    private PlayerAnimation _playerAnimation;
    [SerializeField] private LayerMask MagnesisLayer;
    private RaycastHit _hitGetObject;
    private Vector3 rayEnd;
     private float rayDistance = 20;

    private Transform CurrentSelectedObj;
    private Vector3 ActiveObjPos;
    private Vector3 offset;
    private int MinDistance = 1;
    private int MaxDistance = 12;
    private float DistanceOffsetSpped = 10;

    public GameObject _magnesisBulletPrefab;
    private GameObject _currentMagnesisBullet;

    public bool _shootMagnesisBullet = false;
    public bool _controlObject;
    public override void Activate(Transform player, GameObject Magnet)
    {
        
        base.Activate(player, Magnet);

        if (playerAction == null) {playerAction = player.GetComponent<IPlayerController>(); }
        if (playerController == null) { playerController = player.GetComponent<PlayerController>(); }
        if (_playerCameraMod == null) { _playerCameraMod = player.GetComponent<PlayerCameraMod>(); }
        if (_playerAnimation == null) { _playerAnimation = player.GetComponent<PlayerAnimation>(); }


        if (CurrentSelectedObj != null && _controlObject)
        {
            CurrentSelectedObj.position = (player.position + offset);
            ActiveObjPos = CurrentSelectedObj.position + Vector3.up * playerAction.MouseInput.y;
            CurrentSelectedObj.position = Vector3.Lerp(CurrentSelectedObj.position, ActiveObjPos,Time.deltaTime * 20);
            CurrentSelectedObj.transform.RotateAround(player.position, Vector3.up, playerAction.MouseInput.x * Time.deltaTime * 100);
            if (GetXZDistance(CurrentSelectedObj.position, player.position) > MinDistance && GetXZDistance(CurrentSelectedObj.position, player.position) < MaxDistance)
            {
                CurrentSelectedObj.position += playerController.transform.forward * playerAction.MagnesisDistanceInput * DistanceOffsetSpped * Time.deltaTime;
            }else if(GetXZDistance(CurrentSelectedObj.position, player.position) < MinDistance)
            {
                CurrentSelectedObj.position += playerController.transform.forward * Mathf.Abs(playerAction.MagnesisDistanceInput) * DistanceOffsetSpped * Time.deltaTime;
            }else if(GetXZDistance(CurrentSelectedObj.position, player.position) > MaxDistance)
            {
                CurrentSelectedObj.position -= playerController.transform.forward * Mathf.Abs(playerAction.MagnesisDistanceInput) * DistanceOffsetSpped * Time.deltaTime;
            }

            offset = CurrentSelectedObj.position - player.position;

            Vector3 playerFwdDir = -(player.position - CurrentSelectedObj.position).normalized;
            Vector3 dir = Quaternion.LookRotation(playerFwdDir).eulerAngles;
            player.eulerAngles = new Vector3(0, dir.y, 0);

            playerController.ReCenterCamera(true);
        }

        if (_shootMagnesisBullet)
        {

            if (CurrentSelectedObj != null && _currentMagnesisBullet != null)
            {
                _currentMagnesisBullet.transform.position = Vector3.MoveTowards(_currentMagnesisBullet.transform.position, CurrentSelectedObj.transform.position, Time.deltaTime * 15);
            }
            else if(_currentMagnesisBullet != null)
            {
                _currentMagnesisBullet.transform.position = Vector3.MoveTowards(_currentMagnesisBullet.transform.position, playerController.PlayerCenterToScreenPos, Time.deltaTime * 15);
            }
        }
        _playerAnimation.MagnesisBulletShoot(_shootMagnesisBullet);
        _playerAnimation.MagnesisLocomotion(_controlObject);

        if (isActive ) return;

        if (Input.GetMouseButtonDown(0))
        {
            //Shoot Magnesis bullet
            if (!_currentMagnesisBullet) 
            {
                _currentMagnesisBullet = Instantiate(_magnesisBulletPrefab, playerController.MagnesisProjectileStart, Quaternion.identity);
                _currentMagnesisBullet.GetComponent<Magnesisbullet>().magnesis = this;
            } 

            _shootMagnesisBullet = true;

            rayEnd = Camera.main.ScreenToViewportPoint(new Vector3(Screen.width * 0.5f, 0,Screen.height * 0.5f));
            if (Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward , out _hitGetObject, rayDistance, MagnesisLayer))
            {
                
                CurrentSelectedObj = _hitGetObject.transform;
                CurrentSelectedObj.GetComponent<Rigidbody>().isKinematic = true;
                offset = CurrentSelectedObj.position - player.position;
                playerController._locmotionState = PlayerController.LocamotionState.RuneActive;
                isActive = true;
            }
        }
        _playerCameraMod.SetCameraTrackimgTarget(_playerCameraMod._OTS_Target);
    }
    public override void CancelAbility( Transform player)
    {
        base.CancelAbility(player);
        if (playerController == null) { playerController = player.GetComponent<PlayerController>(); }
        playerController._locmotionState = PlayerController.LocamotionState.Normal;

        if (CurrentSelectedObj != null)
        {
            CurrentSelectedObj.GetComponent<Rigidbody>().isKinematic = false;
            CurrentSelectedObj = null;
        }
        playerController.ReCenterCamera(false);

        _playerCameraMod.SetCameraTrackimgTarget(_playerCameraMod._normalTarget);

        //Destory any existing Bullet
        if(_currentMagnesisBullet != null)
        {
            Destroy(_currentMagnesisBullet);
        }

        _playerAnimation.MagnesisBulletShoot(false);
        _playerAnimation.MagnesisLocomotion(false);
        _shootMagnesisBullet = false;
        _controlObject = false;
        isActive = false;
    }
    private float GetXZDistance(Vector3 a,Vector3 b)
    {
        float dist = Vector2.Distance(new Vector2 (a.x,a.z), new Vector2 (b.x,b.z));
        return dist;
    }

}
