using UnityEngine;

public class PlayerAbilityManager : MonoBehaviour
{
    private IPlayerController playerAction;
    private PlayerController playerCon;
    private PlayerAnimation _playerAnim;
    [SerializeField] private Magnesis magnesis;
    [SerializeField] private Bomb Squarebomb;

    public Rigidbody HoldingBomb;
    [Header("UI")]
    public GameObject Magnesis_UI;

    private void Start()
    {
        playerAction = GetComponent<IPlayerController>();
        playerCon = GetComponent<PlayerController>();
        _playerAnim = GetComponent<PlayerAnimation>();
    }
    private void Update()
    {
        PlayerAbility();
    }
    public void PlayerAbility()
    {
        switch (playerCon.Ability)
        {
            case PlayerController.AbilityState.None:
                CancelAbility();
                break;
            case PlayerController.AbilityState.Magnesis:
                magnesis.Activate(transform);
                HandleAbilityUI(Magnesis_UI);
                break;
            case PlayerController.AbilityState.SquareBomb:
                Squarebomb.Activate(transform);

                if (Squarebomb.canDetonate && Input.GetKeyDown(KeyCode.G))
                {
                    HoldingBomb.GetComponent<BombRune>().Detonate();
                }
                break;
            case PlayerController.AbilityState.SphereBomb:
                break;
            case PlayerController.AbilityState.Stasis:
                break;
            case PlayerController.AbilityState.Cyonis:
                break;
        }
    }
    public void CancelAbility()
    {
        magnesis.CancelAbility(transform);
        Squarebomb.CancelAbility(transform);
    }
    private void HandleAbilityUI(GameObject CurrentIU)
    {
        Magnesis_UI.SetActive(false);

        CurrentIU.SetActive(true);
    }
    public void ThrowBombObj()
    {
        if(HoldingBomb!= null)
        {
            HoldingBomb.transform.parent = null;
            HoldingBomb.isKinematic = false;
            HoldingBomb.transform.localScale = Vector3.one;
            HoldingBomb.AddForce(transform.forward * 2,ForceMode.Force);
        }
    }
}
public interface IExplosion
{
    public Vector3 ExplosionOrigin { get; set; }
    public void Explode();
}