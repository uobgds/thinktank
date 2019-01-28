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

    [SerializeField]
    private RectTransform healthBar;
    [SerializeField]
    private Text healthText;
    [SerializeField]
    private float healthBarWidth = 100;

    private PlayerController player;


    void Start()
    {
        SetAntidote(0);
        SetCompletion(0);
        SetHealth(1);
        player = FindObjectOfType<PlayerController>();
    }

    void FixedUpdate()
    {
        float antidote = player.GetAntidotePercent();
        SetAntidote(antidote);

        float completion = GameManager.myManager.GetCompletionPercent();
        SetCompletion(completion);

        float health = player.GetHealthPercent();
        SetHealth(health);

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

    public void SetHealth(float percent)
    {
        healthText.text = string.Format("{0:0%}", percent);
        Vector2 s = healthBar.sizeDelta;
        s.x = Mathf.Lerp(0, healthBarWidth, percent);
        healthBar.sizeDelta = s;
    }
}
