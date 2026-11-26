using UnityEngine;
using UnityEngine.InputSystem;
public class HowToPlayManager : MonoBehaviour
{   
    [SerializeField] private GameObject howToPlayPanel;
    [SerializeField] private GameObject StartButton;
    [SerializeField] private GameObject HowToButton;
    [SerializeField] private GameObject HowToCloseButton;
    [SerializeField] private GameObject ExitButton;


    void Start()
    {
        if (howToPlayPanel != null)
        {
            howToPlayPanel.SetActive(false);
        }
        if (HowToCloseButton != null)
        {
            HowToCloseButton.SetActive(false);
        }
    }
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ClosePanel();
        }
    }
    public void OpenPanel()
    {
        SetPanelState(true);
    }

    public void ClosePanel()
    {
        SetPanelState(false);
    }

    private void SetPanelState(bool isOpen)
    {
  
        if (howToPlayPanel != null) howToPlayPanel.SetActive(isOpen);
        if (HowToCloseButton != null) HowToCloseButton.SetActive(isOpen);


        if (HowToButton != null) HowToButton.SetActive(!isOpen);
        if (StartButton != null) StartButton.SetActive(!isOpen);
        if (ExitButton != null) ExitButton.SetActive(!isOpen);
    }

}
