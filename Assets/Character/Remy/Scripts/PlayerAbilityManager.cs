using UnityEngine;

public class PlayerAbilityManager : MonoBehaviour
{
    private IPlayerController playerAction;
    private PlayerController playerCon;
    private PlayerAnimation _playerAnim;
    [SerializeField] private Magnesis magnesis;
    [SerializeField] private Bomb Squarebomb;
    [SerializeField] private Bomb Spherebomb;

    [Header("Prefabs")]
    [SerializeField] private GameObject SquareBomb_prefab;
    [SerializeField] private GameObject SphereBomb_prefab;

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

                break;
            case PlayerController.AbilityState.Magnesis:
                magnesis.Activate(transform,null);
                HandleAbilityUI(Magnesis_UI);
                break;
            case PlayerController.AbilityState.SquareBomb:
                Squarebomb.Activate(transform,SquareBomb_prefab);

                break;
            case PlayerController.AbilityState.SphereBomb:
                Spherebomb.Activate(transform, SphereBomb_prefab);
                break;
            case PlayerController.AbilityState.Stasis:
                break;
            case PlayerController.AbilityState.Cyonis:
                break;
        }
    }
    public void CancelAbility(int index)
    {
        switch (index)
        {
            case 1:
                magnesis.CancelAbility(transform);
                break;
            case 2:
                Squarebomb.CancelAbility(transform);
                break;
            case 3:
                Spherebomb.CancelAbility(transform);
                break;
        }
    }
    public void CancelAllAbility()
    {
        magnesis.CancelAbility(transform);
        Squarebomb.CancelAbility(transform);
        Spherebomb.CancelAbility(transform);
    }
    private void HandleAbilityUI(GameObject CurrentIU)
    {
        Magnesis_UI.SetActive(false);

        CurrentIU.SetActive(true);
    }
    public void ThrowBombObj()
    {
        if(playerCon.Ability.Equals(PlayerController.AbilityState.SquareBomb))
            Squarebomb.ThrowBomb();
        else if (playerCon.Ability.Equals(PlayerController.AbilityState.SphereBomb))
            Spherebomb.ThrowBomb();

    }
}