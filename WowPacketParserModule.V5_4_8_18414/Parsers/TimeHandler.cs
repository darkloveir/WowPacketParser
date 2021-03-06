using System;
using System.Collections;
using System.Collections.Generic;
using WowPacketParser.Enums;
using WowPacketParser.Enums.Version;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;
using CoreParsers = WowPacketParser.Parsing.Parsers;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class TimeHandler
    {
        [Parser(Opcode.SMSG_GAMETIME_UPDATE)]
        public static void HandleGameTimeUpdate(Packet packet)
        {
            packet.ReadPackedTime("Int28"); // 28
            packet.ReadPackedTime("Int16"); // 16
            packet.ReadInt32("Int20"); // 20
            packet.ReadInt32("Int24"); // 24
        }

        [Parser(Opcode.SMSG_LOGIN_SETTIMESPEED)]
        public static void HandleLoginSetTimeSpeed(Packet packet)
        {
            packet.ReadInt32("unk32");
            packet.ReadPackedTime("Time1");
            packet.ReadInt32("unk20");
            packet.ReadPackedTime("Time2");
            packet.ReadSingle("unk28");
        }

        [Parser(Opcode.SMSG_START_TIMER)]
        public static void HandleStartTimer(Packet packet)
        {
            packet.ReadInt32("unk20");
            packet.ReadInt32("unk24");
            packet.ReadInt32("unk16");
        }

        [Parser(Opcode.SMSG_TIME_ADJUSTMENT)]
        public static void HandleTimeAdjustement(Packet packet)
        {
            packet.ReadSingle("unk1");
            packet.ReadInt32("unk2");
        }
    }
}