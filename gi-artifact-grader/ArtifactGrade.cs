using System;
using System.Collections.Generic;
using System.Linq;

namespace GIArtifactGrader
{
    internal enum ArtifactGradeEnum
    {
        S = 7,
        A = 6,
        B = 5,
        C = 4,
        D = 3,
        E = 2,
        F = 1
    }

    internal class ArtifactGrade : Artifact
    {
        public int ArtifactNumber { get; set; }
        public ArtifactGradeEnum Grade { get; set; }
        public string? Usage { get; set; }
        public string? SubstatsJoined { get; set; }


        public static List<ArtifactGrade>? GradeForGeneralCV(List<Artifact> artifacts)
        {
            //if (MainWindow.MWindow == null)
            //{
            //    return null;
            //}

            if (artifacts == null)
            {
                //MainWindow.MWindow.ArtifactInfoBox.Text = "Error: Couldn't load artifacts.";
                //MainWindow.MWindow.LoadBarInfo.Content = "Error :(";
                return null;
            }

            List<ArtifactGrade> _artifactGrades = new();
            int _artifactNumber = 1;

            foreach (Artifact _artifact in artifacts)
            {
                ArtifactGrade _artifactGrade = new()
                {
                    ArtifactNumber = _artifactNumber,
                    SetKey = _artifact.SetKey,
                    Rarity = _artifact.Rarity,
                    Level = _artifact.Level,
                    SlotKey = _artifact.SlotKey,
                    MainStatKey = _artifact.MainStatKey,
                    Substats = _artifact.Substats,
                    Location = _artifact.Location,
                    Locked = _artifact.Locked,
                    SubstatsJoined = string.Join(", ", _artifact.Substats.Select(x => x.Key + ": " + x.Value))
                };

                var _match_cr = _artifactGrade.Substats.FirstOrDefault(s => s.Key.Contains("critRate"));
                var _match_cd = _artifactGrade.Substats.FirstOrDefault(s => s.Key.Contains("critDMG"));

                // 2x crit substats => grade = A
                if (_match_cr != null && _match_cd != null)
                {
                    _artifactGrade.Grade = ArtifactGradeEnum.A;
                    _artifactGrade.Usage = "2x CV";
                    _artifactGrades.Add(_artifactGrade);
                    _artifactNumber++;
                    continue;
                }

                // main stat isn't crit and 1x crit substat => grade = C
                if ((!_artifactGrade.MainStatKey.Contains("critRate")
                    && !_artifactGrade.MainStatKey.Contains("critDMG"))
                    && (_match_cr != null || _match_cd != null))
                {
                    _artifactGrade.Grade = ArtifactGradeEnum.C;
                    _artifactGrade.Usage = "1x CV";
                    _artifactGrades.Add(_artifactGrade);
                    _artifactNumber++;
                    continue;
                }

                // main stat is crit and 1x crit substat => grade = A
                if ((_artifactGrade.MainStatKey.Contains("critRate")
                    || _artifactGrade.MainStatKey.Contains("critDMG"))
                    && (_match_cr != null || _match_cd != null))
                {
                    _artifactGrade.Grade = ArtifactGradeEnum.A;
                    _artifactGrade.Usage = "CV MS + 1x CV";
                    _artifactGrades.Add(_artifactGrade);
                    _artifactNumber++;
                    continue;
                }

                // no crit mainstat or substats
                _artifactGrade.Grade = ArtifactGradeEnum.F;
                _artifactGrade.Usage = "Convert";
                _artifactGrades.Add(_artifactGrade);
                _artifactNumber++;
            }
            //MainWindow.MWindow.ArtifactInfoBox.Text += $"\nGraded {_artifactGrades.Count} artifacts!";
            //MainWindow.MWindow.LoadBarInfo.Content = "Grading complete!";

            return _artifactGrades;
        }

