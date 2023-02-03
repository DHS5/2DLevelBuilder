using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelBuilder2D
{
    public enum LevelBuilderEvent
    {
        CREATE_BUILDER = 0,
        BUILDER_CREATED = 1,
        OPEN_BUILDER = 2,
        QUIT_BUILDER = 3,
        SAVE_LEVEL = 4,
        BEFORE_SAVE = 5,
        DO_ACTION = 6,
        UNDO_ACTION = 7,
        OPEN_HELP = 8,
        QUIT_HELP = 9
    }
}
