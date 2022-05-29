public enum PieceType
{
    EMPTY,
    NORMAL,
    BUBBLE,
    ROW_CLEAR,
    COLUMN_CLEAR,
    RAINBOW,
    COUNT,
    NULL,
    //will be ignored when count piece amount will be spawned or move down 
    //does not effect when check another piece move diagnal down
    //will be ignored when check move down
    IGNORE,
    BOMB
}
