using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BackOffice.Models;
using BackOffice.Models.DTOs.Other;
using BackOffice.Services;
using BackOffice.ViewModels.Other;
using Mapsui;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Providers;
using Mapsui.Styles;
using Mapsui.Tiling;
using Mapsui.UI.Wpf;
using static SkiaSharp.HarfBuzz.SKShaper;
using Brush = Mapsui.Styles.Brush;
using Color = Mapsui.Styles.Color;
using Map = Mapsui.Map;

namespace BackOffice.Views.Other
{
    /// <summary>
    /// Interaction logic for MapView.xaml
    /// </summary>
    public partial class MapView : UserControl
    {
        private Map map;
        private MemoryLayer pointLayer = new MemoryLayer();
        private ObservableCollection<LocationDto> locations = new();
        private Popup _currentPopup;
        public MapView()
        {
            // Create a new map
            map = new Map();
            InitializeComponent();
            InitializeMap();
            MapLocationsAsync();
        }

        private async Task MapLocationsAsync()
        {
            try
            {
                var _api = new ApiClient();
                var results = await _api.GetAsync<PaginatedResult<LocationDto>>($"Locations?page=1&pageSize=100");
                locations.Clear();
                foreach (var result in results.Items)
                {
                    locations.Add(result);
                }

                if (locations != null)
                {
                    var features = new List<IFeature>();

                    foreach (var location in locations)
                    {
                        // Convert coordinates to spherical mercator
                        var coordinates = SphericalMercator.FromLonLat(location.GpsLongitude, location.GpsLatitude);
                        var point = new MPoint(coordinates.x, coordinates.y);

                        // Create feature with properties
                        var feature = new PointFeature(point)
                        {
                            ["LocationId"] = location.LocationId,
                            ["VehicleId"] = location.VehicleId,
                            ["RentalPlaceId"] = location.RentalPlaceId,
                            ["DateTime"] = location.DateTime,
                            ["Description"] = CreateLocationDescription(location)
                        };

                        // Set different style based on VehicleId
                        var style = new SymbolStyle
                        {
                            Fill = new Brush(location.VehicleId.HasValue ? Color.Blue : Color.Red),
                            SymbolScale = 0.5,
                            SymbolOffset = new Offset(0, 0)
                        };

                        feature.Styles.Add(style);
                        features.Add(feature);
                    }

                    // Update layer - remove the layer style since each feature has its own
                    pointLayer.Style = null;
                    pointLayer.Features = features;

                    // Center map
                    if (features.Any())
                    {
                        var firstFeature = features.First() as PointFeature;
                        map.Navigator.CenterOn(firstFeature?.Point);
                        map.Navigator.ZoomTo(5000);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading locations: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string CreateLocationDescription(LocationDto location)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Date: {location.DateTime}");

            if (location.VehicleId.HasValue)
                sb.AppendLine($"Vehicle ID: {location.VehicleId}");

            if (location.RentalPlaceId.HasValue)
                sb.AppendLine($"Rental Place ID: {location.RentalPlaceId}");

            sb.AppendLine($"Coordinates: {location.GpsLatitude:F6}, {location.GpsLongitude:F6}");

            return sb.ToString();
        }

        private void InitializeMap()
        {
            // Add OpenStreetMap as the base layer
            var tileLayer = OpenStreetMap.CreateTileLayer();
            map.Layers.Add(tileLayer);

            // Set the map to the MapControl
            MapControl.Map = map;

            var coordinates = SphericalMercator.FromLonLat(20.69228635550254, 49.623966334846685);
            var center = new MPoint(coordinates.x, coordinates.y);

            map.Navigator.CenterOn(center);
            map.Navigator.ZoomTo(5); // Use a positive zoom level

            var feature = new PointFeature(center)
            {
                ["Title"] = "My Location",
                ["Description"] = "This is a sample description"
            };

            var style = new SymbolStyle
            {
                Fill = new Brush(Color.Red),
                SymbolScale = 0.5,
                SymbolOffset = new Offset(0, 0)
            };

            //try
            //{
            //    style.BitmapId = BitmapRegistry.Instance.Register("pack://application:,,,/BackOffice;component/Images/MapMarker.png");
            //    style.SymbolScale = 5; // Adjust based on your image size
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Failed to load marker image: {ex.Message}");
            //}

            //feature.Styles.Add(style);

            pointLayer = new MemoryLayer
            {
                Name = "Points Layer",
                Features = new [] { feature },
                Style = style,
                IsMapInfoLayer = true  // This is important for click handling
            };

            map.Layers.Add(pointLayer);

            MapControl.Info += MapControlOnInfo;
        }

        private void MapControlOnInfo(object sender, MapInfoEventArgs e)
        {
            if (e.MapInfo?.Feature != null)
            {
                // Close the current popup if it is open
                if (_currentPopup != null && _currentPopup.IsOpen)
                {
                    _currentPopup.IsOpen = false;
                }

                var title = e.MapInfo.Feature["Title"]?.ToString() ?? "Location";
                var description = e.MapInfo.Feature["Description"]?.ToString() ?? "No description available";

                var clickPosition = e.MapInfo.ScreenPosition;

                var popup = new Popup
                {
                    Child = new Border
                    {
                        Background = new SolidColorBrush(Colors.White),
                        BorderBrush = new SolidColorBrush(Colors.Black),
                        BorderThickness = new Thickness(1),
                        Padding = new Thickness(10),
                        Child = new Grid
                        {
                            Children =
                    {
                        new StackPanel
                        {
                            Children =
                            {
                                // Header with title and close button
                                new Grid
                                {
                                    Margin = new Thickness(0, 0, 0, 10),
                                    Children =
                                    {
                                        new TextBlock
                                        {
                                            Text = title,
                                            FontWeight = FontWeights.Bold,
                                            VerticalAlignment = VerticalAlignment.Center
                                        },
                                        new Button
                                        {
                                            Content = "×",
                                            FontSize = 16,
                                            Padding = new Thickness(5, 0, 5, 0),
                                            HorizontalAlignment = HorizontalAlignment.Right,
                                            Background = new SolidColorBrush(Colors.Transparent),
                                            BorderThickness = new Thickness(0)
                                        }
                                    }
                                },
                                new TextBlock
                                {
                                    Text = description,
                                    Margin = new Thickness(0, 0, 0, 5)
                                }
                            }
                        }
                    }
                        }
                    },
                    Placement = PlacementMode.Absolute,
                    AllowsTransparency = true,
                    StaysOpen = true
                };

                // Get the close button and add click handler
                var closeButton = ((popup.Child as Border)?.Child as Grid)?.Children.OfType<StackPanel>().FirstOrDefault()?
                    .Children.OfType<Grid>().FirstOrDefault()?
                    .Children.OfType<Button>().FirstOrDefault();

                if (closeButton != null)
                {
                    closeButton.Click += (s, args) => popup.IsOpen = false;
                }

                // Position the popup at the click location
                popup.HorizontalOffset = clickPosition.X;
                popup.VerticalOffset = clickPosition.Y;

                // Set the current popup to the new popup
                _currentPopup = popup;

                // Open the new popup
                popup.IsOpen = true;
            }
        }
    }
}
