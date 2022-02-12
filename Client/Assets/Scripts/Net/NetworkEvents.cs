﻿using System;

using UnityEngine.Events;

using GiantScape.Common.Net.Packets;

namespace GiantScape.Client.Net
{
    [Serializable]
    public class PacketEvent : UnityEvent<NetworkPacket> { }
}
