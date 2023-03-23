using JetBrains.Annotations;
using Mirror;
using UnityEngine;

namespace Managers.Network
{
    public struct CreateCharacterMessage : NetworkMessage
    {
        public int index;
    }
}