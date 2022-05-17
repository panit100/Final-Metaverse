using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public struct DataCollect : INetworkSerializable
{
    public string playerName;

    public DataCollect(string _playerName)
    {
        playerName = _playerName;
    }
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref playerName);
    }
}

public struct DataChatCollect : INetworkSerializable
{
    public string chatText;

    public DataChatCollect(string _chatText)
    {
        chatText = _chatText;
    }
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref chatText);
    }
}
