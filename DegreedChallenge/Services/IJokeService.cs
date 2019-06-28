using System.Collections.Generic;
using System.Threading.Tasks;
using DegreedChallenge.Models;

namespace DegreedChallenge.Services
{
    public interface IJokeService
    {
        Task<DadJoke> GetRandomJoke();

        Task<IEnumerable<DadJoke>> GetJokesWithTerm(string jokeTerm);
    }
}