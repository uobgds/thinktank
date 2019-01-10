using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

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
    private float antidoteBarHeight = 100;

    private PlayerController player;


    void Start()
    {
        SetAntidote(0);
        SetCompletion(0);
        player = GameObject.FindObjectOfType<PlayerController>();
    }

    void OnPreRender()
    {
        if(player == null)
        {
            return;
        }
        // TODO get the data from the player for the bars
    }


    public void SetAntidote(float percent)
    {
        float val = 100 * percent;
        antidoteText.text = string.Format("{0:c1}%", val);
        Vector2 s = antidoteBar.sizeDelta;
        s.y = Mathf.Lerp(0, antidoteBarHeight, percent);
        antidoteBar.sizeDelta = s;
    }

    public void SetCompletion(float percent)
    {
        float val = 100 * percent;
        completionText.text = string.Format("{0:c1}%", val);
        Vector2 s = completionBar.sizeDelta;
        s.x = Mathf.Lerp(0, completionBarWidth, percent);
        completionBar.sizeDelta = s;
    }
}
