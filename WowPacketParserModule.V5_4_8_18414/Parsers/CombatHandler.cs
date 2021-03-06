using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using SpellHitInfo = WowPacketParser.Enums.SpellHitInfo;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class CombatHandler
    {
        [Parser(Opcode.CMSG_ATTACKSWING)]
        public static void HandleAttackSwing(Packet packet)
        {
            var guid = packet.StartBitStream(6, 5, 7, 0, 3, 1, 4, 2);
            packet.ParseBitStream(guid, 6, 7, 1, 3, 2, 0, 4, 5);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_TOGGLE_PVP)]
        public static void HandleTogglePvp(Packet packet)
        {
            packet.ReadBit("Value");
        }

        [Parser(Opcode.SMSG_AI_REACTION)]
        public static void HandleAIReaction(Packet packet)
        {
            var guid = packet.StartBitStream(5, 7, 0, 4, 6, 2, 3, 1);
            packet.ParseBitStream(guid, 4, 6, 5);
            packet.ReadEnum<AIReaction>("Reaction", TypeCode.Int32);
            packet.ParseBitStream(guid, 7, 1, 2, 0, 3);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_ATTACKERSTATEUPDATE)]
        public static void HandleAttackerStateUpdate(Packet packet)
        {
            var bit2C = packet.ReadBit("bit2C");
            if (bit2C)
            {
                var bits1C = (int)packet.ReadBits(21);
                packet.ReadInt32("Int18");
                for (var i = 0; i < bits1C; i++)
                {
                    packet.ReadInt32("IntED", i);
                    packet.ReadInt32("IntED", i);
                }

                packet.ReadInt32("Int10");
                packet.ReadInt32("Int14");
            }

            packet.ReadInt32("Length");
            var hitInfo = packet.ReadEnum<SpellHitInfo>("HitInfo", TypeCode.Int32);

            packet.ReadPackedGuid("AttackerGUID");
            packet.ReadPackedGuid("TargetGUID");

            packet.ReadInt32("Damage");
            packet.ReadInt32("OverDamage");

            var subDmgCount = packet.ReadByte("subDmgCount");

            for (var i = 0; i < subDmgCount; i++)
            {
                packet.ReadInt32("SchoolMask", i);
                packet.ReadSingle("Float Damage", i);
                packet.ReadInt32("Int Damage", i);
            }

            if (hitInfo.HasAnyFlag(SpellHitInfo.HITINFO_PARTIAL_ABSORB | SpellHitInfo.HITINFO_FULL_ABSORB))
                for (var i = 0; i < subDmgCount; ++i)
                    packet.ReadInt32("Damage Absorbed", i);

            if (hitInfo.HasAnyFlag(SpellHitInfo.HITINFO_PARTIAL_RESIST | SpellHitInfo.HITINFO_FULL_RESIST))
                for (var i = 0; i < subDmgCount; ++i)
                    packet.ReadInt32("Damage Resisted", i);

            var state = packet.ReadEnum<VictimStates>("VictimState", TypeCode.Byte);
            packet.ReadInt32("Unk Attacker State 0");

            packet.ReadEntry<Int32>(StoreNameType.Spell, "Melee Spell ID");

            var block = 0;
            if (hitInfo.HasAnyFlag(SpellHitInfo.HITINFO_BLOCK))
                block = packet.ReadInt32("Block Amount");

            if (hitInfo.HasAnyFlag(SpellHitInfo.HITINFO_RAGE_GAIN))
                packet.ReadInt32("Rage Gained");

            if (hitInfo.HasAnyFlag(SpellHitInfo.HITINFO_UNK0))
            {
                packet.ReadInt32("Unk Attacker State 3 1");
                packet.ReadSingle("Unk Attacker State 3 2");
                packet.ReadSingle("Unk Attacker State 3 3");
                packet.ReadSingle("Unk Attacker State 3 4");
                packet.ReadSingle("Unk Attacker State 3 5");
                packet.ReadSingle("Unk Attacker State 3 6");
                packet.ReadSingle("Unk Attacker State 3 7");
                packet.ReadSingle("Unk Attacker State 3 8");
                packet.ReadSingle("Unk Attacker State 3 9");
                packet.ReadSingle("Unk Attacker State 3 10");
                packet.ReadSingle("Unk Attacker State 3 11");
                packet.ReadInt32("Unk Attacker State 3 12");
            }

            if (hitInfo.HasAnyFlag(SpellHitInfo.HITINFO_BLOCK | SpellHitInfo.HITINFO_UNK12))
                packet.ReadSingle("Unk Float");
        }
        [Parser(Opcode.SMSG_ATTACKSTART)]
        public static void HandleAttackStartStart(Packet packet)
        {
            var attackerGUID = new byte[8];
            var victimGUID = new byte[8];

            victimGUID[7] = packet.ReadBit();
            attackerGUID[7] = packet.ReadBit();
            attackerGUID[3] = packet.ReadBit();
            victimGUID[3] = packet.ReadBit();
            victimGUID[5] = packet.ReadBit();
            attackerGUID[4] = packet.ReadBit();
            attackerGUID[1] = packet.ReadBit();
            victimGUID[4] = packet.ReadBit();
            attackerGUID[0] = packet.ReadBit();
            victimGUID[6] = packet.ReadBit();
            attackerGUID[5] = packet.ReadBit();
            victimGUID[2] = packet.ReadBit();
            attackerGUID[6] = packet.ReadBit();
            victimGUID[1] = packet.ReadBit();
            attackerGUID[2] = packet.ReadBit();
            victimGUID[0] = packet.ReadBit();
            packet.ReadXORByte(attackerGUID, 5);
            packet.ReadXORByte(attackerGUID, 0);
            packet.ReadXORByte(victimGUID, 5);
            packet.ReadXORByte(attackerGUID, 4);
            packet.ReadXORByte(attackerGUID, 6);
            packet.ReadXORByte(victimGUID, 6);
            packet.ReadXORByte(victimGUID, 1);
            packet.ReadXORByte(victimGUID, 0);
            packet.ReadXORByte(attackerGUID, 7);
            packet.ReadXORByte(victimGUID, 4);
            packet.ReadXORByte(attackerGUID, 2);
            packet.ReadXORByte(victimGUID, 3);
            packet.ReadXORByte(victimGUID, 7);
            packet.ReadXORByte(victimGUID, 2);
            packet.ReadXORByte(attackerGUID, 3);
            packet.ReadXORByte(attackerGUID, 1);

            packet.WriteGuid("Attacker GUID", attackerGUID);
            packet.WriteGuid("Victim GUID", victimGUID);
        }

        [Parser(Opcode.SMSG_ATTACKSTOP)]
        public static void HandleAttackStartStop(Packet packet)
        {
            var victimGUID = new byte[8];
            var attackerGUID = new byte[8];

            victimGUID[5] = packet.ReadBit();
            victimGUID[6] = packet.ReadBit();
            attackerGUID[3] = packet.ReadBit();
            attackerGUID[6] = packet.ReadBit();
            attackerGUID[7] = packet.ReadBit();
            attackerGUID[2] = packet.ReadBit();
            attackerGUID[5] = packet.ReadBit();
            victimGUID[4] = packet.ReadBit();
            packet.ReadBit("Unk Bit");
            victimGUID[3] = packet.ReadBit();
            victimGUID[0] = packet.ReadBit();
            victimGUID[2] = packet.ReadBit();
            victimGUID[7] = packet.ReadBit();
            attackerGUID[4] = packet.ReadBit();
            attackerGUID[1] = packet.ReadBit();
            attackerGUID[0] = packet.ReadBit();
            victimGUID[1] = packet.ReadBit();
            packet.ReadXORByte(victimGUID, 0);
            packet.ReadXORByte(victimGUID, 3);
            packet.ReadXORByte(victimGUID, 5);
            packet.ReadXORByte(victimGUID, 2);
            packet.ReadXORByte(attackerGUID, 0);
            packet.ReadXORByte(attackerGUID, 6);
            packet.ReadXORByte(attackerGUID, 3);
            packet.ReadXORByte(victimGUID, 4);
            packet.ReadXORByte(attackerGUID, 1);
            packet.ReadXORByte(attackerGUID, 4);
            packet.ReadXORByte(victimGUID, 6);
            packet.ReadXORByte(attackerGUID, 5);
            packet.ReadXORByte(attackerGUID, 7);
            packet.ReadXORByte(attackerGUID, 2);
            packet.ReadXORByte(victimGUID, 1);
            packet.ReadXORByte(victimGUID, 7);

            packet.WriteGuid("Attacker GUID", attackerGUID);
            packet.WriteGuid("Victim GUID", victimGUID);
        }

        [Parser(Opcode.SMSG_CANCEL_AUTO_REPEAT)]
        public static void HandleCancelAutoRepeat(Packet packet)
        {
            var guid = packet.StartBitStream(1, 3, 0, 4, 6, 7, 5, 2);
            packet.ParseBitStream(guid, 7, 6, 2, 5, 0, 4, 1, 3);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_CANCEL_COMBAT)]
        public static void HandleCancelCombat(Packet packet)
        {
            packet.ReadBits("Unk16", 2);
        }

        [Parser(Opcode.SMSG_CLEAR_TARGET)]
        public static void HandleSClearTarget(Packet packet)
        {
            var guid = packet.StartBitStream(6, 2, 0, 4, 7, 1, 3, 5);
            packet.ParseBitStream(guid, 4, 0, 3, 5, 2, 7, 6, 1);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_COMBAT_EVENT_FAILED)]
        public static void HandleCombatEventFailed(Packet packet)
        {
            var guid = new byte[8];
            var guid2 = new byte[8];

            guid[2] = packet.ReadBit();
            guid2[2] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid2[4] = packet.ReadBit();
            guid2[0] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid2[6] = packet.ReadBit();
            guid2[7] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid2[1] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid2[5] = packet.ReadBit();
            guid2[3] = packet.ReadBit();
            guid[0] = packet.ReadBit();

            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid2, 4);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid2, 1);
            packet.ReadXORByte(guid2, 0);
            packet.ReadXORByte(guid2, 3);
            packet.ReadXORByte(guid2, 5);
            packet.ReadXORByte(guid2, 7);
            packet.ReadXORByte(guid2, 6);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid2, 2);
            packet.ReadXORByte(guid, 0);

            packet.WriteGuid("Guid", guid);
            packet.WriteGuid("Guid2", guid2);
        }

        [Parser(Opcode.SMSG_DUEL_INBOUNDS)]
        public static void HandleDuelInbounds(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_DUEL_REQUESTED)]
        public static void HandleDuelRequested(Packet packet)
        {
            var guid1 = new byte[8];
            var guid2 = new byte[8];

            guid1[5] = packet.ReadBit();
            guid2[4] = packet.ReadBit();
            guid2[2] = packet.ReadBit();
            guid2[7] = packet.ReadBit();
            guid1[0] = packet.ReadBit();
            guid2[5] = packet.ReadBit();
            guid1[4] = packet.ReadBit();
            guid1[6] = packet.ReadBit();
            guid2[1] = packet.ReadBit();
            guid2[3] = packet.ReadBit();
            guid2[6] = packet.ReadBit();
            guid1[7] = packet.ReadBit();
            guid1[3] = packet.ReadBit();
            guid1[2] = packet.ReadBit();
            guid1[1] = packet.ReadBit();
            guid2[0] = packet.ReadBit();

            packet.ReadXORByte(guid1, 5);
            packet.ReadXORByte(guid1, 3);
            packet.ReadXORByte(guid2, 7);
            packet.ReadXORByte(guid2, 4);
            packet.ReadXORByte(guid1, 7);
            packet.ReadXORByte(guid2, 3);
            packet.ReadXORByte(guid2, 6);
            packet.ReadXORByte(guid2, 0);
            packet.ReadXORByte(guid1, 4);
            packet.ReadXORByte(guid2, 2);
            packet.ReadXORByte(guid2, 1);
            packet.ReadXORByte(guid1, 0);
            packet.ReadXORByte(guid1, 2);
            packet.ReadXORByte(guid1, 6);
            packet.ReadXORByte(guid1, 1);
            packet.ReadXORByte(guid2, 5);

            packet.WriteGuid("Flag GUID", guid1);
            packet.WriteGuid("Opponent GUID", guid2);
        }

        [Parser(Opcode.SMSG_DUEL_WINNER)]
        public static void HandleDuelWinner(Packet packet)
        {
            packet.ReadBit("Abnormal finish");
            var unk24 = packet.ReadBits("unk24", 6);
            var unk73 = packet.ReadBits("unk73", 6);

            packet.ReadInt32("unk20");
            packet.ReadWoWString("str1", unk24);
            packet.ReadInt32("unk16");
            packet.ReadWoWString("str2", unk73);
        }

        [Parser(Opcode.SMSG_ENVIRONMENTALDAMAGELOG)]
        public static void HandleEnvirenmentalDamageLog(Packet packet)
        {
            var guid = new byte[8];

            guid[5] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            var bit30 = packet.ReadBit();
            guid[6] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            if (bit30)
            {
                var bits20 = packet.ReadBits(21);

                packet.ReadInt32("Int14");
                for (var i = 0; i < bits20; ++i)
                {
                    packet.ReadInt32("IntED", i);
                    packet.ReadInt32("IntED", i);
                }

                packet.ReadInt32("Int1C");
                packet.ReadInt32("Int18");
            }

            packet.ReadInt32("Int3C");
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 7);
            packet.ReadEnum<EnvironmentDamage>("Type", TypeCode.Byte);
            packet.ReadXORByte(guid, 6);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 5);
            packet.ReadInt32("Int38");
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 4);
            packet.ReadInt32("Damage");

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_UPDATE_COMBO_POINTS)]
        public static void HandleUpdateComboPoints(Packet packet)
        {
            var guid = packet.StartBitStream(0, 5, 6, 3, 7, 4, 1, 2);
            packet.ParseBitStream(guid, 5, 6, 4, 7, 3, 0);
            packet.ReadByte("Combo Points");
            packet.ParseBitStream(guid, 2, 1);
            packet.WriteGuid("Guid", guid);
        }
    }
}
