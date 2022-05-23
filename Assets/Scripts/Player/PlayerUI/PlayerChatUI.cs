using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using System;
using System.Text;

public class PlayerChatUI : NetworkBehaviour
{

    public GameObject ImageBG;
    public Text chatText;
    public string chat;

    private void Start() 
    {
        GetComponentInParent<MainPlayer>().SetPlayerChatText += SetPlayerChat;
    }

    public void SetPlayerChat()
    {
        // chat = _chat;
        chat = GetComponentInParent<MainPlayer>().clientData.chatText;
        chatText.text = chat;

        StopCoroutine(ShowChatText());
        StartCoroutine(ShowChatText());
    }

    IEnumerator ShowChatText()
    {
        chatText.gameObject.SetActive(true);
        ImageBG.SetActive(true);

        yield return new WaitForSeconds(5f);
        chatText.gameObject.SetActive(false);
        ImageBG.SetActive(false);
    }
}
