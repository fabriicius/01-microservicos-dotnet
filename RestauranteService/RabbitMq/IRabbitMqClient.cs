using RestauranteService.Dtos;
using RestauranteService.Models;

namespace RestauranteService.RabbitMq;

public interface IRabbitMqClient
{
   void PublicaRestaurante(RestauranteReadDto readDto);
}
