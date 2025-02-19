using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Mapsui;
using Mapsui.Projections;
using Mapsui.Tiling;
using Mapsui.UI.Wpf;

namespace BackOffice.Views.Other
{
    /// <summary>
    /// Interaction logic for MapView.xaml
    /// </summary>
    public partial class MapView : UserControl
    {
        public MapView()
        {
            InitializeComponent();
            InitializeMap();
        }

        private void InitializeMap()
        {
            // Create a new map
            var map = new Map();

            // Add OpenStreetMap as the base layer
            var tileLayer = OpenStreetMap.CreateTileLayer();
            map.Layers.Add(tileLayer);

            // Set the map to the MapControl
            MapControl.Map = map;

            var center = new MPoint(19.21, 52.11);
            map.Navigator.CenterOn(center, -1);
            map.Navigator.ZoomIn(-1);
        }
    }
}
