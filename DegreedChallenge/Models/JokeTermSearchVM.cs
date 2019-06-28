using System.Collections.Generic;
using System.Linq;

namespace DegreedChallenge.Models
{
    public class JokeTermSearchVM
    {
        public JokeTermSearchVM()
        {
            Term = string.Empty;
            ShortJokes = new List<DadJokeVM>();
            MediumJokes = new List<DadJokeVM>();
            LargeJokes = new List<DadJokeVM>();
        }
        
        public string Term { get; set; }
        public List<DadJokeVM> ShortJokes { get; set; }
        public List<DadJokeVM> MediumJokes{ get; set; }
        public List<DadJokeVM> LargeJokes{ get; set; }
        public bool JokesFound => ShortJokes.Any() || MediumJokes.Any() || LargeJokes.Any();
    }
}