//TODO this file is unused

namespace Uso.Core.Judgement
{
    

    abstract class Judgement<InputType,MatchType>
    {
        public double RecordedTime;

        /// <summary>
        /// Is null if no note was matched
        /// </summary>
        public MatchType Match;

        /// <summary>
        /// The input that was judged
        /// </summary>
        public InputType Input;
    }


    /// <summary>
    /// A judger recieves a song and user input as well as a time source
    /// It's job is to correlate inputs with the correct reference note
    /// And maintain overall performance of player
    /// It shall return a judgement object which is then taken by the ui, in order to show score
    /// </summary>
    interface Judger<Input, out Score>
    {
        Score TotalScore { get; }
        void OnInput(Input i);
    }

    interface JudgementListener<InputJudgement, Score, I,M> where InputJudgement : Judgement<I,M>
    {
        void JudgmentPassed(InputJudgement judgement);
        void ScoreChanged(Score score, InputJudgement reason);
    }
}
