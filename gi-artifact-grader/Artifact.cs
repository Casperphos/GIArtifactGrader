using System.Collections.Generic;

namespace GIArtifactGrader
{
    internal class Artifact
    {
        public string? SetKey { get; set; }
        public short Rarity { get; set; }
        public short Level { get; set; }
        public string? SlotKey { get; set; }
        public string? MainStatKey { get; set; }
        public List<Substat> Substats { get; set; } = new List<Substat>();
        public string? Location { get; set; }
        public bool Locked { get; set; }
    }

    internal class Substat
    {
        public string? Key { get; set; }
        public double Value { get; set; }
    }
}
