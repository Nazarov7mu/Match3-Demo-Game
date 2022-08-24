using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RG.Match3.Scripts
{
    public class GameField : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject _tilePrefab;
        [SerializeField] private GameObject _containerPrefab;

        [Header("Transforms")]
        [SerializeField] private Transform _containersTransform;
        [SerializeField] private Transform _tilesTransform;

        [Header("Scripts")]
        [SerializeField] private GameData _gameData;

        /*
         I am used to prefixing private fields with "_", as recommended by Microsoft 
         (https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions). 
         But overall, I am flexible to change my naming convention style, if the company uses another. 
         I know several programming languages besides C#, so it is not a problem for me.
        */
        private TileShape _prevShape;

        private void Awake()
        {
            // Initializing Jagged Array properly (https://www.programiz.com/csharp-programming/jagged-array)
            _gameData.Grid = new Container[_gameData.NumOfRows][];

            for (int row = 0; row < _gameData.NumOfRows; row++)
            {
                _gameData.Grid[row] = new Container[_gameData.NumOfColumns];
            }

            _prevShape = GetRandomShape();
            CreateGridWithTiles();
        }

        /*
         Algorithm for Creating and Populating the Grid:
            For each row and column
                 Instantiate Container and Tile  
                 Assign Tile to the corresponding Container in the Grid
        */
        private void CreateGridWithTiles()
        {
            for (int row = 0; row < _gameData.NumOfRows; row++)
            {
                for (int column = 0; column < _gameData.NumOfColumns; column++)
                {
                    Container container = Instantiate(_containerPrefab, _containersTransform).GetComponent<Container>();

                    container.transform.position = new Vector3(GameData.StartPosX + column * GameData.HopSize,
                        GameData.StartPosY - row * GameData.HopSize,
                        0);

                    Tile tile = Instantiate(_tilePrefab, _tilesTransform).GetComponent<Tile>();
                    container.tile = tile;
                    tile.transform.position = container.transform.position;

                    tile.Init(GetTileShape());
                    _gameData.Grid[row][column] = container;
                }
            }
        }


        private TileShape GetTileShape()
        {
            TileShape shape = GetRandomShape();

            // Eliminating the possibility of two same shapes in a horizontal row
            while (shape == _prevShape)
                shape = GetRandomShape();

            _prevShape = shape;
            return shape;
        }

        private TileShape GetRandomShape()
        {
            // Select random value from enum (https://answers.unity.com/questions/810638/using-randomrange-to-pick-a-random-value-out-of-an.html)
            Array colors = Enum.GetValues(typeof(TileShape));
            List<TileShape> allShapes = Enumerable.Range(0, colors.Length).Select(x => (TileShape) x).ToList();

            return allShapes[Random.Range(0, allShapes.Count)];
        }
    }
}