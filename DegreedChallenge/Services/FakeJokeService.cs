using System.Collections.Generic;
using System.Threading.Tasks;
using DegreedChallenge.Models;

namespace DegreedChallenge.Services
{
    public class FakeJokeService : IJokeService
    {
        private static int _counter;

        private static readonly DadJoke[] RandomJokes = {
            new DadJoke
            {
                Id = "123",
                Joke = "Why did the chicken cross the road"
            },
            new DadJoke
            {
                Id = "456",
                Joke = "This is a funny dad joke that people will roll their eyes at"
            }
        };

        public async Task<DadJoke> GetRandomJoke()
        {
            var returnJoke = RandomJokes[_counter];
            _counter++;

            if (_counter >= RandomJokes.Length)
            {
                _counter = 0;
            }

            return returnJoke;
        }

        public async Task<IEnumerable<DadJoke>> GetJokesWithTerm(string jokeTerm)
        {
            var allJokes = new List<DadJoke>();

            for (int i = 0; i < 30; i++)
            {
                allJokes.Add(new DadJoke
                {
                    Id = i.ToString(),
                    Joke = $"This joke is the {i + 1} joke and it has the word {jokeTerm} in it somewhere"
                });
            }

            return allJokes;
        }
    }
}