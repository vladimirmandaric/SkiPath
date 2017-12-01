using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkiPath
{
    /// <summary>
    /// Holds the info about all paths down from particular point. Tree structure
    /// </summary>
    public class SkiTree
    {
        /// <summary>
        /// My position
        /// </summary>
        public Point Position { get; set; }

        /// <summary>
        /// My elevation
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// From the root to me
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        /// Next steps down from me
        /// </summary>
        public IEnumerable<SkiTree> Nexts { get; set; }

        public SkiTree Parent { get; set; }

        public int HighestValue { get; set; }

        public int Drop()
        {
            return HighestValue - Value;
        }
    }
}
