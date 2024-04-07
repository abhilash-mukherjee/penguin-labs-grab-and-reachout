using UnityEngine;

public class GameplayUI : SessionUISuperClass
{
    [SerializeField] private GameObject uiReference;
    public override void HideUI(SessionData data)
    {
        uiReference.SetActive(false);
    }

    public override void ShowUI(SessionData data)
    {
        uiReference.SetActive(true);
    }
}
