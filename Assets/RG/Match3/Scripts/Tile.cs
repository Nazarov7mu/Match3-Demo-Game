using System.Collections.Generic;
using UnityEngine;

namespace RG.Match3.Scripts
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private GameObject[] _shapes; // to set in the inspector
        public TileShape TileShape { get; private set; }

        // Initialize the Tile with TileShape and Instantiate on the Scene
        public void Init(TileShape tileShape)
        {
            TileShape = tileShape;
            Instantiate(_shapes[(int) tileShape], transform);
        }
    }
}