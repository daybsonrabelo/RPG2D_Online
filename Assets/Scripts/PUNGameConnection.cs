using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PUNGameConnection : MonoBehaviourPunCallbacks
{
    public Text txtLog;
    public GameObject playerPrefab;
    public Transform initialPosition;

    private void Awake()
    {
        AddLogText("Conectando ao servidor...");
        PhotonNetwork.LocalPlayer.NickName = "Guest_" + Random.Range(0, 1000);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        AddLogText("Conectado ao servidor!");
        if (!PhotonNetwork.InLobby)
        {
            AddLogText("Entrando no Lobby...!");
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnJoinedLobby()
    {
        AddLogText("Entrou no Lobby...!");
        RoomOptions roomOpt = new RoomOptions() { MaxPlayers = 20 };
        PhotonNetwork.JoinOrCreateRoom("DBS_Room", roomOpt, TypedLobby.Default);
        //PhotonNetwork.JoinRoom("DBS_Room");
        AddLogText("Entrando na Sala DBS_Room!");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        AddLogText("Erro ao entrar na sala: " + message + " | codigo: " + returnCode);
        //if (returnCode == ErrorCode.GameDoesNotExist)
        //{
        //    AddLogText("Criando sala DBS_Room...");
        //}
    }

    public override void OnJoinedRoom()
    {
        AddLogText("Você entrou na sala DBS_Room! Nickname: " + PhotonNetwork.LocalPlayer.NickName);

        //Instantiate(playerPrefab, initialPosition.position, initialPosition.rotation);
        PhotonNetwork.Instantiate("Player", initialPosition.position, initialPosition.rotation);
    }

    public override void OnLeftRoom()
    {
        AddLogText("Você saiu da sala DBS_Room!");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddLogText("Um jogador entrou na sala DBS_Room! Nickname: " + newPlayer.NickName);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        AddLogText("O jogador: " + otherPlayer.NickName + " saiu da sala!");
    }

    public override void OnErrorInfo(ErrorInfo errorInfo)
    {
        AddLogText("Aconteceu um erro! " + errorInfo.Info);
    }

    private void AddLogText(string text)
    {
        txtLog.text += "\n" + text;
    }
}
