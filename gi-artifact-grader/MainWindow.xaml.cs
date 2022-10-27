using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace GIArtifactGrader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow? MWindow;

        List<Artifact>? _artifacts = new();
        List<ArtifactGrade> _artifactGrades = new();
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "JSON Files (*.json)|*.json|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
                JsonPath.Text = System.IO.Path.GetFullPath(openFileDialog.FileName);
        }

        private void LoadArtifactsButton(object sender, RoutedEventArgs e)
        {
            // print info
            LoadBar.Value = 0;
            LoadBarInfo.Content = "Loading artifacts...";

            ArtifactInfoBox.Text = String.Empty;
            ArtifactInfoBox.Text = "=======================================";
            ArtifactInfoBox.Text += "\nChecking artifacts...";


            // load artifact info from path
            _artifacts = ArtifactLoader.LoadArtifacts(JsonPath.Text);

            if (_artifacts == null)
                return;

            // print info
            ArtifactInfoBox.Text += "\nDone!";
            ArtifactInfoBox.Text += "\n=======================================\n";
            ProgressBarSmoother.SetSmoothValue(LoadBar, 100);
            LoadBarInfo.Content = "Done!";
            ArtifactInfoBox.ScrollToEnd();
        }

        private void CheckArtifactsButtonClick(object sender, RoutedEventArgs e)
        {
            GradeArtifacts();
        }

        private void GradeArtifacts()
        {
            LoadBar.Value = 0;
            ArtifactInfoBox.Text = String.Empty;
            ArtifactInfoBox.Text = "=======================================";
            ArtifactInfoBox.Text += "\nChecking artifacts...";

            // grade artifacts
            _artifactGrades = ArtifactGrade.GradeForGeneralCV(_artifacts);
            _artifactGrades = ArtifactGrade.GradeForYelan(_artifactGrades);
            _artifactGrades = ArtifactGrade.GradeForNilou(_artifactGrades);

            if (_artifactGrades == null)
                return;

            ProgressBarSmoother.SetSmoothValue(LoadBar, 25);

            // load artifact info to data grid
            LoadGradedArtifacts();

            ProgressBarSmoother.SetSmoothValue(LoadBar, 100);

            ArtifactInfoBox.Text += "\nDone!";
            ArtifactInfoBox.Text += "\n=======================================";
            ArtifactInfoBox.ScrollToEnd();
        }

        private void LoadGradedArtifacts()
        {
            GradedArtifactsGrid.ItemsSource = _artifactGrades;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            // get example_json.json path and set it
            string _path = System.IO.Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "example_json.json");
            JsonPath.Text = _path;
        }
    }
}
