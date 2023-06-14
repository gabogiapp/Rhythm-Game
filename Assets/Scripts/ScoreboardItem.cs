using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ScoreboardItem : MonoBehaviourPunCallbacks
{
    public TMP_Text usernameText;
    public TMP_Text scoreText;
    public TMP_Text missedText;

    Player player;

    public void Initialize(Player player)
    {
        usernameText.text = player.NickName;
        this.player = player;
        UpdateStats();
    }

    void UpdateStats()
    {
        if (player.CustomProperties.TryGetValue("Score", out object score))
        {
            scoreText.text = score.ToString();
        }
        if (player.CustomProperties.TryGetValue("Misses", out object misses))
        {
            missedText.text = misses.ToString();
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer == player)
        {
            if (changedProps.ContainsKey("Score"))
            {
                UpdateStats();
            }
            if (changedProps.ContainsKey("Misses"))
            {
                UpdateStats();
            }
        }
    }
}
