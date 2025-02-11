using System;
using UnityEngine;


namespace Match3
{
    public class InputController : MonoBehaviour
    {
        [SerializeField] private GameInputReaderSO _input;
        [SerializeField] private RaycastHit2D _hit;
        [SerializeField] private Ray _ray;
        [SerializeField] private Vector3 _clickTransform;
        [SerializeField] private Camera _camera;

        public event Action<Cell> ClickObject;

        private void OnEnable()
        {
            _input.ClickEvent += CheckClick;
            _input.TouchEvent += CheckClick;
        }

        private void OnDisable()
        {
            _input.ClickEvent -= CheckClick;
            _input.TouchEvent -= CheckClick;
        }

        private void Awake()
        {
            _input.Initialization();
        }

        private void CheckCamera()
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        }

        private void CheckClick()
        {
            CheckCamera();

            try 
            {
                if (_hit.collider.gameObject.TryGetComponent(out Cell cell))
                {
                    Debug.Log($"Click on the cell, pos = {cell.transform.position}");
                    ClickObject?.Invoke(cell);
                }
                else
                {
                    Debug.Log($"Click outside the field ");
                    return;
                }
            }
            catch
            {
                Debug.Log($"Click outside the field ");
                return ;
            }



        }
    }
}