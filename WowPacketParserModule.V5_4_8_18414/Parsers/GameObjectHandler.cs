using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class GameObjectHandler
    {
        [Parser(Opcode.CMSG_GAMEOBJECT_QUERY)]
        public static void HandleGameObjectQuery(Packet packet)
        {
            var entry = packet.ReadUInt32("Entry");

            var guid = packet.StartBitStream(5, 3, 6, 2, 7, 1, 0, 4);
            packet.ParseBitStream(guid, 1, 5, 3, 4, 6, 2, 7, 0);
            packet.WriteGuid("GameObject Guid", guid);
        }

        [Parser(Opcode.CMSG_GAMEOBJ_REPORT_USE)]
        public static void HandleGOReportUse(Packet packet)
        {
            var guid = packet.StartBitStream(4, 7, 5, 3, 6, 1, 2, 0);
            packet.ParseBitStream(guid, 7, 1, 6, 5, 0, 3, 2, 4);
            packet.WriteGuid("GameObject Guid", guid);
        }

        [Parser(Opcode.CMSG_GAMEOBJ_USE)]
        public static void HandleGOUse(Packet packet)
        {
            var guid = packet.StartBitStream(6, 1, 3, 4, 0, 5, 7, 2);
            packet.ParseBitStream(guid, 0, 1, 6, 2, 3, 4, 5, 7);
            packet.WriteGuid("GameObject Guid", guid);
        }

        [Parser(Opcode.SMSG_GAMEOBJECT_CUSTOM_ANIM)]
        public static void HandleGOCustomAnim(Packet packet)
        {
            var guid = new byte[8];
            guid[4] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            var hasAnim = !packet.ReadBit("!hasAnim");
            guid[6] = packet.ReadBit();
            packet.ReadBit("Byte16");
            if (hasAnim)
                packet.ReadInt32("Anim");

            packet.ParseBitStream(guid, 5, 6, 7, 3, 4, 0, 2, 1);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_GAMEOBJECT_DESPAWN_ANIM)]
        public static void HandleGODespawnAnim(Packet packet)
        {
            var guid = packet.StartBitStream(0, 2, 4, 1, 7, 3, 6, 5);
            packet.ParseBitStream(guid, 0, 2, 4, 5, 7, 3, 1, 6);
            packet.WriteGuid("Guid", guid);
        }

        [HasSniffData]
        [Parser(Opcode.SMSG_GAMEOBJECT_QUERY_RESPONSE)]
        public static void HandleGameObjectQueryResponse(Packet packet)
        {
            var gameObject = new GameObjectTemplate();

            packet.ReadByte("Unk1 Byte");

            var entry = packet.ReadEntry("Entry");
            if (entry.Value) // entry is masked
                return;

            var unk1 = packet.ReadInt32("Unk1 UInt32");
            if (unk1 == 0)
            {
                //packet.ReadByte("Unk1 Byte");
                return;
            }

            gameObject.Type = packet.ReadEnum<GameObjectType>("Type", TypeCode.Int32);
            gameObject.DisplayId = packet.ReadUInt32("Display ID");

            var name = new string[4];
            for (var i = 0; i < 4; i++)
                name[i] = packet.ReadCString("Name", i);
            gameObject.Name = name[0];

            gameObject.IconName = packet.ReadCString("Icon Name");
            gameObject.CastCaption = packet.ReadCString("Cast Caption");
            gameObject.UnkString = packet.ReadCString("Unk String");

            gameObject.Data = new int[32];
            for (var i = 0; i < gameObject.Data.Length; i++)
                gameObject.Data[i] = packet.ReadInt32("Data", i);


            gameObject.Size = packet.ReadSingle("Size");

            gameObject.QuestItems = new uint[packet.ReadByte("QuestItems Length")];

            for (var i = 0; i < gameObject.QuestItems.Length; i++)
                gameObject.QuestItems[i] = (uint)packet.ReadEntry<Int32>(StoreNameType.Item, "Quest Item", i);

            packet.ReadEnum<ClientType>("Expansion", TypeCode.UInt32);

            Storage.GameObjectTemplates.Add((uint)entry.Key, gameObject, packet.TimeSpan);

            var objectName = new ObjectName
            {
                ObjectType = ObjectType.GameObject,
                Name = gameObject.Name,
            };
            Storage.ObjectNames.Add((uint)entry.Key, objectName, packet.TimeSpan);
        }
    }
}
