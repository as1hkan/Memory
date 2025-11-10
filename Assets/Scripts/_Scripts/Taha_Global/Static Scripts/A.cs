using UnityEngine;

/// <summary>
/// this class is used to avoid hard coding
/// </summary>
public static class A
{
    public static class Tags
    {
        public const string player = "Player";
    }
    public static class LayerMasks
    {
        public static LayerMask player = LayerMask.GetMask("player");
    }
    public static class Layers
    {
        public const int player = 8;
    }
    public static class Anim
    {
        // optional naming : trigger => t, Bool => b, int => i, float => f
        public const string t_attack = "attack";
    }
    public static class DataKey
    {
        public const int True = 241;
        public const int False = 456;

        public const string currentLevelIndex = "j325";
        public const string lastUnfinishedLevel = "4da";
        public const string areAdsRemoved = "f32w";
        public const string timersData = "o21ea";
        public const string savedData = "a24e";

        private const string sceneBaseName = "df0";

        public static string GetSceneKey(int iSceneIndex)
        {
            return sceneBaseName + iSceneIndex;
        }
    }
}
