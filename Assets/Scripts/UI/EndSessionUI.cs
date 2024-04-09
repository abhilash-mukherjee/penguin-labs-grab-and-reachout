using System.Collections;
using UnityEngine;

public class EndSessionUI : SessionUISuperClass
{
    [SerializeField] private TMPro.TextMeshProUGUI scoreUI;
    [SerializeField] private IntegerData score;
    [SerializeField] private GameObject uiObjectReference;
    [SerializeField] private float uiDisplayDelay;

    public override void HideUI(SessionData data)
    {
        uiObjectReference.SetActive(false);
        SetUIState(false);
    }

    public override void ShowUI(SessionData data)
    {
        StartCoroutine(UIDisplayCoroutine(uiDisplayDelay));
    }

    IEnumerator UIDisplayCoroutine(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        uiObjectReference.SetActive(true);
        SetUIState(true);
        scoreUI.text = score.value.ToString();

    }

    private void SetUIState(bool isActive)
    {
        scoreUI.gameObject.SetActive(isActive); 
    }
}