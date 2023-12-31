﻿using MicroService.Api.CarritoCompra.RemoteInterface;
using MicroService.Api.CarritoCompra.RemoteModel;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MicroService.Api.CarritoCompra.RemoteServices
{
    public class LibrosService : ILibrosService
    {
        public readonly IHttpClientFactory _httpClient;
        public readonly ILogger<LibrosService> _logger;

        public LibrosService(IHttpClientFactory httpClient, ILogger<LibrosService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<(bool resultado, LibroRemote Libro, string ErrorMessage)> GetLibro(Guid LibroId)
        {
            try {
                var cliente = _httpClient.CreateClient("Libros");
                var response = await cliente.GetAsync($"api/libreria/{LibroId}");
                
                if(response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var result = JsonSerializer.Deserialize<LibroRemote>(content, options);
                    return (true, result, null);
                }
                return (false, null, response.ReasonPhrase);

            
            }catch(Exception e)
            {
                _logger?.LogError(e.ToString());
                return (false, null, e.Message);
            }
        }
    }
}
