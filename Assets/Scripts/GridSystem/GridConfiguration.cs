using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    [CreateAssetMenu(fileName = "GameConfiguration", menuName = "ScriptableObject/GameConfiguration")]
    public class GridConfiguration : ScriptableObject
    {
        public Tile[] _gameTiles;
        public int _rows;
        public int _columns;
    }
}
