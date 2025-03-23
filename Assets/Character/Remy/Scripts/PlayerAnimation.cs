using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator _anim;

    public IPlayerController _playerAction;
    void Start()
    {
        _playerAction = GetComponent<IPlayerController>();
    }

    private void Update()
    {
        _anim.SetFloat("UnitVelocity", _playerAction.InputVector.magnitude);
        _anim.SetBool("Jump", _playerAction.isJumping);
        _anim.SetBool("Magnesis", _playerAction.magnesisActive);
        _anim.SetBool("Bomb", _playerAction.BombActive);
        _anim.SetFloat("xValue", _playerAction.InputVector.x);
        _anim.SetFloat("zValue", _playerAction.InputVector.z);
    }

    public void ThrowBombAnim()
    {
        _anim.SetTrigger("ThrowBomb");
    }
    public void MagnesisBulletShoot(bool value)
    {
        _anim.SetBool("magnesisShoot", value);
    }
    public void MagnesisLocomotion(bool value)
    {
        _anim.SetBool("MagnesisLoco", value);
    }
}
