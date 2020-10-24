using System;
using System.Collections.Generic;
using System.Text;
using Uso.Core.Song;

namespace Uso.Core.Judgement
{
    abstract class Input
    {
        public int Note;
        public int Velocity;
    }

    class NoteOnInput:Input
    {

    }
    class NoteOffInput : Input
    {

    }

    abstract class Judgement
    {
        public double RecordedTime;

        /// <summary>
        /// Is null if no note was matched
        /// </summary>
        public NoteEvent Match;

        /// <summary>
        /// The input that was judged
        /// </summary>
        public Input Input;
    }


    /// <summary>
    /// A judger recieves a song and user input as well as a time source
    /// It's job is to correlate inputs with the correct reference note
    /// And maintain overall performance of player
    /// It shall return a judgement object which is then taken by the ui, in order to show score
    /// </summary>
    interface Judger<out InputJudgement, out Score> where InputJudgement :Judgement
    {
        Score TotalScore { get; }
        InputJudgement JudgeInput(Input i);
    }

    interface JudgementAccepter<InputJudgement> where InputJudgement : Judgement
    {
        void OnInput(InputJudgement judgement);
    }
}
