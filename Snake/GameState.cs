using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Snake
{
    public class GameState
    {
        public int Rows { get; }
        public int Cols { get; }

        public GridValue[,] Grid { get; }
        public Direction Dir { get; private set; }

        public int Score { get; private set; }
        public bool GameOver { get; private set; }

        // where is the snake right now
        // First element -> head of the snake
        // Last element -> tail of the snake
        private readonly LinkedList<Position> _snakePositions = new LinkedList<Position>();
        private LinkedList<Direction> _dirChangeBuffer = new LinkedList<Direction>();
        private readonly Random _random = new Random();

        public GameState(int rows, int cols) {
            Rows = rows;
            Cols = cols;
            Grid = new GridValue[rows, cols];

            Dir = Direction.Right;

            AddSnake();
            AddFood();

        }

        // Add snake initially in the grid
        private void AddSnake() {
            int r = Rows / 2;

            for (int c = 1; c <= 3; c++)
            {
                Grid[r, c] = GridValue.Snake;
                _snakePositions.AddFirst(new Position(r, c));
            }

        }


        // this method will return all empty grid positions
        private IEnumerable<Position> EmptyPositions() {
            for (int r = 0; r < Rows; r++) {
                for (int c = 0; c < Cols; c++) {
                    if (Grid[r, c] == GridValue.Empty) {
                        yield return new Position(r, c);
                    }
                }
            }
        }


        private void AddFood() {
            List<Position> empty = new List<Position> (EmptyPositions());

            // if there is no empty position
            if (empty.Count == 0) {
                return;
            }

            // pick a random empty position
            Position pos = empty[_random.Next(empty.Count)];

            Grid[pos.Row, pos.Col] = GridValue.Food;

        }

        public Position HeadPosition()
        {
            return _snakePositions.First.Value;
        }

        public Position TailPosition()
        {
            return _snakePositions.Last.Value;
        }

        // this will return all the snake positions as the IEnumerable
        public IEnumerable<Position> SnakePositions()
        {
            return _snakePositions;
        }

        private void AddHead(Position pos) {
            _snakePositions.AddFirst(pos);

            //set the corresponding position to the grid array
            Grid[pos.Row, pos.Col] = GridValue.Snake;
        }

        private void RemoveTail() {
            Position tail = _snakePositions.Last.Value;
            Grid[tail.Row, tail.Col] = GridValue.Empty;
            _snakePositions.RemoveLast();
        }

        private Direction GetLastDirection()
        {
            // if buffer is empty, then consider the current direction
            if (_dirChangeBuffer.Count == 0)
            {
                return Dir;
            }

            return _dirChangeBuffer.Last.Value;
        }

        private bool CanChangeDirection(Direction newDir)
        {
            // This is the max size of the buffer
            if (_dirChangeBuffer.Count == 2)
            {
                return false;
            }

            Direction lastDir = GetLastDirection();
            return lastDir != newDir && lastDir != newDir.Opposite();
        }

        public void ChangeDirection(Direction dir) {

            if (CanChangeDirection(dir))
            {
                _dirChangeBuffer.AddLast(dir);
            }
        }

        // will check whether outside of the grid or not
        private bool OutsideGrid(Position pos) { 
            return pos.Row < 0 || pos.Row >= Rows || pos.Col < 0 || pos.Col >= Cols;
        }

        private GridValue WillHit(Position newHeadPos) {
            if (OutsideGrid(newHeadPos)) {
                return GridValue.Outside;
            }

            // Case: The head is about to take the next block at which
            // the tail is there, when the snake move the head and will take
            // the position of the tail.
            if (newHeadPos == TailPosition()) {
                return GridValue.Empty;
            }

            return Grid[newHeadPos.Row, newHeadPos.Col];
        }

        // move the snake
        public void Move() {

            if (_dirChangeBuffer.Count > 0)
            {
                Dir = _dirChangeBuffer.First.Value;
                _dirChangeBuffer.RemoveFirst();
            }

            Position newHeadPos = HeadPosition().Translate(Dir);
            GridValue hit = WillHit(newHeadPos);

            // if either moves outside of the grid or hits itself
            if (hit == GridValue.Outside || hit == GridValue.Snake)
            {
                GameOver = true;
            }
            // if moves to empty position: to move the snake one step forward
            // remove its tail and add a new head
            else if (hit == GridValue.Empty)
            {
                RemoveTail();
                AddHead(newHeadPos);
            }
            // if moved to food: to increase the size of the snake
            // simply add the new head without removing its tail
            else { 
                AddHead(newHeadPos);
                Score++;
                AddFood();
            }


        }
    }
}