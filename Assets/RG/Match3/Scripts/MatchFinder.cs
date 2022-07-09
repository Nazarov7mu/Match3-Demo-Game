using System.Collections.Generic;
using UnityEngine;

namespace RG.Match3.Scripts {
    public class MatchFinder : MonoBehaviour {
        [Header("Scripts")]
        [SerializeField] private GameData _gameData;
        
        /*
         We could use a simple List to store all matched tiles,
         but if we want to extend the functionality to, for example, finding vertical matches - 
         HashSet comes in handy, as it doesn't allow storing duplicate values. 
         */
        private HashSet<(int row, int column)> _allMatches;
        
        /*
         Overall Algorithm:
            Go through each row and column:
                If 3 or more tiles of the same shape appears in one row
                    Add this combination to _allMatches
                If combination is interrupted 
                    Clear the combination
                    
            Then Destroy each tile in _allMatches.
            
         ----
         This Algorithm can easily be extended to find not only horizontal but vertical matches. 
          
         */
        public void SearchForMatches() {
            TileShape? previous = null; // Same as Nullable<TileShape> (https://www.geeksforgeeks.org/c-sharp-nullable-types/)
            
            _allMatches = new HashSet<(int row, int column)>();
            List<(int row, int column)> currentMatch = new List<(int row, int column)>();
        
            for (int row = 0; row < _gameData.NumOfRows; row++) {
                for (int column = 0; column < _gameData.NumOfColumns; column++) {
                    if (_gameData.Grid[row][column].tile == null) {
                        previous = null;
                        CheckForMatch();
                        continue;
                    }

                    if (previous == _gameData.Grid[row][column].tile.TileShape) {
                        currentMatch.Add((row, column));
                    }
                    else {
                        CheckForMatch();
                        currentMatch.Add((row, column));
                    }

                    previous = _gameData.Grid[row][column].tile.TileShape;
                }
                // It is the end of the row
                previous = null;
                CheckForMatch();
            }
        
            // Local function (https://www.geeksforgeeks.org/local-function-in-c-sharp/)
            void CheckForMatch() {
                if (currentMatch.Count >= GameData.MatchSize) {
                    for (int i = 0; i < currentMatch.Count; i++) {
                        _allMatches.Add(currentMatch[i]);
                    }
                }
            
                currentMatch.Clear();
            }

            if (_allMatches.Count > 0) {
                DestroyMatches();
            }
        }


        private void DestroyMatches() {
            foreach ((int row, int column) in _allMatches) {
                Destroy(_gameData.Grid[row][column].tile.gameObject);
                _gameData.Grid[row][column].tile = null;
            }

            GridUpdater._instance.UpdateGrid(); // Update the Grid after destroying all matched Tiles
        }
    }
}