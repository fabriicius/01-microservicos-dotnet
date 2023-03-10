using System.Text.Json;
using AutoMapper;
using ItemService.Data;
using ItemService.Dtos;
using ItemService.Models;

namespace ItemService.EventProcessor
{
    public class ProcessarEnvento : IProcessarEnvento
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public ProcessarEnvento(IMapper mapper)
        {
            _mapper = mapper;
        }

        public void Processa(string mensagem)
        {
            using var scope = _scopeFactory.CreateAsyncScope();
            var itemRepository = scope.ServiceProvider.GetRequiredService<IItemRepository>();

            var restauranteReadDto = JsonSerializer.Deserialize<RestauranteReadDto>(mensagem);
            var restaurante = _mapper.Map<Restaurante>(restauranteReadDto);

            if(!itemRepository.ExisteRestauranteExterno(restaurante.Id))
            {
                itemRepository.CreateRestaurante(restaurante);
                itemRepository.SaveChanges();
            }
        }
    }
}