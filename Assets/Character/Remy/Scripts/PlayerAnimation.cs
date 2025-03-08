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
    }

    public void ThrowBombAnim()
    {
        _anim.SetTrigger("ThrowBomb");
    }
}
