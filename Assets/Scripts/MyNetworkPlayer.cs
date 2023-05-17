using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SyncVar(hook = nameof(HandleDisplayNameUpdate))]
    [SerializeField] private string displayName = "Missing Name";

    [SyncVar(hook = nameof(HandleDisplayColorUpdate))]
    [SerializeField] private Color displayColour = Color.red;

    [SerializeField] private TMP_Text displayNameText = null;
    [SerializeField] private Renderer displatColorRenderer = null;

    #region Server
    [Server]
    public void SetDisplayeName(string newDisplayName)
    {
        displayName = newDisplayName;
    }

    [Server]
    public void SetDisplayColour(Color color)
    {
        displayColour = color;
    }

    [Command]
    private void CmdSetDisplayName(string newDisplayName)
    {
        if(newDisplayName.Length < 2 || newDisplayName.Length > 20) { return; }
        RpcLogNewName(newDisplayName);
        SetDisplayeName(newDisplayName);
    }
    #endregion

    #region Client
    private void HandleDisplayColorUpdate(Color oldColour, Color newColor)
    {
        displatColorRenderer.material.SetColor("_BaseColor", newColor);
    }

    private void HandleDisplayNameUpdate(string oldDisplayName, string newDisplayName)
    {
        displayNameText.text = newDisplayName;
    }

    [ContextMenu("SetMyName")]
    private void SetMyName()
    {
        CmdSetDisplayName("M");
    }
    [ClientRpc]
    private void RpcLogNewName(string newDisplayName)
    {
        Debug.Log(newDisplayName);
    }
    #endregion


}
