using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class GuildHandler
    {
        [Parser(Opcode.CMSG_AUTO_DECLINE_GUILD_INVITES)]
        public static void HandleAutodeclineGuildInvites(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_GUILD_ACCEPT)]
        public static void HandleGuildAccept(Packet packet)
        {
        }

        [Parser(Opcode.CMSG_GUILD_ADD_RANK)]
        public static void HandleGuildAddRank(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_GUILD_ASSIGN_MEMBER_RANK)]
        public static void HandleGuildAssignMemberRank(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_GUILD_BANK_BUY_TAB)]
        public static void HandleGuildBankBuyTab(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_GUILD_BANK_DEPOSIT_MONEY)]
        public static void HandleGuildBankDepositMoney(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_GUILD_BANK_LOG_QUERY)]
        public static void HandleGuildBankLogQuery(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_GUILD_BANK_UPDATE_TAB)]
        public static void HandleGuildBankUpdateTab(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_GUILD_BANKER_ACTIVATE)]
        public static void HandleGuildBankerActivate(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_GUILD_DECLINE)]
        public static void HandleGuildDecline(Packet packet)
        {
        }

        [Parser(Opcode.CMSG_GUILD_DEL_RANK)]
        public static void HandleGuildDelRank(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_GUILD_DEMOTE)]
        public static void HandleGuildDemote(Packet packet)
        {
            var guid = packet.StartBitStream(3, 6, 0, 2, 7, 5, 4, 1);
            packet.ParseBitStream(guid, 7, 4, 2, 5, 1, 3, 0, 6);
            packet.WriteGuid("GUID", guid);
        }

        [Parser(Opcode.CMSG_GUILD_DISBAND)]
        public static void HandleGuildDisband(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_GUILD_EVENT_LOG_QUERY)]
        public static void HandleGuildEventLogQuery(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_GUILD_INFO_TEXT)]
        public static void HandleGuildInfoText(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_GUILD_INVITE)]
        public static void HandleGuildInvite(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_GUILD_LEAVE)]
        public static void HandleGuildLeave(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_GUILD_MOTD)]
        public static void HandleGuildMotd(Packet packet)
        {
            var len = packet.ReadBits("Len", 10);
            packet.ReadWoWString("Motd", len);
        }

        [Parser(Opcode.SMSG_GUILD_ROSTER)] // sub_6A8B6B
        public static void HandleGuildRoster(Packet packet)
        {
            var count16 = packet.ReadBits("count16", 17);
            var count2041 = packet.ReadBits("count2041", 10);
            var guid = new byte[count16][];
            var unk53 = new uint[count16];
            var unk238 = new uint[count16];
            var unk109 = new uint[count16];
            for (var i = 0; i < count16; i++)
            {
                guid[i] = new byte[8];
                unk238[i] = packet.ReadBits("unk238*4", 8, i); // 238
                guid[i][5] = packet.ReadBit();
                packet.ReadBit("unk372*4", i); // 372
                unk109[i] = packet.ReadBits("unk109*4", 8, i); // 109
                guid[i][7] = packet.ReadBit();
                guid[i][0] = packet.ReadBit();
                guid[i][6] = packet.ReadBit();
                unk53[i] = packet.ReadBits("unk53*4", 6, i); // 53
                packet.ReadBit("unk371*4", i); // 371
                guid[i][3] = packet.ReadBit();
                guid[i][4] = packet.ReadBit();
                guid[i][1] = packet.ReadBit();
                guid[i][2] = packet.ReadBit();
            }
            var count40 = packet.ReadBits("count40", 11);
            for (var i = 0; i < count16; i++)
            {
                packet.ReadByte("unk384", i); // 384
                packet.ReadInt32("unk41*4", i); // 41
                packet.ReadWoWString("str", unk53[i], i);
                packet.ParseBitStream(guid[i], 0);
                for (var j = 0; j < 2; j++)
                {
                    packet.ReadInt32("unk381*4", i, j); // 381
                    packet.ReadInt32("unk373*4", i, j); // 373
                    packet.ReadInt32("unk377*4", i, j); // 377
                }
                packet.ReadByte("unk383", i); // 383
                packet.ReadByte("unk382", i); // 382
                packet.ReadInt32("unk33*4", i); // 33
                packet.ReadInt32("unk45*4", i); // 45
                packet.ParseBitStream(guid[i], 3);
                packet.ReadInt64("unk36", i); // 36
                packet.ReadWoWString("str2", unk238[i], i);
                packet.ReadSingle("unk2ch", i); // 2ch
                packet.ReadByte("unk385", i); // 385
                packet.ReadInt32("unk29", i); // 29
                packet.ReadInt32("unk105", i); // 105
                packet.ParseBitStream(guid[i], 5, 7);
                packet.ReadWoWString("str3", unk109[i], i);
                packet.ParseBitStream(guid[i], 4);
                packet.ReadInt64("unk52", i); // 52
                packet.ReadInt32("unk37*4", i); // 37
                packet.ParseBitStream(guid[i], 6, 1, 2);
                packet.WriteGuid("Guid", guid[i], i);
            }
            packet.ReadInt32("unk36"); // 36
            packet.ReadPackedTime("Time");
            packet.ReadWoWString("str4", count40);
            packet.ReadInt32("unk647*4"); // 647
            packet.ReadWoWString("str5", count2041);
            packet.ReadInt32("unk32"); // 32
        }

        [Parser(Opcode.CMSG_GUILD_SET_ACHIEVEMENT_TRACKING)]
        public static void HandleGuildSetAchievementTracking(Packet packet)
        {
            var count = packet.ReadBits("Count", 22);
            for (var i = 0; i < count; ++i)
                packet.ReadUInt32("Criteria Id", i); // 24
        }

        [Parser(Opcode.CMSG_GUILD_NEWS_UPDATE_STICKY)]
        [Parser(Opcode.CMSG_GUILD_PROMOTE)]
        [Parser(Opcode.CMSG_GUILD_QUERY_NEWS)]
        [Parser(Opcode.CMSG_GUILD_QUERY_RANKS)]
        [Parser(Opcode.CMSG_GUILD_REMOVE)]
        [Parser(Opcode.CMSG_GUILD_REQUEST_CHALLENGE_UPDATE)]
        [Parser(Opcode.CMSG_GUILD_ROSTER)]
        [Parser(Opcode.CMSG_GUILD_SET_GUILD_MASTER)]
        [Parser(Opcode.SMSG_GUILD_COMMAND_RESULT)]
        [Parser(Opcode.SMSG_GUILD_NEWS_UPDATE)]
        [Parser(Opcode.SMSG_GUILD_RANKS_UPDATE)]
        [Parser(Opcode.SMSG_GUILD_REPUTATION_WEEKLY_CAP)]
        [Parser(Opcode.SMSG_GUILD_REWARDS_LIST)]
        [Parser(Opcode.SMSG_GUILD_XP)]
        [Parser(Opcode.SMSG_GUILD_XP_GAIN)]
        [Parser(Opcode.SMSG_PETITION_ALREADY_SIGNED)]
        [Parser(Opcode.SMSG_PETITION_QUERY_RESPONSE)]
        [Parser(Opcode.SMSG_PETITION_RENAME_RESULT)]
        [Parser(Opcode.SMSG_PETITION_SHOWLIST)]
        [Parser(Opcode.SMSG_PETITION_SHOW_SIGNATURES)]
        [Parser(Opcode.SMSG_PETITION_SIGN_RESULTS)]
        [Parser(Opcode.SMSG_TURN_IN_PETITION_RESULTS)]
        public static void HandleGuild(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_GUILD_QUERY)]
        public static void HandleGuildQuery(Packet packet)
        {
            var playerGUID = new byte[8];
            var guildGUID = new byte[8];

            playerGUID[7] = packet.ReadBit();
            playerGUID[3] = packet.ReadBit();
            playerGUID[4] = packet.ReadBit();
            guildGUID[3] = packet.ReadBit();
            guildGUID[4] = packet.ReadBit();
            playerGUID[2] = packet.ReadBit();
            playerGUID[6] = packet.ReadBit();
            guildGUID[2] = packet.ReadBit();
            guildGUID[5] = packet.ReadBit();
            playerGUID[1] = packet.ReadBit();
            playerGUID[5] = packet.ReadBit();
            guildGUID[7] = packet.ReadBit();
            playerGUID[0] = packet.ReadBit();
            guildGUID[1] = packet.ReadBit();
            guildGUID[6] = packet.ReadBit();
            guildGUID[0] = packet.ReadBit();

            packet.ReadXORByte(playerGUID, 7);
            packet.ReadXORByte(guildGUID, 2);
            packet.ReadXORByte(guildGUID, 4);
            packet.ReadXORByte(guildGUID, 7);
            packet.ReadXORByte(playerGUID, 6);
            packet.ReadXORByte(playerGUID, 0);
            packet.ReadXORByte(guildGUID, 6);
            packet.ReadXORByte(guildGUID, 0);
            packet.ReadXORByte(guildGUID, 3);
            packet.ReadXORByte(playerGUID, 2);
            packet.ReadXORByte(guildGUID, 5);
            packet.ReadXORByte(playerGUID, 3);
            packet.ReadXORByte(guildGUID, 1);
            packet.ReadXORByte(playerGUID, 4);
            packet.ReadXORByte(playerGUID, 1);
            packet.ReadXORByte(playerGUID, 5);

            packet.WriteGuid("Player GUID", playerGUID);
            packet.WriteGuid("Guild GUID", guildGUID);
        }

        [Parser(Opcode.CMSG_OFFER_PETITION)]
        public static void HandlePetitionOffer(Packet packet)
        {
            packet.ReadUInt32("Unk UInt32");
            var guid = new byte[8];
            var targetGuid = new byte[8];
            targetGuid[4] = packet.ReadBit();
            targetGuid[1] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            targetGuid[6] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            targetGuid[2] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            targetGuid[3] = packet.ReadBit();
            targetGuid[7] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            targetGuid[5] = packet.ReadBit();
            targetGuid[0] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[7] = packet.ReadBit();

            packet.ReadXORByte(targetGuid, 7);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(targetGuid, 6);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(targetGuid, 0);
            packet.ReadXORByte(targetGuid, 2);
            packet.ReadXORByte(targetGuid, 5);
            packet.ReadXORByte(targetGuid, 3);
            packet.ReadXORByte(targetGuid, 4);
            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(targetGuid, 1);
            packet.ReadXORByte(guid, 6);
            packet.WriteGuid("Guid", guid);
            packet.WriteGuid("targetGuid", targetGuid);
        }

        [Parser(Opcode.CMSG_PETITION_BUY)]
        public static void HandlePetitionBuy(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_PETITION_DECLINE)]
        public static void HandlePetitionDecline(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_PETITION_QUERY)]
        public static void HandlePetitionQuery(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_PETITION_RENAME)]
        public static void HandlePetitionRename(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_PETITION_SHOW_SIGNATURES)]
        public static void HandlePetitionShowSignatures(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_PETITION_SHOWLIST)]
        public static void HandlePetitionShowlist(Packet packet)
        {
            var guid = packet.StartBitStream(1, 7, 2, 5, 4, 0, 3, 6);
            packet.ParseBitStream(guid, 6, 3, 2, 4, 1, 7, 5, 0);
            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.CMSG_PETITION_SIGN)]
        public static void HandlePetitionSign(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_QUERY_GUILD_REWARDS)]
        public static void HandleQueryGuildRewards(Packet packet)
        {
            packet.ReadInt32("unk");
        }

        [Parser(Opcode.CMSG_QUERY_GUILD_XP)]
        public static void HandleQueryGuildXp(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.CMSG_TURN_IN_PETITION)]
        public static void HandlePetitionTurnIn(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.SMSG_GUILD_BANK_LIST)]
        public static void HandleServerGuildBankList(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.SMSG_GUILD_BANK_LOG_QUERY_RESULT)]
        public static void HandleServerGuildBankLogQueryResult(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.SMSG_GUILD_CHALLENGE_UPDATED)]
        public static void HandleServerGuildChallengeUpdated(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.SMSG_GUILD_EVENT_LOG_QUERY_RESULT)]
        public static void HandleServerGuildEventLogQueryResult(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.SMSG_GUILD_INVITE)]
        public static void HandleServerGuildInvite(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.SMSG_GUILD_INVITE_CANCEL)]
        public static void HandleServerGuildInviteCancel(Packet packet)
        {
        }

        [Parser(Opcode.SMSG_GUILD_MEMBER_DAILY_RESET)]
        public static void HandleServerGuildMemberDailyReset(Packet packet)
        {
            packet.ReadToEnd();
        }

        [Parser(Opcode.SMSG_GUILD_NEWS_TEXT)]
        public static void HandleNewText(Packet packet)
        {
            packet.ReadWoWString("Text", (int)packet.ReadBits(10));
        }

        [Parser(Opcode.SMSG_GUILD_QUERY_RESPONSE)]
        public static void HandleGuildQueryResponse(Packet packet)
        {
            var guid = new byte[8];
            var guid2 = new byte[8];

            var len = new uint[255];
            var count = 0u;
            var len2 = 0u;


            guid[5] = packet.ReadBit();
            var unk16 = packet.ReadBit("unk16");
            if (unk16)
            {
                count = packet.ReadBits("cnt", 21);
                guid2[5] = packet.ReadBit();
                guid2[1] = packet.ReadBit();
                guid2[4] = packet.ReadBit();
                guid2[7] = packet.ReadBit();
                for (var i = 0; i < count; i++)
                {
                    len[i] = packet.ReadBits(7);
                }
                guid2[3] = packet.ReadBit();
                guid2[2] = packet.ReadBit();
                guid2[0] = packet.ReadBit();
                guid2[6] = packet.ReadBit();

                len2 = packet.ReadBits("unk36", 7);
            }
            guid[3] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[0] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            if (unk16)
            {
                packet.ReadInt32("unk160");
                packet.ReadInt32("unk152");
                packet.ParseBitStream(guid2, 2, 7);
                packet.ReadInt32("unk156");
                packet.ReadInt32("RealmID"); // 32
                for (var i = 0; i < count; i++)
                {
                    packet.ReadInt32("unk144", i);
                    packet.ReadInt32("unk140", i);
                    packet.ReadWoWString("str", len[i], i);
                }
                packet.ReadWoWString("str", len2);
                packet.ReadInt32("unk168");
                packet.ParseBitStream(guid2, 5, 4);
                packet.ReadInt32("unk164");
                packet.ParseBitStream(guid2, 1, 6, 0, 3);
                packet.WriteGuid("Guid2", guid2);
            }
            packet.ParseBitStream(guid, 2, 6, 4, 0, 7, 3, 5, 1);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_GUILD_RANK)]
        public static void HandleGuildRankServer(Packet packet)
        {
            const int guildBankMaxTabs = 8;
            var count = packet.ReadBits("Count", 17);
            var length = new uint[count];

            for (var i = 0; i < count; ++i)
                length[i] = packet.ReadBits(7);

            for (var i = 0; i < count; ++i)
            {
                packet.ReadInt32("Creation Order", i);
                packet.ReadInt32("Gold Per Day", i);

                for (var j = 0; j < guildBankMaxTabs; ++j)
                {
                    packet.ReadInt32("Tab Slots", i, j);
                    packet.ReadEnum<GuildBankRightsFlag>("Tab Rights", TypeCode.Int32, i, j);
                }

                packet.ReadWoWString("Name", length[i], i);

                packet.ReadInt32("Rights Order", i);
                packet.ReadInt32("Unk 1", i);
            }
        }

        [Parser(Opcode.CMSG_GUILD_SET_RANK_PERMISSIONS)]
        public static void HandleGuildSetRankPerm(Packet packet)
        {
            packet.ReadInt32("OldRankID"); // 92
            for (var tabId = 0; tabId < 8; ++tabId)
            {
                packet.ReadInt32("BankRights", tabId); // 128
                packet.ReadInt32("Slots", tabId); // 136
            }
            packet.ReadInt32("MoneyPerDay"); // 96
            packet.ReadInt32("NewRights"); // 20
            packet.ReadInt32("NewRankId"); // 100
            packet.ReadInt32("OldRights"); // 16

            var len = packet.ReadBits("Len", 7);
            packet.ReadWoWString("Name", len);
        }
    }
}
