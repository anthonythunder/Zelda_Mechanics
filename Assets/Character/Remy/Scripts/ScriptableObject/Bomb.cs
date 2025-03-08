using UnityEngine;

[CreateAssetMenu(fileName = "Bomb", menuName = "Scriptable Objects/Bomb")]
public class Bomb : AbilityPropertise
{
    private bool canDetonate = false;
    private bool hasThrown = false;
    private PlayerAnimation playerAnimation;
    private PlayerController playerCon;
    private PlayerAbilityManager playerAbilityManager;
    private GameObject ActiveBomb;
    public override void Activate(Transform player,GameObject Bomb)
    {
        base.Activate(player,Bomb);
        if(playerAnimation == null || playerCon == null)
        {
            playerCon = player.GetComponent<PlayerController>();
            playerAnimation = player.GetComponent<PlayerAnimation>();
            playerAbilityManager = player.GetComponent<PlayerAbilityManager>();
        }
        //instantiate Bomb
        if (ActiveBomb == null)
        {
            hasThrown = false;
            canDetonate = false;

            if (Input.GetKeyDown(KeyCode.G))
            {
                ActiveBomb = Instantiate(Bomb, playerCon.PalmBone_R.position, Quaternion.identity);
                ActiveBomb.transform.localScale = Vector3.one * 0.25f;
                ActiveBomb.transform.SetParent(playerCon.PalmBone_R);
                isActive = true;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && !hasThrown)
            {
                playerAnimation.ThrowBombAnim();
                canDetonate = true;
                hasThrown = true;
            }
            if(canDetonate && Input.GetKeyDown(KeyCode.G))
            {
                ActiveBomb.GetComponent<BombRune>().Detonate();
                canDetonate = false;
                hasThrown = false;
            }
        }
    }
    public override void CancelAbility(Transform player)
    {
        base.CancelAbility(player);
        if (!isActive) return;
        if (ActiveBomb != null)
        {
            Destroy(ActiveBomb);
        }
        isActive = false;
    }
    public void ThrowBomb()
    {
        ActiveBomb.transform.parent = null;
        ActiveBomb.GetComponent<Rigidbody>().isKinematic = false;
        ActiveBomb.transform.localScale = Vector3.one;
        ActiveBomb.GetComponent<Rigidbody>().AddForce(playerCon.transform.forward * 2, ForceMode.Force);

    }
}
