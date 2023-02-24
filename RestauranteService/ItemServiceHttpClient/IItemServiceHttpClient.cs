using RestauranteService.Dtos;
using RestauranteService.Models;

namespace RestauranteService.ItemServiceHttpClient;

public interface IItemServiceHttpClient
{
   public void EnviaRestauranteParaItemService(RestauranteReadDto readDto);
}
