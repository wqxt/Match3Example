using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Match3
{
    public class TileAnimationController : MonoBehaviour
    {
        [SerializeField] private float _dropTileAnimationDuration = 0.2f;
        [SerializeField] private float _spawnTileAnimationDuration = 0.5f;
        [SerializeField] private float _deleteTileAnimationDuration = 0.03f;
        public IEnumerator PlaySpawnTileAnimation(Tile tile, Transform targetTransform)
        {
            tile.transform.localScale = new Vector3(0, 0);
            tile.transform.position = new Vector3(targetTransform.position.x, 6);

            tile.transform
            .DOMove(targetTransform.position, _spawnTileAnimationDuration)
            .SetEase(Ease.InOutQuad)
            .WaitForCompletion();

            tile.transform
                 .DOScale(0.5f, 0.2f)
                 .SetEase(Ease.InOutQuad);
            yield break;
        }

        public IEnumerator PlayDropTileAnimation(Tile tile, Transform targetTransform)
        {
            tile.transform
                .DOMove(targetTransform.position, _dropTileAnimationDuration)
                .SetEase(Ease.InOutQuad)
                .WaitForCompletion();
            yield break;
        }

        public IEnumerator PlayDeletedTileAnimation(Tile tile)
        {
            yield return tile.transform
            .DOScale(0f, _deleteTileAnimationDuration)
            .SetEase(Ease.Linear)
            .WaitForCompletion();
        }
    }
}