using System;
using WowPacketParser.Enums;
using WowPacketParser.Enums.Version;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;
using Guid = WowPacketParser.Misc.Guid;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class CharacterHandler
    {
        [Parser(Opcode.CMSG_CHAR_CREATE)]
        public static void HandleClientCharCreate(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_CHAR_DELETE)]
        public static void HandleClientCharDelete(Packet packet)
        {
            var guid = packet.StartBitStream(1, 3, 2, 7, 4, 6, 0, 5);
            packet.ParseBitStream(guid, 7, 1, 6, 0, 3, 4, 2, 5);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_EMOTE)]
        public static void HandleEmote(Packet packet)
        {
            packet.ReadInt32("Emote");
        }

        [Parser(Opcode.CMSG_PLAYED_TIME)]
        public static void HandleCPlayedTime(Packet packet)
        {
            packet.ReadBoolean("Print in chat");
        }

        [Parser(Opcode.CMSG_REORDER_CHARACTERS)]
        public static void HandleReorderCharacters(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_SHOWING_CLOAK)]
        [Parser(Opcode.CMSG_SHOWING_HELM)]
        public static void HandleShowingCloakAndHelm(Packet packet)
        {
            packet.ReadBoolean("Showing");
        }

        [Parser(Opcode.CMSG_STANDSTATECHANGE)]
        public static void HandleStandStateChange(Packet packet)
        {
            packet.ReadInt32("Standstate");
        }

        [Parser(Opcode.CMSG_TEXT_EMOTE)]
        public static void HandleTextEmote(Packet packet)
        {
            packet.ReadEnum<EmoteTextType>("Text Emote ID", TypeCode.Int32);
            packet.ReadEnum<EmoteType>("Emote ID", TypeCode.Int32);
            var guid = packet.StartBitStream(6, 7, 3, 2, 0, 5, 1, 4);
            packet.ResetBitReader();
            packet.ParseBitStream(guid, 0, 5, 1, 4, 2, 3, 7, 6);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_CHAR_CREATE)]
        public static void HandleCharCreate(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.SMSG_CHAR_DELETE)]
        public static void HandleCharDelete(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.SMSG_CHAR_ENUM)]
        public static void HandleCharEnum(Packet packet)
        {
            // імена не перевірено, лише послідовність типів данних
            var unkCounter = packet.ReadBits("Unk Counter", 21);//[DW5]
            var count = packet.ReadBits("Char count", 16);//[DW9]

            var charGuids = new byte[count][];
            var guildGuids = new byte[count][];
            var firstLogins = new bool[count];
            var nameLenghts = new uint[count];

            for (int c = 0; c < count; ++c)
            {
                charGuids[c] = new byte[8];
                guildGuids[c] = new byte[8];

                guildGuids[c][4] = packet.ReadBit();
                charGuids[c][0] = packet.ReadBit();
                guildGuids[c][3] = packet.ReadBit();
                charGuids[c][3] = packet.ReadBit();
                charGuids[c][7] = packet.ReadBit();
                packet.ReadBit("unk bit 124", c); //124
                firstLogins[c] = packet.ReadBit(); //108
                charGuids[c][6] = packet.ReadBit();
                guildGuids[c][6] = packet.ReadBit();
                nameLenghts[c] = packet.ReadBits(6);
                charGuids[c][1] = packet.ReadBit();
                guildGuids[c][1] = packet.ReadBit();
                guildGuids[c][0] = packet.ReadBit();
                charGuids[c][4] = packet.ReadBit();
                guildGuids[c][7] = packet.ReadBit();
                charGuids[c][2] = packet.ReadBit();
                charGuids[c][5] = packet.ReadBit();
                guildGuids[c][2] = packet.ReadBit();
                guildGuids[c][5] = packet.ReadBit();
            }//+=416

            //packet.ResetBitReader();
            packet.ReadBit("UnkB16");

            for (int c = 0; c < count; ++c)
            {
                packet.ReadInt32("DW132", c);
                packet.ReadXORByte(charGuids[c], 1);//1
                packet.ReadByte("Slot", c); //57
                packet.ReadByte("Hair Style", c); //63
                packet.ReadXORByte(guildGuids[c], 2);//90
                packet.ReadXORByte(guildGuids[c], 0);//88
                packet.ReadXORByte(guildGuids[c], 6);//94
                var name = packet.ReadWoWString("Name", (int)nameLenghts[c], c);
                packet.ReadXORByte(guildGuids[c], 3);//91
                var x = packet.ReadSingle("Position X", c); //4Ch
                packet.ReadInt32("DW104", c);
                packet.ReadByte("Face", c); //62
                var Class = packet.ReadEnum<Class>("Class", TypeCode.Byte, c); //59
                packet.ReadXORByte(guildGuids[c], 5); //93
             
                for (var itm = 0; itm < 23; itm++)
                {
                    packet.ReadInt32("Item EnchantID", c, itm); //140 prolly need to replace those 2
                    packet.ReadEnum<InventoryType>("Item InventoryType", TypeCode.Byte, c, itm); //144
                    packet.ReadInt32("Item DisplayID", c, itm); //136
                }

                packet.ReadEnum<CustomizationFlag>("CustomizationFlag", TypeCode.UInt32, c); //100
                packet.ReadXORByte(charGuids[c], 3); //3
                packet.ReadXORByte(charGuids[c], 5); //5
                packet.ReadInt32("PetFamily", c); //120
                packet.ReadXORByte(guildGuids[c], 4); //92
                var mapId = packet.ReadInt32("Map", c); //72
                var race = packet.ReadEnum<Race>("Race", TypeCode.Byte, c); //58
                packet.ReadByte("Skin", c); //61
                packet.ReadXORByte(guildGuids[c], 1); //89
                var level = packet.ReadByte("Level", c); //66
                packet.ReadXORByte(charGuids[c], 0); //0
                packet.ReadXORByte(charGuids[c], 2); //2
                packet.ReadByte("Hair Color", c); //64
                packet.ReadEnum<Gender>("Gender", TypeCode.Byte, c); //60
                packet.ReadByte("Facial Hair", c); //65
                packet.ReadInt32("Pet Level", c); //116
                packet.ReadXORByte(charGuids[c], 4); //4
                packet.ReadXORByte(charGuids[c], 7); //7
                var y = packet.ReadSingle("Position Y", c); //50h
                packet.ReadInt32("Pet DisplayID", c); //112
                packet.ReadInt32("DW128", c);
                packet.ReadXORByte(charGuids[c], 6); //6
                packet.ReadEnum<CharacterFlag>("CharacterFlag", TypeCode.Int32, c); //96
                var zone = packet.ReadEntryWithName<UInt32>(StoreNameType.Zone, "Zone Id", c); //68
                packet.ReadXORByte(guildGuids[c], 7); //95
                var z = packet.ReadSingle("Position Z", c); //54h

                var playerGuid = new Guid(BitConverter.ToUInt64(charGuids[c], 0));

                packet.WriteGuid("Character GUID", charGuids[c], c);
                packet.WriteGuid("Guild GUID", guildGuids[c], c);

                if (firstLogins[c])
                {
                    var startPos = new StartPosition();
                    startPos.Map = mapId;
                    startPos.Position = new Vector3(x, y, z);
                    startPos.Zone = zone;

                    Storage.StartPositions.Add(new Tuple<Race, Class>(race, Class), startPos, packet.TimeSpan);
                }

                var playerInfo = new Player { Race = race, Class = Class, Name = name, FirstLogin = firstLogins[c], Level = level };
                if (Storage.Objects.ContainsKey(playerGuid))
                    Storage.Objects[playerGuid] = new Tuple<WoWObject, TimeSpan?>(playerInfo, packet.TimeSpan);
                else
                    Storage.Objects.Add(playerGuid, playerInfo, packet.TimeSpan);
                StoreGetters.AddName(playerGuid, name);
            }

            for (var i = 0; i < unkCounter; i++)
            {
                packet.ReadByte("Unk byte", i); // char_table+28+i*8
                packet.ReadUInt32("Unk int", i); // char_table+24+i*8
            }
        }

        [Parser(Opcode.SMSG_EMOTE)]
        public static void HandleSEmote(Packet packet)
        {
            var guid = new byte[8];
            var guid2 = new byte[8];

            guid[1] = packet.ReadBit();
            guid2[7] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid2[5] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid2[6] = packet.ReadBit();
            guid2[2] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid2[0] = packet.ReadBit();
            guid2[1] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid2[3] = packet.ReadBit();
            guid2[4] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[5] = packet.ReadBit();

            packet.ReadXORByte(guid2, 2);
            packet.ReadXORByte(guid2, 1);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid2, 7);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 2);
            packet.ReadEnum<EmoteTextType>("Text Emote ID", TypeCode.Int32);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid2, 0);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid2, 6);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid2, 3);
            packet.ReadXORByte(guid2, 5);
            packet.ReadXORByte(guid2, 4);
            packet.ReadEnum<EmoteType>("Emote ID", TypeCode.Int32);
            packet.WriteGuid("Caster", guid);
            packet.WriteGuid("Target", guid2);
        }

        [Parser(Opcode.SMSG_INIT_CURRENCY)]
        public static void HandleInitCurrency(Packet packet)
        {
            var count = packet.ReadBits("Count", 21);
            if (count == 0)
                return;

            var hasWeekCount = new bool[count];
            var hasWeekCap = new bool[count];
            var hasSeasonTotal = new bool[count];
            var flags = new uint[count];
            for (var i = 0; i < count; ++i)
            {
                hasSeasonTotal[i] = packet.ReadBit("hasSeasonTotal", i);
                flags[i] = packet.ReadBits("flags", 5, i);
                hasWeekCap[i] = packet.ReadBit("hasWeekCap", i);
                hasWeekCount[i] = packet.ReadBit("hasWeekCount", i);
            }

            for (var i = 0; i < count; ++i)
            {
                if (hasWeekCount[i])
                    packet.ReadUInt32("Weekly count", i);

                packet.ReadUInt32("Entry", i);

                if (hasSeasonTotal[i])
                    packet.ReadUInt32("Season count", i);

                packet.ReadUInt32("Currency count", i);

                if (hasWeekCap[i])
                    packet.ReadUInt32("Weekly cap", i);
            }
        }

        [Parser(Opcode.SMSG_POWER_UPDATE)]
        public static void HandlePowerUpdate(Packet packet)
        {
            var guid = packet.StartBitStream(4, 6, 7, 5, 2, 3, 0, 1);

            var count = packet.ReadBits("Count", 21);

            packet.ParseBitStream(guid, 7, 0, 5, 3, 1, 2, 4);

            for (var i = 0; i < count; i++)
            {
                packet.ReadEnum<PowerType>("Power type", TypeCode.Byte, i); // Actually powertype for class
                packet.ReadInt32("Value", i);
            }

            packet.ReadXORByte(guid, 6);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_STANDSTATE_UPDATE)]
        public static void HandleStandStateUpdate(Packet packet)
        {
            packet.ReadByte("Standstate");
        }
    }
}
