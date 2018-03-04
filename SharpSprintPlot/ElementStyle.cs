using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using SharpSprint.Elements;

namespace SharpSprint.Plot
{
    public class ElementStyle
    {
        public Dictionary<Layer, Color> LayerColors { get; private set; }
        public bool LayerColorOverride { get; set; }
        public Color LayerColor { get; set; }
        public Color StrokeColor { get; set; }
        public decimal StrokeMillimeters { get; set; }
        public Color ViaColor { get; set; }
        public Color ConnectionColor { get; set; }
        public Color DrillColor { get; set; }
        public Color BackgroundColor { get; set; }

        // 15240 Pixels / mm == 600 ppi

        public ElementStyle()
        {
            LayerColors = new Dictionary<Layer, Color>();
            LayerColors.Add(Layer.SilkscreenTop, Color.Crimson);
            LayerColors.Add(Layer.CopperTop, Color.DodgerBlue);
            LayerColors.Add(Layer.CopperInnerTop, Color.Goldenrod);
            LayerColors.Add(Layer.CopperInnerBottom, Color.Gold);
            LayerColors.Add(Layer.CopperBottom, Color.LimeGreen);
            LayerColors.Add(Layer.SilkscreenBottom, Color.DarkKhaki);
            LayerColors.Add(Layer.Mechanical, Color.White);

            LayerColorOverride = false;
            LayerColor = Color.Empty;
            StrokeColor = Color.White;
            StrokeMillimeters = 0.5m;
            ViaColor = Color.Cyan;
            ConnectionColor = Color.LightGray;
            DrillColor = Color.Black;
            BackgroundColor = Color.Transparent;
        }
    }
}
