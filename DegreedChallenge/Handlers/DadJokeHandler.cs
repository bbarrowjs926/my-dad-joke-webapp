using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DegreedChallenge.Models;
using DegreedChallenge.Services;

namespace DegreedChallenge.Handlers
{
    public class DadJokeHandler
    {
        private IJokeService _jokeService;

        public DadJokeHandler(IJokeService jokeService)
        {
            _jokeService = jokeService;
        }

        public async Task<DadJokeVM> GetRandomJoke()
        {
            var vm = new DadJokeVM();

            var joke = await _jokeService.GetRandomJoke();
            vm.Joke = joke.Joke;

            return vm;
        }

        public async Task<JokeTermSearchVM> GetJokesWithTerm(string jokeTerm)
        {
            // Make sure a term has been completed
            if (string.IsNullOrWhiteSpace(jokeTerm))
            {
                throw new ApplicationException("A term is required for search");
            }

            var vm = new JokeTermSearchVM
            {
                Term = jokeTerm
            };

            var jokes = await _jokeService.GetJokesWithTerm(jokeTerm);

            foreach (var joke in jokes)
            {
                int wordCount = GetWordCount(joke.Joke);
                DadJokeVM currJoke = FormatJokeForDisplay(joke.Joke, jokeTerm);

                if (wordCount < 10)
                {
                    vm.ShortJokes.Add(currJoke);
                }
                else if (wordCount < 20)
                {
                    vm.MediumJokes.Add(currJoke);
                }
                else
                {
                    vm.LargeJokes.Add(currJoke);
                }
            }

            return vm;
        }

        public DadJokeVM FormatJokeForDisplay(string joke, string jokeTerm)
        {
            var formattedJoke = Regex.Replace(joke, jokeTerm, "<span class='look-at-me'>" + jokeTerm + "</span>", RegexOptions.IgnoreCase);

            return new DadJokeVM
            {
                Joke = formattedJoke
            };
        }
        
        public int GetWordCount(string joke)
        {
            int count = 0;

            if (!string.IsNullOrWhiteSpace(joke))
            {
                count = joke.Split(null).Length;
            }

            return count;
        }
    }
}