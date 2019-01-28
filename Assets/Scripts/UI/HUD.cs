using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{

    [SerializeField]
    private RectTransform completionBar;
    [SerializeField]
    private Text completionText;
    [SerializeField]
    private float completionBarWidth = 100;
    [SerializeField]
    private RectTransform antidoteBar;
    [SerializeField]
    private Text antidoteText;
    [SerializeField]
    private float antidoteBarEmptyY = -4;
    [SerializeField]
    private float antidoteBarFullY = 0;

    private PlayerController player;


    void Start()
    {
        SetAntidote(0);
        SetCompletion(0);
        player = FindObjectOfType<PlayerController>();
    }

    void FixedUpdate()
    {
        float antidote = player.GetAntidotePercent();
        SetAntidote(antidote);

        float completion = GameManager.myManager.GetCompletionPercent();
        SetCompletion(completion);

    }


    public void SetAntidote(float percent)
    {
        antidoteText.text = string.Format("{0:0%}", percent);
        Vector2 s = antidoteBar.anchoredPosition;
        s.y = Mathf.Lerp(antidoteBarEmptyY, antidoteBarFullY, percent);
        antidoteBar.anchoredPosition = s;
    }

    public void SetCompletion(float percent)
    {
        completionText.text = string.Format("{0:0%}", percent);
        Vector2 s = completionBar.sizeDelta;
        s.x = Mathf.Lerp(0, completionBarWidth, percent);
        completionBar.sizeDelta = s;
    }
}
