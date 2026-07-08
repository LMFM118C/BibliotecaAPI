using BibliotecaAPI.DTOs;
using BibliotecaAPI.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.ClientModel.Primitives;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BibliotecaApiTests.Integration
{
    public class AutoresIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient Client;
        public AutoresIntegrationTests(WebApplicationFactory<Program> factory)
        {
            Client = factory.CreateClient();
        }

        [Fact]
        public async Task ObtenerTodas_DebeResponderCorrectamente()
        {
            HttpResponseMessage respuesta = await Client.GetAsync("/api/autor");

            respuesta.IsSuccessStatusCode.Should().BeTrue();
        }

        [Fact]
        public async Task ObtenerTodas_DebeDevolverUnaLista()
        {
            List<AutorDTO>? autores = await Client.GetFromJsonAsync<List<AutorDTO>>("/api/autor");

            autores.Should().NotBeNull();
            autores.Should().NotBeEmpty();
        }

        [Fact]
        public async Task ObtenerPorId_Existente_DebeDevolverAutor()
        {
            AutorDTO? autor = await Client.GetFromJsonAsync<AutorDTO>("/api/autor/1");

            autor.Should().NotBeNull();
            autor!.Id.Should().Be(1);
            autor.Nombre.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task ObtenerPorId_NoExistente_DebeDevolver404()
        {
            HttpResponseMessage respuesta = await Client.GetAsync("api/autor/9999");

            respuesta.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Crear_DebeCrearNuevoAutor()
        {
            AutorCreateDTO nuevoAutor = new()
            {
                Nombre = "Rigoberto Osorio",
                Nacionalidad = "España",
                FechaNacimiento = new DateTime(1978-01-21)
            };

            await Client.PostAsJsonAsync("/api/autor", nuevoAutor);
            List<AutorDTO>? autores = await Client.GetFromJsonAsync<List<AutorDTO>>("/api/autor");
            autores!.Any(d => d.Nombre == "Rigoberto Osorio").Should().BeTrue();
        }

        [Fact]
        public async Task Actualizar_DebeModificarLosDatos()
        {
            AutorUpdateDTO actualizar = new()
            {
                Nombre = "J. K. Rowling Actualizada",
                Nacionalidad = "Francia",
                FechaNacimiento = new DateTime(1972 - 08 - 11)
            };

            HttpResponseMessage respuesta = await Client.PutAsJsonAsync("/api/autor/1", actualizar);
            respuesta.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Actualizar_DebeGuardarLosCambios()
        {
            AutorUpdateDTO actualizar = new()
            {
                Nombre = "J. K. Rowling Actualizada",
                Nacionalidad = "Francia",
                FechaNacimiento = new DateTime(1972 - 08 - 11)
            };

            await Client.PutAsJsonAsync("/api/autor/1", actualizar);
            AutorDTO? Autor = await Client.GetFromJsonAsync<AutorDTO>("/api/autor/1");
            Autor!.Nombre.Should().Be("J. K. Rowling Actualizada");
        }

        [Fact]
        public async Task Actualizar_NoExistente_DebeDevolver404()
        {
            AutorUpdateDTO actualizar = new()
            {
                Nombre = "J. K. Rowling Actualizada",
                Nacionalidad = "Francia",
                FechaNacimiento = new DateTime(1972 - 08 - 11)
            };

            HttpResponseMessage respuesta = await Client.PutAsJsonAsync("/api/autor/9999", actualizar);
            respuesta.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Eliminar_DebeEliminarAutor()
        {
            AutorCreateDTO nuevoAutor = new()
            {
                Nombre = "Rigoberto Osorio",
                Nacionalidad = "España",
                FechaNacimiento = new DateTime(1978 - 01 - 21)
            };

            await Client.PostAsJsonAsync("/api/autor", nuevoAutor);
            List<AutorDTO>? lista = await Client.GetFromJsonAsync<List<AutorDTO>>("/api/autor");
            int id = lista!.Last().Id;
            HttpResponseMessage respuesta = await Client.DeleteAsync($"/api/autor/{id}");
            respuesta.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Eliminar_DebeDesaparecerDeLaLista()
        {
            AutorCreateDTO nuevoAutor = new()
            {
                Nombre = "Rigoberto Osorio",
                Nacionalidad = "España",
                FechaNacimiento = new DateTime(1978 - 01 - 21)
            };

            await Client.PostAsJsonAsync("/api/autor", nuevoAutor);
            List<AutorDTO>? lista = await Client.GetFromJsonAsync<List<AutorDTO>>("/api/autor");
            int id = lista!.Last().Id;
            await Client.DeleteAsync($"/api/autor/{id}");
            HttpResponseMessage respuesta = await Client.GetAsync($"/api/autor/{id}");
            respuesta.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}
