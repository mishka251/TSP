﻿using System;
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
using TSP.ModelTSP;

namespace TSP
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Grid[] graphs;
        Cities cities;
        public MainWindow()
        {
            InitializeComponent();
            graphs = new Grid[5];
            graphs[0] = graph1;
            graphs[1] = graph2;
            graphs[2] = graph3;
            graphs[3] = graph4;
            graphs[4] = graph5;
            
        }
        public void DrawPoints(object sender, RoutedEventArgs e)
        {
            int nCities;
            if (!Int32.TryParse(textB_countCities.Text, out nCities))
                return;
            cities = new Cities(nCities);
            cities.Generate((int)graph1.Width);
            GeometryGroup cityGroup = new GeometryGroup();
            for (int i = 0; i < cities.NumCities; i++)
            {
                Location l = cities.GetLocation(i);
                // формирование точек на карте
                EllipseGeometry city = new EllipseGeometry();
                city.Center = new Point(l.X, l.Y);
                city.RadiusX = 4;
                city.RadiusY = 4;
                cityGroup.Children.Add(city);
            }

            Path[] myPath = new Path[5];
            for (int i = 0; i < 5; i++)
            {
                myPath[i] = new Path();
                myPath[i].Fill = Brushes.Plum;
                myPath[i].Stroke = Brushes.Black;
                myPath[i].Data = cityGroup;

                graphs[i].Children.Clear();
                graphs[i].Children.Add(myPath[i]);
            }
            button_Calculate.IsEnabled = true;

            /*
                        EllipseGeometry myEllipseGeometry = new EllipseGeometry();
                        myEllipseGeometry.Center = new Point(50, 50);
                        myEllipseGeometry.RadiusX = 50;
                        myEllipseGeometry.RadiusY = 50;
           
                        Path myPath = new Path();
                        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                        myPath.Fill = Brushes.Plum;
                        myPath.Stroke = Brushes.Black;
                        // Create the line geometry to add to the Path
                        LineGeometry myLineGeometry = new LineGeometry();
                        myLineGeometry.StartPoint = new Point(10, 10);
                        myLineGeometry.EndPoint = new Point(50, 30);

                        // Create a rectangle geometry to add to the Path
                        RectangleGeometry myRectGeometry = new RectangleGeometry();
                        myRectGeometry.Rect = new Rect(0, 0, 100, 30);

                        // Add all the geometries to a GeometryGroup.
                        GeometryGroup myGeometryGroup = new GeometryGroup();
                        myGeometryGroup.Children.Add(myLineGeometry);
                        myGeometryGroup.Children.Add(myEllipseGeometry);
                        //myGeometryGroup.Children.Add(myRectGeometry);

                        myPath.Data = myGeometryGroup;
                        graph2.Children.Add(myPath);
                        Path myPath1 = new Path();
                        myPath1.Fill = Brushes.Purple;
                        myPath1.Data = myRectGeometry;
                        graph2.Children.Add(myPath1);
             */
        } // DrawPoints

        public void DrawLines(int[] trail, Grid graph)
        {
            GeometryGroup linesGroup = new GeometryGroup();
            LineGeometry line = new LineGeometry();
            int city = 1;
            for (;city < trail.Length; city++)
            {
                line = new LineGeometry();
                line.StartPoint = new Point(cities.GetLocation(trail[city - 1]).X, cities.GetLocation(trail[city - 1]).Y);
                line.EndPoint = new Point(cities.GetLocation(trail[city]).X, cities.GetLocation(trail[city]).Y);
                linesGroup.Children.Add(line);
            }
            line = new LineGeometry();
            line.StartPoint = new Point(cities.GetLocation(trail[city - 1]).X, cities.GetLocation(trail[city - 1]).Y);
            line.EndPoint = new Point(cities.GetLocation(trail[0]).X, cities.GetLocation(trail[0]).Y);
            linesGroup.Children.Add(line);

            Path myPath = new Path();
            myPath.Stroke = Brushes.Black;
            myPath.Data = linesGroup;
            if(graph.Children.Count > 1)
                graph.Children.Remove(graph.Children[1]);
            graph.Children.Add(myPath);
        }
        public void DrawLines(Location[] trail, Grid graph)
        {
            GeometryGroup linesGroup = new GeometryGroup();
            LineGeometry line = new LineGeometry();        
            int city = 1;
            for (; city < trail.Length; city++)
            {
                line = new LineGeometry();
                line.StartPoint = new Point(trail[city - 1].X, trail[city - 1].Y);
                line.EndPoint = new Point(trail[city].X, trail[city].Y);
                linesGroup.Children.Add(line);
            }
            line = new LineGeometry();
            line.StartPoint = new Point(trail[city - 1].X, trail[city - 1].Y);
            line.EndPoint = new Point(trail[0].X, trail[0].Y);
            linesGroup.Children.Add(line);

            Path myPath = new Path();
            myPath.Stroke = Brushes.Black;
            myPath.Data = linesGroup;
            if (graph.Children.Count > 1)
                graph.Children.Remove(graph.Children[1]);
            graph.Children.Add(myPath);
        }

        public void Calculate(object sender, RoutedEventArgs e)
        {
            CalculateBF();
            CalculateACO();
            CalculateGA();
        }
        public void CalculateBF()
        {

        }
        public void CalculateACO()
        {
            int alpha;
            if (!Int32.TryParse(textB_alpha.Text, out alpha))
            {
                textB_alpha.Background = Brushes.Coral;
                return;
            }
            int beta;
            if (!Int32.TryParse(textB_beta.Text, out beta))
            {
                textB_beta.Background = Brushes.Coral;
                return;
            }
            double rho;
            if (!Double.TryParse(textB_rho.Text, out rho))
            {
                textB_rho.Background = Brushes.Coral;
                return;
            }
            double Q;
            if (!Double.TryParse(textB_Q.Text, out Q))
            {
                textB_Q.Background = Brushes.Coral;
                return;
            }
            int numAnts;
            if (!Int32.TryParse(textB_nAnts.Text, out numAnts))
            {
                textB_nAnts.Background = Brushes.Coral;
                return;
            }
            int maxTime;
            if (!Int32.TryParse(textB_time.Text, out maxTime))
            {
                textB_time.Background = Brushes.Coral;
                return;
            }
            AntColony algorithm = new AntColony(alpha, beta, rho, Q, numAnts, maxTime);
            DrawLines(algorithm.Soulition(cities), graphs[1]);
        }

        public void CalculateGA()
        {
            int numPopulation;
            if (!Int32.TryParse(textB_numPopulate.Text, out numPopulation))
            {
                textB_numPopulate.Background = Brushes.Coral;
                return;
            }
            int maxTime;
            if (!Int32.TryParse(textB_timeGA.Text, out maxTime))
            {
                textB_time.Background = Brushes.Coral;
                return;
            }
            GeneticAlgorithm algorithm = new GeneticAlgorithm(numPopulation, maxTime);
            DrawLines(algorithm.Soulition(cities), graphs[2]);
        }

    } // Class MainWindow
}