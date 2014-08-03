namespace WowPacketParserModule.V5_4_8_18291.Enums
{
    public enum ChatMessageType : byte
    {
        Addon = byte.MaxValue,
        System = 0,
        Say = 1,
        Party = 2,
        Raid = 3,
        Guild = 4,
        Officer = 5,
        Yell = 6,
        Whisper = 7,
        Whisper2 = 8,
        WhisperInform = 9,
        Emote = 10,
        TextEmote = 11,
        MonsterSay = 12,
        MonsterParty = 13,
        MonsterYell = 14,
        MonsterWhisper = 15,
        MonsterEmote = 16,
        Channel = 17,
        ChannelJoin = 18,
        ChannelLeave = 19,
        ChannelList = 20,
        ChannelNotice = 21,
        ChannelNoticeUser = 22,
        Afk = 23,
        Dnd = 24,
        Ignored = 25,
        Skill = 26,
        Loot = 27,
        Money = 28,
        Opening = 29,
        Tradeskills = 30,
        PetInfo = 31,
        CombatMiscInfo = 32,
        CombatXpGain = 33,
        CombatHonorGain = 34,
        CombatFactionChange = 35,
        BgSystemNeutral = 36,
        BgSystemAlliance = 37,
        BgSystemHorde = 38,
        RaidLeader = 39,
        RaidWarning = 40,
        RaidBossEmote = 41,
        RaidBossWhisper = 42,
        Filtered = 43,
        Restricted = 44,
        //unused1 = 45,
        Achievement = 46,
        GuildAchievement = 47,
        //unused2 = 48,
        PartyLeader = 49,
        Targeticons = 50,
        BnWhisper = 51,
        BnWhisperInform = 52,
        BnConversation = 53,
        BnConversationNotice = 54,
        BnConversationList = 55,
        BnInlineToastAlert = 56,
        BnInlineToastBroadcast = 57,
        BnInlineToastBroadcastInform = 58,
        BnInlineToastConversation = 59,
        BnWhisperPlayerOffline = 60,
        CombatGuildXpGain = 61,
        Currency = 62,
        QuestBossEmote = 63,
        PetBattleCombatLog = 64,
        PetBattleInfo = 65,
        InstanceChat = 66,
        InstanceChatLeader = 67,
    }
}
