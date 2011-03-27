using System;
using System.Collections.Generic;

namespace NEmplode.EmpegCar.Model.Database
{
    public class ItemBase
    {
        private Dictionary<string, string> _tags = new Dictionary<string, string>();

        protected ItemBase(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }

        public string Artist { get; set; }
        public string Title { get; set; }
        public string Source { get; set; }
        public int Year { get; set; }
        public int TrackNumber { get; set; }
        public string Genre { get; set; }

        public TimeSpan Duration { get; set; }
        public long Length { get; set; }

        public int Options { get; set; }
        public int PickN { get; set; }
        public int PickPercent { get; set; }

        public DateTime CreationTime { get; set; }

        public string Codec { get; set; }
        public string Bitrate { get; set; }
        public int SampleRate { get; set; }

        public long Offset { get; set; }

        public override string ToString()
        {
            return string.Format("{0:X}: {1} - {2} - {3} - {4}", Id, Source, TrackNumber, Artist, Title);
        }

        public void Add(string key, string value)
        {
            _tags[key] = value;
        }

        // TODO: I could store the stuff in a dictionary anyway, and make the properties do the conversions.
    }
}