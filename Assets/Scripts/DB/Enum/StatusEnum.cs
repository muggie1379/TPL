public enum SceneEnum
{
    // Etc
    TestScene = -99,
    None = -1,

    // Basic
    TitleScene = 0,
    InfoSelectScene,
    StoryScene,

    // Hub
    LobbyScene = 10,
    ArmoryScene,
    ForgeScene,
    ItemShopScene,

    // Dungeon
    DungeonLevelScene = 20,
    DungeonBattleScene,

    // Arena
    ArenaLobbyScene = 30,
    ArenaRankingScene,
    ArenaBattleScene,

    // Minigame
    MinigameMainScene = 50,

    // Tutorial
    TutorialDungeonScene = 90,
    TutorialArenaScene,
}

public enum GameState
{
    Test = -99,

    Paused = -3,
    Loading = -2,
    None = -1,

    Title = 0,
    Sequence,
    Hub,
    InGame,
    Minigame,
}

public enum GameState_Sequence
{
    Story,
    ClassSelect,
    Tutorial,
}

public enum GameState_Hub
{
    Lobby,
    Shop,
    Forge,
    Armory,
}