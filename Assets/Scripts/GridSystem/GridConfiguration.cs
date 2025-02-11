using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    [CreateAssetMenu(fileName = "GameConfiguration", menuName = "ScriptableObject/GameConfiguration")]
    public class GridConfiguration : ScriptableObject
    {

        public int _rows;
        public int _columns;

        public int[,] _gridMask = new int[,]
{
          { 0, 1, 1, 1, 1, 1, 1, 0 },
          { 1, 1, 1, 1, 1, 1, 1, 1 },
          { 1, 1, 1, 1, 1, 1, 1, 1 },
          { 1, 1, 1, 1, 1, 1, 1, 1 },
          { 1, 1, 1, 1, 1, 1, 1, 1 },
          { 1, 1, 1, 1, 1, 1, 1, 1 },
          { 1, 1, 1, 1, 1, 1, 1, 1 },
          { 0, 1, 1, 1, 1, 1, 1, 0 }
};
    }
}
