using UnityEngine;

public class SettingsMainMenu : SettingsMenu
{
    
    public override void back()
    {
        transform.parent.GetChild(transform.GetSiblingIndex() - 1).GetComponent<MainMenu>().closeSettings();
    }

}
