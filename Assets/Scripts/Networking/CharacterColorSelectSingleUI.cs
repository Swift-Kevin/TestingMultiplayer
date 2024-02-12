using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterColorSelectSingleUI : MonoBehaviour
{
    [SerializeField] private int colorId;
    [SerializeField] private Image image;
    [SerializeField] private GameObject selectedGameObject;
    [SerializeField] private Button button;


    private void Start()
    {
        image.color = RelayManager.Instance.GetPlayerColor(colorId);
        button.onClick.AddListener(UpdateColor);

        UpdateIsSelected();
    }

    public void UpdateColor()
    {
        Debug.Log("Updating Color Main Func");
        RelayManager.Instance.ChangePlayerColor((byte)colorId);
        UpdateIsSelected();
    }

    private void UpdateIsSelected()
    {
        Debug.Log("Updating Selected Color");

        if (RelayManager.Instance.GetPlayerData().colorId == colorId)
        {
            selectedGameObject.SetActive(true);
        }
        else
        {
            selectedGameObject.SetActive(false);
        }
    }
}
