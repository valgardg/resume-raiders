using UnityEngine;

public class TraitSheet : MonoBehaviour
{
    public void ToggleTraitSheet()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
