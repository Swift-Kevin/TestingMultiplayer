using System;
using Unity.Collections;
using Unity.Netcode;

public struct PlayerData : IEquatable<PlayerData>, INetworkSerializable
{
    public ulong clientId;
    public int colorId;
    public FixedString128Bytes clientName;

    public bool Equals(PlayerData other)
    {
        return other.clientId == clientId 
            && other.colorId == colorId 
            && other.clientName == clientName;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientId);
        serializer.SerializeValue(ref colorId);
        serializer.SerializeValue(ref clientName);
    }
}
