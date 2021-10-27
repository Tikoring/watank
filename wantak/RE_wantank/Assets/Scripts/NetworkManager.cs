using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// 네트워크 
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update

    private void Awake()
    {
        // 해상도 설정
        // 가로,세로,풀화면
        //Screen.SetResolution(1920, 1080, false);

        /// 네트워크 설정
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;

    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();
    public override void OnConnectedToMaster()
    {
        // 방 바로 생성, 최대인원 4 
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 4 }, null ); ;
    }

    public override void OnJoinedRoom()
    {
       //  방 입장시 할일
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
