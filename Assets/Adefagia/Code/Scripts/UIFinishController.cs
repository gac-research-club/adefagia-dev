using TMPro;
using UnityEngine;

public class UIFinishController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textTeamName;

    public void ChangeName(string teamName)
    {
        textTeamName.text = $"Team {teamName} Win";
    }
}
