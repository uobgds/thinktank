using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty
{
    Tutorial,
    Easy,
    Medium,
    Hard
}

public static class GameSettings {
    public static Difficulty difficulty = Difficulty.Tutorial;
}
