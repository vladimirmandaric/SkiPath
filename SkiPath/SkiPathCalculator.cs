using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkiPath
{

    public class SkiPathCalculator
    {
        private int[,] _map;
        private bool[,] _shouldCheck;

        private int SizeX, SizeY;

        /// <summary>
        /// pointing to the lowest node of the lognest path
        /// </summary>
        private SkiTree Max = null;

        /// <summary>
        /// MaxDepth up till now
        /// </summary>
        private int MaxDepth = 0;

        /// <summary>
        /// Max drop for max depth until now
        /// </summary>
        private int MaxDrop = 0;

        /// <summary>
        /// First int is current point value, second is next point value in path
        /// </summary>
        private Func<int, int, bool> _isOnPathCondition;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="map">map</param>
        /// <param name="sizeX">size X</param>
        /// <param name="sizeY">size Y</param>
        /// <param name="isOnPathCondition">First int is current point value, second is next point value in path</param>
        public SkiPathCalculator(int[,] map, Func<int, int,bool> isOnPathCondition)
        {
            _map = map;
            SizeX = map.GetLength(0);
            SizeY = map.GetLength(1);
            _isOnPathCondition = isOnPathCondition;
        }

        /// <summary>
        /// Find next point down from nearest environment
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private IEnumerable<Point> Find(Point p)
        {
            var l = new List<Point>();
            foreach (Point pp in Environment(p, SizeX, SizeY))
                if (_isOnPathCondition(_map[p.X, p.Y], _map[pp.X, pp.Y]))
                    l.Add(pp);
            return l;
        }

        /// <summary>
        /// Make tree with all paths down from current point
        /// </summary>
        /// <param name="p"></param>
        /// <param name="parent"></param>
        /// <param name="highestValue"></param>
        /// <returns></returns>
        private SkiTree GetGraphForPoint(Point p, SkiTree parent = null, int? highestValue=null)
        {
            // little heuristics
            // if some point is not on the begining of the path, it should not be checked later as a starting point
            if (parent == null && !_shouldCheck[p.X, p.Y])
                return null;
            if (parent != null)
                _shouldCheck[p.X, p.Y] = false;
            

            var nexts = new List<SkiTree>();
            var g = new SkiTree
            {
                Position = p,
                Value = _map[p.X, p.Y],
                Parent = parent,
                Depth = parent==null ? 1 : parent.Depth+1,
                HighestValue = highestValue==null ? _map[p.X, p.Y] : highestValue.Value
            };
            foreach (var pp in Find(p))
                nexts.Add(GetGraphForPoint(pp,g, g.HighestValue));
            if (nexts.Count() != 0)
                g.Nexts = nexts;
            if ((g.Depth == MaxDepth && g.Drop() > MaxDrop) || (g.Depth > MaxDepth))
            {
                MaxDepth = g.Depth;
                MaxDrop = g.Drop();
                Max = g;
            }                        
            return g;
        }



        public SkiTree GetMax()
        {
            _shouldCheck = new bool[SizeX, SizeY];
            for (var i = 0; i < SizeX; i++)
                for (var j = 0; j < SizeY; j++)
                    _shouldCheck[i, j] = true;
            MaxDepth = -1;

            for (var i = 0; i < SizeX; i++)
                for (var j = 0; j < SizeY; j++)
                    GetGraphForPoint(new Point { X = i, Y = j });
            return Max;
        }

        /// <summary>
        /// iterator for points environment
        /// </summary>
        /// <param name="p"></param>
        /// <param name="wx"></param>
        /// <param name="wy"></param>
        /// <returns></returns>
        public static IEnumerable<Point> Environment(Point p, int wx, int wy)
        {
            if (p.X>0)
                yield return new Point { X = p.X - 1, Y = p.Y };
            if (p.X < wx-1)
                yield return new Point { X = p.X + 1, Y = p.Y, };
            if (p.Y > 0)
                yield return new Point { X = p.X, Y = p.Y - 1 };
            if (p.Y < wy-1)
                yield return new Point { X = p.X, Y = p.Y + 1 };
        }

    }
}
