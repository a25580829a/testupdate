using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        string shapeType = "Line";
        string actionType = "Draw";
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

            if (actionType == "Draw")
            {
                myCanvas.Cursor = Cursors.Cross;
                switch (shapeType)
                {
                    case "Line":
                        DrawLine(Brushes.Gray, 1);
                        break;
                    case "Rectangle":
                        DrawRectangle(Brushes.Gray, Brushes.LightGray, 1);
                        break;
                    case "Ellipse":
                        DrawEllipse(Brushes.Gray, Brushes.LightGray, 1);
                        break;
                    case "Polyline":
                        DrawPolyline(Brushes.Gray, Brushes.LightGray, 1);
                        break;
                }


            }

            positionLabel.Content = $"座標點({start.X}),({start.Y}) - ({dest.X}),({dest.Y})";

 

            DisplayStatus();
        }

        private void DrawPolyline(Brush stroke, Brush fill, int thickness)
        {
            PointCollection pointcollection = new PointCollection();
            pointcollection.Add(start);
            Polyline polyline = new Polyline()
            {
                Stroke = stroke,
                StrokeThickness = thickness,
                Fill = fill,
                Points = pointcollection
            };
            myCanvas.Children.Add(polyline);

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
           
            if (actionType == "Erase")
            {
                var eraseShape = e.OriginalSource as Shape;
                myCanvas.Children.Remove(eraseShape);

                if (myCanvas.Children.Count == 0)
                {
                    myCanvas.Cursor = Cursors.Arrow;
                    actionType = "Draw";
                }
            }

            else if (e.LeftButton == MouseButtonState.Pressed)
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
                    case "Polyline":
                        var polyline = myCanvas.Children.OfType<Polyline>().LastOrDefault();
                        var pointCollection = polyline.Points;
                        pointCollection.Add(dest);
                        break;
                }
            }
            DisplayStatus();
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (actionType == "Draw")
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
                        UpdateShape(rect, CurrentStrokeBrush, CurrentStrokeBrushFill, strokeThickness);
                        break;
                    case "Ellipse":
                        var ellipse = myCanvas.Children.OfType<Ellipse>().LastOrDefault();
                        UpdateShape(ellipse, CurrentStrokeBrush, CurrentStrokeBrushFill, strokeThickness);
                        break;
                    case "Polyline":
                        var polyline = myCanvas.Children.OfType<Polyline>().LastOrDefault();
                        UpdateShape(polyline, CurrentStrokeBrush, CurrentStrokeBrushFill, strokeThickness);
                        break;
                }
                myCanvas.Cursor = Cursors.Arrow;
            }
            DisplayStatus();

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

        private void DrawEllipse(SolidColorBrush stroke, Brush fill, int thickness)
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

        private void DrawRectangle(SolidColorBrush stroke, Brush fill, int thickness)
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
            shapeType = rb1.Tag.ToString();
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

        private void EraserButton_Click(object sender, RoutedEventArgs e)
        {
            actionType = "Erase";
            myCanvas.Cursor = Cursors.Hand;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            myCanvas.Children.Clear();
            actionType = "Draw";
        }

        private void ShapeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            shapeType = menuItem.Header.ToString();
        }

        private void SaveItem_Click(object sender, RoutedEventArgs e)
        {
            int w = Convert.ToInt32(myCanvas.RenderSize.Width);
            int h = Convert.ToInt32(myCanvas.RenderSize.Height);

            RenderTargetBitmap rtb = new RenderTargetBitmap(w, h, 64d, 64d, PixelFormats.Default);
            rtb.Render(myCanvas);

            PngBitmapEncoder png = new PngBitmapEncoder();
            png.Frames.Add(BitmapFrame.Create(rtb));
            JpegBitmapEncoder jpeg = new JpegBitmapEncoder();
            jpeg.Frames.Add(BitmapFrame.Create(rtb));

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "儲存畫布";
            saveFileDialog.DefaultExt = ".png";
            saveFileDialog.Filter = "png檔案|*.png|所有檔案|*,*|.JPG|*.jpg";

            if (saveFileDialog.ShowDialog() == true)
            {
                string path = saveFileDialog.FileName;
                using (var fs = File.Create(path))
                {
                    if (saveFileDialog.FilterIndex == 1) png.Save(fs);
                    else if (saveFileDialog.FilterIndex == 3) jpeg.Save(fs);
                }
            }
        }

        private void displaytoolbar_Click(object sender, RoutedEventArgs e)
        {
            if (displaytoolbar.IsChecked == true)
            {
                MaintoolBarTray.Visibility = Visibility.Collapsed;
            }
            else MaintoolBarTray.Visibility = Visibility.Visible;
        }

        private void UpdateShape(Shape shape, Brush stroke, Brush fill, int thickness)
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
