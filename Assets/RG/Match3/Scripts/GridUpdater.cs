using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RG.Match3.Scripts
{
    public class GridUpdater : MonoBehaviour
    {
        [Header("Scripts")]
        [SerializeField] private GameData _gameData;
        [SerializeField] private MatchFinder _matchFinder;

        /*
         https://gamedevbeginner.com/singletons-in-unity-the-right-way
         
         Singletons can be dangerous, but in this case the usage is justified in my opinion.
         Because each Container needs to call the UpdateGrid() method, 
         and it's not efficient to link EACH Container to GridUpdater instance.
         
         When the project becomes larger, it is recommended to use a dependency injection framework,
         like Zenject - https://github.com/modesttree/Zenject 
        */
        public static GridUpdater Instance;
        public bool IsMoving { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        /*
         Overall Algorithm:
            Go through each column bottom -> up
                Find the number of empty Containers(skips) in the column
                If there are skips below the Tile:
                    Add the Tile and its initial(start) and expected(end) positions in the tilesToMove list
                    
            Go through each Tile in the tilesToMove list 
                Move the Tile to its new position
         */
        public void UpdateGrid()
        {
            List<(Tile tile, Vector3 start, Vector3 end)> tilesToMove = new List<(Tile, Vector3, Vector3)>();

            for (int column = 0; column < _gameData.NumOfColumns; column++)
            {
                int skips = 0;

                for (int row = _gameData.NumOfRows - 1; row >= 0; row--)
                {
                    if (_gameData.Grid[row][column].tile == null)
                    {
                        skips++;
                        continue;
                    }

                    if (skips > 0)
                    {
                        int newRow = row + skips;

                        tilesToMove.Add((
                            _gameData.Grid[row][column].tile,
                            _gameData.Grid[row][column].transform.position,
                            _gameData.Grid[newRow][column].transform.position));

                        _gameData.Grid[newRow][column].tile = _gameData.Grid[row][column].tile;
                        _gameData.Grid[row][column].tile = null;
                    }
                }
            }

            MoveTiles(tilesToMove);
        }

        private void MoveTiles(List<(Tile tile, Vector3 start, Vector3 end)> tilesToMove)
        {
            for (int i = 0; i < tilesToMove.Count; i++)
            {
                StartCoroutine(MoveObject(
                    tilesToMove[i].tile.transform,
                    tilesToMove[i].start,
                    tilesToMove[i].end,
                    GameData.TileMovementTime));
            }

            StartCoroutine(WaitForMovementEnd(GameData.TileMovementTime)); // Block the clicking
        }


        // Move Transform to Target in X seconds (http://answers.unity.com/answers/1146981/view.html)
        private IEnumerator MoveObject(Transform objectToMove, Vector3 start, Vector3 end, float seconds)
        {
            float elapsedTime = 0;

            while (elapsedTime < seconds)
            {
                objectToMove.position = Vector3.Lerp(start, end, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            objectToMove.position = end;
        }

        // This coroutine is needed to disallow clicks while some Tiles are moving
        private IEnumerator WaitForMovementEnd(float seconds)
        {
            IsMoving = true;
            yield return new WaitForSeconds(seconds * GameData.MovementOffset); // Extra offset as Lerp is not perfectly precise
            IsMoving = false;

            _matchFinder.SearchForMatches(); // After the end of movement, call SearchForMatches()
        }
    }
}