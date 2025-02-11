using UnityEngine;
using UnityEngine.SceneManagement;

namespace Match3
{
    public class GameFacade : MonoBehaviour
    {
        // нужна передача зависимостей через zenject, добавить явный метод для инициализации 

        public GridController _gridController;
        public TileSwapper _tileSwapper;
        public CheckMatch _checkMatch;
        public TaskController _taskProcessor;


        private void Awake()
        {
            _tileSwapper._cellGrid = _gridController._cellGrid;
        }

        private void Start()
        {
            _gridController.SpawnCellGrid();
            _taskProcessor.AddTask(_gridController.SpawnTiles(_gridController._cellGrid));
            //_checkMatch.CheckMatchesAndProcess(_gridController._cellList);
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


