using UnityEngine;

[CreateAssetMenu(fileName = "Bomb", menuName = "Scriptable Objects/Bomb")]
public class Bomb : AbilityPropertise
{
    public bool canDetonate;
    private PlayerAnimation playerAnimation;
    private PlayerController playerCon;
    private PlayerAbilityManager playerAbilityManager;
    private GameObject ActiveBomb;
    public override void Activate(Transform player)
    {
        base.Activate(player);
        if(playerAnimation == null || playerCon == null)
        {
            playerCon = player.GetComponent<PlayerController>();
            playerAnimation = player.GetComponent<PlayerAnimation>();
            playerAbilityManager = player.GetComponent<PlayerAbilityManager>();
        }
        //instantiate Bomb

        if(ActiveBomb == null)
        {
            ActiveBomb = Instantiate(playerCon.SquareBomb, playerCon.PalmBone_R.position, Quaternion.identity);
            ActiveBomb.transform.localScale = Vector3.one * 0.25f;
            ActiveBomb.transform.SetParent(playerCon.PalmBone_R);
            canDetonate = true;
        }
        else
        {
            if (playerAbilityManager.HoldingBomb != ActiveBomb.GetComponent<Rigidbody>())
            {
                playerAbilityManager.HoldingBomb = ActiveBomb.GetComponent<Rigidbody>();
            }
            if (Input.GetMouseButtonDown(0))
            {
                playerAnimation.ThrowBombAnim();
            }
        }

    }
    public override void CancelAbility(Transform player)
    {
        base.CancelAbility(player);
        if (!canDetonate) return;
        if (ActiveBomb != null)
        {
            //Destroy(ActiveBomb);
        }
    }
}
