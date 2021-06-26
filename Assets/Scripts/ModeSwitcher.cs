using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Mode
{
    Author,
    Creditor
}
public class ModeSwitcher : MonoBehaviour
{
    public Mode myMode = Mode.Creditor;
    public GameObject[] AuthorObjects;
    public GameObject[] CreditorObjects;
    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }
    void UpdateUI()
    {
        if(myMode == Mode.Creditor)
        {
            foreach(var objects in CreditorObjects)
            {
                objects.SetActive(true);
            }
            foreach (var objects2 in AuthorObjects)
            {
                objects2.SetActive(false);
            }
        }
        else
        {
            foreach (var objects in CreditorObjects)
            {
                objects.SetActive(false);
            }
            foreach (var objects2 in AuthorObjects)
            {
                objects2.SetActive(true);
            }
        }
    }
    public void ToggleMode()
    {
        if(myMode == Mode.Creditor)
        {
            myMode = Mode.Author;
        }
        else
        {
            myMode = Mode.Creditor;
        }
    }
}
