using UnityEngine;

public class PlayerAbilityManager : MonoBehaviour
{
    private IPlayerController playerAction;
    private PlayerController playerCon;
    private PlayerAnimation _playerAnim;
    [SerializeField] private Magnesis magnesis;
    [SerializeField] private Bomb Squarebomb;

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
        Squarebomb.ThrowBomb();
    }
}