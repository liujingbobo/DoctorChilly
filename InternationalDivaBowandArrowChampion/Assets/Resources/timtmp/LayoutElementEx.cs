using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class LayoutElementEx : MonoBehaviour
{
    public LayoutGroup sourceLayout;
    public Text sourceText;
    public TextMeshProUGUI sourceTextMeshPro;
    public bool enableWidth = true;
    public bool enableHeight = true;
    public float maxWidth = float.MaxValue;
    public float maxHeight = float.MaxValue;
    public float widthOffset;
    public float heightOffset;
    private LayoutElement layoutElement;

    private void OnEnable()
    {
        layoutElement = this.GetComponent<LayoutElement>();
    }
    public void Update()
    {
        if (layoutElement && sourceLayout)
        {
            if (enableHeight)
                layoutElement.preferredHeight = Mathf.Min(maxHeight, sourceLayout.preferredHeight) + heightOffset;
            if (enableWidth)
                layoutElement.preferredWidth = Mathf.Min(maxWidth, sourceLayout.preferredWidth) + widthOffset;
        }
        if (layoutElement && sourceText)
        {
            if (enableHeight)
                layoutElement.preferredHeight = Mathf.Min(maxHeight, sourceText.preferredHeight) + heightOffset;
            if (enableWidth)
                layoutElement.preferredWidth = Mathf.Min(maxWidth, sourceText.preferredWidth) + widthOffset;
        }

        if (layoutElement && sourceTextMeshPro)
        {
            if (enableHeight)
                layoutElement.preferredHeight = Mathf.Min(maxHeight, sourceTextMeshPro.preferredHeight) + heightOffset;
            if (enableWidth)
                layoutElement.preferredWidth = Mathf.Min(maxWidth, sourceTextMeshPro.preferredWidth) + widthOffset;
        }
    }
}