using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NETWORK_ENGINE;
public class DicePlayer : NetworkComponent
{

    public Text Dice;
    public InputField NameField;
    public Button RollButton;
    public override void HandleMessage(string flag, string value)
    {
       
    }

    public override void NetworkedStart()
    {
       if(!IsLocalPlayer)
        {
            NameField.interactable = false;
            RollButton.interactable = false;
        }
    }

    public void UI_SetName(string s)
    {

    }

    public void UI_Roll()
    {

    }

    public override IEnumerator SlowUpdate()
    {
        while(IsConnected)
        {

            if(IsServer)
            {
                if(IsDirty)
                {

                    IsDirty = false;
                }
            }
            yield return new WaitForSeconds(.1f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject temp = GameObject.Find("GameCanvas");
        if(temp == null)
        {
            throw new System.Exception("ERROR: Could not find game canvas on the scene.");
        }
        else
        {
            this.transform.SetParent(temp.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
