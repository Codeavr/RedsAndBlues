﻿using System;
using RedsAndBlues.GameArea;

namespace RedsAndBlues
{
    [Serializable]
    public struct GameSaveData
    {
        public string SerializedWorld;
        public SimulationTime SimulationTime;
        public GameAreaSettings GameAreaSettings;
        public long TimeStamp;
    }
}