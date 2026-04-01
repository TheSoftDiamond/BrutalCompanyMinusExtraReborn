using System;
using Unity.Netcode;
using HarmonyLib;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BrutalCompanyMinus.Minus.Handlers.Modded;

namespace BrutalCompanyMinus.Minus
{
    public struct CustomWeatherStruct : INetworkSerializable, IEquatable<CustomWeatherStruct>
    {
        public string name;
        public float difficultyAdditive;

        public CustomWeatherStruct(string name, float difficultyAdditive)
        {
            this.name = name;
            this.difficultyAdditive = difficultyAdditive;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            if (serializer.IsReader)
            {
                FastBufferReader reader = serializer.GetFastBufferReader();
                reader.ReadValueSafe(out name);
                reader.ReadValueSafe(out difficultyAdditive);
            }
            else
            {
                FastBufferWriter writer = serializer.GetFastBufferWriter();
                writer.WriteValueSafe(name);
                writer.WriteValueSafe(difficultyAdditive);
            }
        }

        public bool Equals(CustomWeatherStruct other)
        {
            return name == other.name;
        }
    }
}