using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class CorpseHandler
    {
        [Parser(Opcode.CMSG_CORPSE_QUERY)]
        public static void HandleCorpseQuery(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_CORPSE_RECLAIM_DELAY)]
        public static void HandleCorpseReclaimDelay(Packet packet)
        {
            var hasDelay = !packet.ReadBit("hasDelay");
            if (hasDelay)
                packet.ReadInt32("Delay");
        }

        [Parser(Opcode.SMSG_DEATH_RELEASE_LOC)]
        public static void HandleDeathReleaseLoc(Packet packet)
        {
            packet.ReadEntry<Int32>(StoreNameType.Map, "Map Id");
            var pos = new Vector3();
            pos.Y = packet.ReadSingle();
            pos.X = packet.ReadSingle();
            pos.Z = packet.ReadSingle();

            packet.AddValue("Position", pos);
        }

        [Parser(Opcode.SMSG_CORPSE_QUERY_RESPONSE)]
        public static void HandleCorpseQueryResponse(Packet packet)
        {
            var pos = new Vector3();
            var guid = new byte[8];

            guid[0] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            packet.ReadBit("Corpse Found");
            guid[5] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[6] = packet.ReadBit();

            packet.ReadXORByte(guid, 5);
            pos.Z = packet.ReadSingle();
            packet.ReadXORByte(guid, 1);
            packet.ReadEntry<Int32>(StoreNameType.Map, "Map ID");
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 4);
            pos.X = packet.ReadSingle();
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 0);
            packet.ReadEntry<Int32>(StoreNameType.Map, "Corpse Map ID");
            pos.Y = packet.ReadSingle();

            packet.AddValue("Position", pos);
            packet.WriteGuid("Corpse Low GUID", guid);
        }
    }
}
