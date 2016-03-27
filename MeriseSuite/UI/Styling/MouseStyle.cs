using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeriseSuite.Styling
{
    public class MouseStyle : Style
    {
        public MouseStyle(ModelVisualizer visualizer) : base(null, visualizer)
        {
            Width = 0;
            Height = 0;
        }

        public void Update(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