        public static List<ArtifactGrade>? GradeForYelan(List<ArtifactGrade> gradedArtifacts)
        {
            //if (MainWindow.MWindow == null)
            //{
            //    return null;
            //}

            if (gradedArtifacts == null)
            {
                //MainWindow.MWindow.ArtifactInfoBox.Text = "Error: Couldn't load artifacts (Yelan grading).";
                //MainWindow.MWindow.LoadBarInfo.Content = "Error :(";
                return null;
            }

            foreach (ArtifactGrade _artifactGrade in gradedArtifacts)
            {
                var _match_cr = _artifactGrade.Substats.FirstOrDefault(s => s.Key.Contains("critRate"));
                var _match_cd = _artifactGrade.Substats.FirstOrDefault(s => s.Key.Contains("critDMG"));
                var _match_hp_ = _artifactGrade.Substats.FirstOrDefault(s => s.Key.Contains("hp_"));

                // main stat is hp% and 2x crit substats => grade = S
                if (_artifactGrade.MainStatKey.Contains("hp_")
                    && _match_cr != null
                    && _match_cd != null)
                {
                    if (_artifactGrade.Grade <= ArtifactGradeEnum.S)
                    {
                        _artifactGrade.Grade = ArtifactGradeEnum.S;
                        _artifactGrade.Usage = "Yelan";
                    }
                }

                // main stat is flat hp OR flat atk AND has 2 crit substats AND hp% substat => grade = S
                if ((_artifactGrade.MainStatKey.Contains("hp")
                    || _artifactGrade.MainStatKey.Contains("atk"))
                    && _match_cr != null
                    && _match_cd != null
                    && _match_hp_ != null)
                {
                    if (_artifactGrade.Grade <= ArtifactGradeEnum.S)
                    {
                        _artifactGrade.Grade = ArtifactGradeEnum.S;
                        _artifactGrade.Usage = "Yelan";
                    }
                }
            }
            return gradedArtifacts;
        }

        public static List<ArtifactGrade>? GradeForNilou(List<ArtifactGrade> gradedArtifacts)
        {
            //if (MainWindow.MWindow == null)
            //{
            //    return null;
            //}

            if (gradedArtifacts == null)
            {
                //MainWindow.MWindow.ArtifactInfoBox.Text = "Error: Couldn't load artifacts (Yelan grading).";
                //MainWindow.MWindow.LoadBarInfo.Content = "Error :(";
                return null;
            }

            foreach (ArtifactGrade _artifactGrade in gradedArtifacts)
            {
                var _match_hp_ = _artifactGrade.Substats.FirstOrDefault(s => s.Key.Contains("hp_"));
                var _match_eleMas = _artifactGrade.Substats.FirstOrDefault(s => s.Key.Contains("eleMas"));

                // main stat is hp% and has eleMas substat => grade = A
                if (_artifactGrade.MainStatKey.Contains("hp_")
                    && _match_eleMas != null)
                {
                    if (_artifactGrade.Grade <= ArtifactGradeEnum.A)
                    {
                        _artifactGrade.Grade = ArtifactGradeEnum.A;
                        _artifactGrade.Usage = "Nilou";
                    }
                }

                // main stat is flat hp OR flat atk AND has eleMas AND hp% substats => grade = A
                if ((_artifactGrade.MainStatKey.Contains("hp")
                    || _artifactGrade.MainStatKey.Contains("atk"))
                    && _match_eleMas != null
                    && _match_hp_ != null)
                {
                    if (_artifactGrade.Grade <= ArtifactGradeEnum.A)
                    {
                        _artifactGrade.Grade = ArtifactGradeEnum.A;
                        _artifactGrade.Usage = "Nilou";
                    }
                }
            }
            return gradedArtifacts;
        }

        public static List<ArtifactGrade>? GradeForKazuha(List<ArtifactGrade> gradedArtifacts)
        {

            return null;
        }

        public static List<ArtifactGrade>? GradeForKokomi(List<ArtifactGrade> gradedArtifacts)
        {

            return null;
        }

        public static List<ArtifactGrade>? GradeForYunJin(List<ArtifactGrade> gradedArtifacts)
        {

            return null;
        }

        public override string ToString()
        {
            string _returnString = String.Empty;
            _returnString += $"Artifact Number: {ArtifactNumber}\n";
            _returnString += $"Grade: {Grade}, Usage: {Usage}\n";
            _returnString += $"Set: {SetKey}, Slot: {SlotKey}, Main stat: {MainStatKey}\n";
            string combinedSubstats = string.Join(", ", Substats.Select(x => x.Key + ": " + x.Value));
            _returnString += $"Substats: {combinedSubstats}\n\n";
            return _returnString;
        }
    }

}

