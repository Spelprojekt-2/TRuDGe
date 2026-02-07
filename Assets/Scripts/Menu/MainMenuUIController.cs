using UnityEngine;

public class MainMenuUIController : MonoBehaviour
{
    [SerializeField] private GameObject FirstJoinPopup;
    public void ShowJoinPopup()
    {
        FirstJoinPopup.SetActive(true);
    }

    public void HideJoinPopup()
    {
        FirstJoinPopup.SetActive(false);
    }
}
