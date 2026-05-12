using TMPro;
using UnityEngine;

public class TraitEntry : MonoBehaviour
{
    public TMP_Text traitName;

    public void Initialize(string traitName)
    {
        this.traitName.text = traitName;
    }
}
