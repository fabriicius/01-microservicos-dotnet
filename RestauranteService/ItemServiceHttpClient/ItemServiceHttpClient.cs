using System;
using System.Text;
using System.Text.Json;
using RestauranteService.Dtos;
using RestauranteService.ItemServiceHttpClient;
using RestauranteService.Models;

namespace RestauranteService.Data
{
    public class ItemServiceHttpClient : IItemServiceHttpClient
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public ItemServiceHttpClient(HttpClient client,
        IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }

        public async void EnviaRestauranteParaItemService(RestauranteReadDto readDto)
        {
            var content = new StringContent
            (
                JsonSerializer.Serialize(readDto),
                Encoding.UTF8,
                "application/json"
            );
            
            await _client.PostAsync( "https://localhost:5000/api/item/restaurante", content);
        }
    }
}