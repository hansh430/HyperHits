using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    private TMP_Text tileText;
    private Image fill;
    private Outline outline;

    public char letter { get; private set; }
    public State state { get; private set; }

    private void Awake()
    {
        tileText = GetComponentInChildren<TMP_Text>();
        fill = GetComponent<Image>();
        outline = GetComponent<Outline>();
    }
    public void SetLetter(char letter)
    {
        this.letter = letter;
        tileText.text = letter.ToString();
    }
    public void SetState(State state)
    {
        this.state = state;
        fill.color = state.fillColor;
        outline.effectColor = state.outlineColor;
    }
}
