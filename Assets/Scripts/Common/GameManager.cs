using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Match3
{
    public class GameManager : MonoBehaviour
    {
        public GridSpawner _gridSpawner;
        public TileSwapper _tileSwapper;
        public CheckMatch _checkMatch;
        public TaskProcessor _taskProcessor;


        private void Awake()
        {
            _tileSwapper._tileList = _gridSpawner._tileList;
            _tileSwapper._cellList = _gridSpawner._cellList;
        }

        private void Start()
        {
            _gridSpawner.SpawnCellGrid();
            _taskProcessor.AddTask(_gridSpawner.SpawnTiles(_gridSpawner._tileList));
            _checkMatch.CheckMatchesAndProcess(_gridSpawner._tileList);
        }

#if UNITY_EDITOR
        void Update()
        {
            if (Input.GetKey(KeyCode.R))
            {
                SceneManager.LoadScene("Gameplay");
            }
        }
#endif
    }
}
