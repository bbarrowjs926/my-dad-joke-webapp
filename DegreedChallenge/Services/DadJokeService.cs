using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DegreedChallenge.Models;

namespace DegreedChallenge.Services
{
    public class DadJokeService : IJokeService
    {
        private static readonly HttpClient _httpClient;

        static DadJokeService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<DadJoke> GetRandomJoke()
        {
            using (HttpResponseMessage response = await _httpClient.GetAsync("https://icanhazdadjoke.com/"))
            {
                if (response.IsSuccessStatusCode)
                {
                    DadJoke model = await response.Content.ReadAsAsync<DadJoke>();
                    return model;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<IEnumerable<DadJoke>> GetJokesWithTerm(string jokeTerm)
        {
            string url = $"https://icanhazdadjoke.com/search?limit=30&term={jokeTerm}";
            using (HttpResponseMessage response = await _httpClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    DadJokeSearchResults results = await response.Content.ReadAsAsync<DadJokeSearchResults>();
                    return results.Results;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}