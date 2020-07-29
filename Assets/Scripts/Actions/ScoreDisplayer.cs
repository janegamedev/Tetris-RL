using Tetris_RL.FSM;
using Tetris_RL.Variables;
using TMPro;

namespace Tetris_RL.Actions
{
    public class ScoreDisplayer : StateActions
    {
        private readonly TextMeshProUGUI _scoreText;

        public ScoreDisplayer(IntVariable s, TextMeshProUGUI t)
        {
            var score = s;
            score.Value = 0;
            score.OnValueChanged += UpdateScore;
            _scoreText = t;
            _scoreText.text = score.Value.ToString();
        }

        public override void Execute()
        {
       
        }

        private void UpdateScore(int s)
        {
            _scoreText.text = s.ToString();
        }
    }
}