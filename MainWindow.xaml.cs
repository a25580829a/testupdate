using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        string shapeType = "Line";
        Point start, dest;
        Color StrokeColor = Colors.Red, StrokeFillColor = Colors.Orange;
        Brush CurrentStrokeBrush, CurrentStrokeBrushFill;
        //Brush CurrentStrokeBrush = new SolidColorBrush(Colors.Red);
        int strokeThickness = 3;
        public MainWindow()
        {
            InitializeComponent();
            StrokeColorPicker.SelectedColor = StrokeColor;
            StrokeFillColorPicker.SelectedColor = StrokeFillColor;
        }


        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            myCanvas.Cursor = Cursors.Cross;
            start = e.GetPosition(myCanvas);

            DisplayStatus();

            positionLabel.Content = $"座標點({start.X}),({start.Y}) - ({dest.X}),({dest.Y})";
            
            switch (shapeType)
            {
                case "Line":
                    DrawLine(Brushes.Gray, 1);
                    break;
                case "Rectangle":
                    DrawRectangle(Brushes.Gray,Brushes.LightGray, 1);
                    break;
                case "Ellipse":
                    DrawEllipse(Brushes.Gray,Brushes.LightGray, 1);
                    break;
            }
           
        }

        private void DisplayStatus()
        {
            int linecount = myCanvas.Children.OfType<Line>().Count();
            int Rectanglecount = myCanvas.Children.OfType<Rectangle>().Count();
            int Ellipsecount = myCanvas.Children.OfType<Ellipse>().Count();
            int Polylinecount = myCanvas.Children.OfType<Polyline>().Count();

            positionLabel.Content = $"座標點({start.X}),({start.Y}) - ({dest.X}),({dest.Y})";

            shapeLabel.Content = $"Line : {linecount}, Rectangle : {Rectanglecount}, Ellipse : {Ellipsecount}, Polyline : {Polylinecount}";
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            dest = e.GetPosition(myCanvas);
            DisplayStatus();

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                switch (shapeType)
                {
                    case "Line":
                        var line = myCanvas.Children.OfType<Line>().LastOrDefault();
                        line.X2 = dest.X;
                        line.Y2 = dest.Y;
                        break;
                    case "Rectangle":
                        var rect = myCanvas.Children.OfType<Rectangle>().LastOrDefault();
                        UpdateShape(rect);
                        break;
                    case "Ellipse":
                        var ellipse = myCanvas.Children.OfType<Ellipse>().LastOrDefault();
                        UpdateShape(ellipse);
                        break;
                }
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            switch (shapeType)
            {
                case "Line":
                    var line = myCanvas.Children.OfType<Line>().LastOrDefault();
                    UpdateShape(line, CurrentStrokeBrush, strokeThickness);
                    //line.X2 = dest.X;
                    //line.Y2 = dest.Y;
                    break;
                case "Rectangle":
                    var rect = myCanvas.Children.OfType<Rectangle>().LastOrDefault();
                    UpdateShape(rect, CurrentStrokeBrush,CurrentStrokeBrushFill, strokeThickness);
                    break;
                case "Ellipse":
                    var ellipse = myCanvas.Children.OfType<Ellipse>().LastOrDefault();
                    UpdateShape(ellipse, CurrentStrokeBrush,CurrentStrokeBrushFill, strokeThickness);
                    break;
                case "Polyline":
                    break;
            }
            myCanvas.Cursor = Cursors.Arrow;
        }

        private void DrawLine(Brush stroke, int thickness)
        {
            Line Line = new Line()
            {
                Stroke = stroke,
                X1 = start.X,
                Y1 = start.Y,
                X2 = dest.X,
                Y2 = dest.Y,
                StrokeThickness = thickness
            };
            myCanvas.Children.Add(Line);
        }

        private void DrawEllipse(SolidColorBrush stroke,Brush fill, int thickness)
        {
            Ellipse ellipse = new Ellipse()
            {
                Stroke = stroke,
                StrokeThickness = thickness,
                Fill = fill
            };
            UpdateShape(ellipse);

            myCanvas.Children.Add(ellipse);
        }

        private void DrawRectangle(SolidColorBrush stroke,Brush fill, int thickness)
        {
            Rectangle rect = new Rectangle()
            {
                Stroke = stroke,
                StrokeThickness = thickness,
                Fill = fill
            };
            UpdateShape(rect);

            myCanvas.Children.Add(rect);
        }

        private void RadioButtonClick(object sender, RoutedEventArgs e)
        {
            RadioButton rb1 = sender as RadioButton;
            shapeType = rb1.Content.ToString();
        }

        private void ThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            strokeThickness = (int)e.NewValue;
        }

        private void StrokeColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            StrokeColor = (Color)e.NewValue;
            CurrentStrokeBrush = new SolidColorBrush(StrokeColor);
        }

        private void StrokeFillColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            StrokeFillColor = (Color)e.NewValue;
            CurrentStrokeBrushFill = new SolidColorBrush(StrokeFillColor);
        }

        private void UpdateShape(Shape shape, Brush stroke,Brush fill, int thickness)
        {
            shape.Stroke = stroke;
            shape.Fill = fill;
            shape.StrokeThickness = thickness;
        }

        private void UpdateShape(Shape shape, Brush stroke, int thickness)
        {
            shape.Stroke = stroke;
            shape.StrokeThickness = thickness;
        }

        private void UpdateShape(Shape shape)
        {
            Point origin = new Point();

            origin.X = Math.Min(start.X, dest.X);
            origin.Y = Math.Min(start.Y, dest.Y);
            double width = Math.Abs(dest.X - start.X);
            double height = Math.Abs(dest.Y - start.Y);

            shape.Width = width;
            shape.Height = height;
            shape.SetValue(Canvas.LeftProperty, origin.X);
            shape.SetValue(Canvas.TopProperty, origin.Y);
        }


    }
}
