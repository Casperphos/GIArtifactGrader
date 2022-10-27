using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Windows;

namespace GIArtifactGrader
{
    internal class ArtifactLoader
    {
        private static string _artifactsString = String.Empty;
        private static List<Artifact>? _artifacts = new();

        public static List<Artifact>? LoadArtifacts(string jsonPath)
        {
            try
            {
                JObject _json = JObject.Load(new JsonTextReader(new StreamReader(jsonPath)));
                _artifactsString = _json["artifacts"].ToString();
            }
            catch (Exception e)
            {
                //MainWindow.MWindow.ArtifactInfoBox.Text = "Error: " + e.Message;
                //MainWindow.MWindow.ArtifactInfoBox.Text += "\nCouldn't load .json file.";
                //MainWindow.MWindow.LoadBarInfo.Content = "Error :(";
                return null;
            }

            try
            {
                _artifacts = JsonConvert.DeserializeObject<List<Artifact>>(_artifactsString);
                //MainWindow.MWindow.ArtifactInfoBox.Text += "\nLoaded " + _artifacts.Count + " artifacts.";
            }
            catch (Exception e)
            {
                //MainWindow.MWindow.ArtifactInfoBox.Text = "Error: " + e.Message;
                //MainWindow.MWindow.ArtifactInfoBox.Text += "\nCouldn't parse artifacts.";
                //MainWindow.MWindow.LoadBarInfo.Content = "Error :(";
                return null;
            }
            return _artifacts;
        }

    }

}
