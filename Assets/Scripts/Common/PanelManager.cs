using CoreKit.UI;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField] private PanelControl _startShowPanel;

    private PanelControl _currentPanel;

    private void Start()
    {
        ChangePanel(_startShowPanel);
    }

    public void ChangePanel(PanelControl panel)
    {
        _currentPanel?.OnHidden();
        _currentPanel = panel;
        _currentPanel?.OnActive();

    }
}
