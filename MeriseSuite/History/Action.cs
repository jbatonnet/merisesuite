using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeriseSuite
{
    public abstract class Action
    {
        public ModelVisualizer Visualizer { get; protected set; }

        public Action(ModelVisualizer visualizer)
        {
            Visualizer = visualizer;
        }

        public abstract void Execute();
        public abstract void Rollback();
    }
}