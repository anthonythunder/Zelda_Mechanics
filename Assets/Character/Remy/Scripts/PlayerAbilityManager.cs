using UnityEngine;

public class PlayerAbilityManager : MonoBehaviour
{
    private IPlayerController playerAction;
    [SerializeField] private Magnesis magnesis;

    [Header("UI")]
    public GameObject Magnesis_UI;
    enum AbilityState
    {
        None,
        Magnesis
    }
    AbilityState currentAbility = AbilityState.None;

    private void Start()
    {
        playerAction = GetComponent<IPlayerController>();
    }
    private void Update()
    {
        PlayerAbility();
    }
    public void PlayerAbility()
    {
        if(playerAction.isMagnesis)
        {
            magnesis.Activate(transform);
            HandleAbilityUI(Magnesis_UI);
        }
        else
        {
            CancelAbility();
        }
    }
    public void CancelAbility()
    {
        magnesis.CancelAbility(transform);
    }
    private void HandleAbilityUI(GameObject CurrentIU)
    {
        Magnesis_UI.SetActive(false);

        CurrentIU.SetActive(true);
    }
}