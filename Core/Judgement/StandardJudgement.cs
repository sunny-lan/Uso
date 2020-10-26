using System;
using System.Collections.Generic;
using Uso.Core.Timing;

namespace Uso.Core.Judgement
{
    public class StandardJudgement
    {
        public double RecordedTime;

        /// <summary>
        /// Is null if no note was matched
        /// </summary>
        public Song.NoteEvent Match;

        /// <summary>
        /// The input that was judged
        /// </summary>
        public MIDI.NoteEvent Input;

        public enum Level
        {
            MISS,
            BAD,
            PERFECT,
            MAGNIFICENT
        }

        public Level TimingJudgement;
    }

    public class StandardScore
    {
        public int Combo;
        public long Score;
        public double Accuracy;
    }

    //TODO use default empty implmentation when available
    interface StandardJudgementListener
    {
        void ComboBroken(int newCombo, StandardJudgement reason);
        void ComboUp(int newCombo, StandardJudgement reason);
        void JudgmentPassed(StandardJudgement judgement);
        void ScoreChanged(StandardScore score, StandardJudgement reason);

    }

    class StandardJudger : Judger<MIDI.NoteEvent, StandardScore>
    {
        private TimeSource ts;
        private readonly StandardJudgementListener listener;
        private Dictionary<int, Song.NoteEvent> toHit = new Dictionary<int, Song.NoteEvent>();

        public class JudgementSettings
        {
            public long NotDetect;
            public long Miss;
            public long Bad;
            public long Perfect;
            public long Magnificent;
        }

        private JudgementSettings settings;

        public StandardJudger(List<Song.NoteEvent> judgedEvents, TimeSource ts, StandardJudgementListener listener)
        {
            this.ts = ts;
            this.listener = listener;
            settings = new JudgementSettings
            {
                NotDetect = 2 * ts.PPQ,
                Miss = 1 * ts.PPQ,
                Bad = (long)Math.Ceiling(ts.PPQ / 4.0),
                Perfect = (long)Math.Ceiling(ts.PPQ / 16.0),
                Magnificent = (long)Math.Ceiling(ts.PPQ / 64.0),

            };

            foreach (var e in judgedEvents)
            {
                // ts.Schedule(e.Time - settings.NotDetect, () => { 
                //
                //  });
            }

            TotalScore = new StandardScore();
        }

        public StandardScore TotalScore { get; }

        public void OnInput(MIDI.NoteEvent i)
        {
            var j=new StandardJudgement
            {
                RecordedTime = ts.Time,
                Input = i,
            };
            
            TotalScore.Combo++;
            
            listener.ComboUp(TotalScore.Combo,j);

            listener.ScoreChanged(TotalScore,j);


            listener.JudgmentPassed(j);
        }
    }
}
