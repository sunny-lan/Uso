using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Uso.Core.Song;
using Uso.Core.Timing;

namespace Uso.Core.Judgement
{
    class StandardJudgement : Judgement
    {
        public enum Level
        {
            MISS,
            BAD,
            PERFECT,
            MAGNIFICENT
        }

        public Level TimingJudgement;
    }

    class StandardScore
    {
        public int Combo;
        public long Score;
        public double Accuracy;
    }

    class StandardJudger : Judger<StandardJudgement, StandardScore>
    {
        private Song.Song s;
        private TimeSource ts;
        private Dictionary<int, NoteEvent> toHit = new Dictionary<int, NoteEvent>();

        public class JudgementSettings
        {
            public long NotDetect;
            public long Miss;
            public long Bad;
            public long Perfect;
            public long Magnificent;
        }

        private JudgementSettings settings;

        public StandardJudger(Song.Song s, TimeSource ts)
        {
            this.s = s;
            this.ts = ts;

            settings = new JudgementSettings
            {
                NotDetect = 2 * ts.PPQ,
                Miss = 1 * ts.PPQ,
                Bad = (long)Math.Ceiling(ts.PPQ / 4.0),
                Perfect = (long)Math.Ceiling(ts.PPQ / 16.0),
                Magnificent = (long)Math.Ceiling(ts.PPQ / 64.0),

            };

            foreach (var e in s.JudgedEvents)
            {
                // ts.Schedule(e.Time - settings.NotDetect, () => { 
                //
                //  });
            }

            TotalScore = new StandardScore();
        }

        public StandardScore TotalScore { get; }

        public StandardJudgement JudgeInput(Input i)
        {
            TotalScore.Combo++;


            return new StandardJudgement
            {
                RecordedTime = ts.Time,
                Input = i,
            };
        }
    }
}
