


using System;

public struct PlayerData : IEquatable<PlayerData>
{
    public ulong clientId;

    public bool Equals(PlayerData other)
    {
        return other.clientId == clientId;
    }
}
