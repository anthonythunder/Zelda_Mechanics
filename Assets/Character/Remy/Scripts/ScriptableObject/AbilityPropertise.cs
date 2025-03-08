using UnityEngine;

[CreateAssetMenu(fileName = "AbilityPropertise", menuName = "Scriptable Objects/AbilityPropertise")]
public class AbilityPropertise : ScriptableObject
{
    [Header("Propertise")]
    public string Name;
    public string Description;
    public bool isActive = false;
    public virtual void Activate(Transform player,GameObject Obj)
    {
        Debug.Log("Activate : " + Name);
    }
    public virtual void CancelAbility(Transform player)
    {

    }
}
