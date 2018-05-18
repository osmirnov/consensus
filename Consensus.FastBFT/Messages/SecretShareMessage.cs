﻿namespace Consensus.FastBFT.Messages
{
    public class SecretShareMessage : Message
    {
        public int ReplicaId { get; set; }

        public string SecreShare { get; set; }
    }
}