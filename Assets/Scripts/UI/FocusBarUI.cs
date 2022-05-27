using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FocusBarUI : BaseUI
{
    PlayerInteraction playerInstance;
    public TextMeshProUGUI focusText;

    private void Start()
    {
        playerInstance = PlayerManager.instance.playerInteraction;
        playerInstance.onFocusChanged += UpdateUI;
        //UpdateUI();

    }

    void Update()
    {
        //UpdateUI();
    }

    public override void UpdateUI()
    {
        string text = "Nothing focused";

        if (playerInstance.focus != null)
            text = playerInstance.focus.nameOfObject;

        focusText.text = text;
    }


}
