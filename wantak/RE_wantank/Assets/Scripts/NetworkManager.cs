using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// ��Ʈ��ũ 
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update

    private void Awake()
    {
        // �ػ� ����
        // ����,����,Ǯȭ��
        //Screen.SetResolution(1920, 1080, false);

        /// ��Ʈ��ũ ����
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;

    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();
    public override void OnConnectedToMaster()
    {
        // �� �ٷ� ����, �ִ��ο� 4 
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 4 }, null ); ;
    }

    public override void OnJoinedRoom()
    {
       //  �� ����� ����
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
