using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConsoleLogEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI exceptionText;
    [SerializeField] private TextMeshProUGUI exceptionCountText;

    public void SetExceptionText(string e)
    {
        exceptionText.text = e;
    }

    public void UpdateCount(int count)
    {
        exceptionCountText.text = count.ToString();
    }

}
