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
    public class LibrosIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient Client;

        public LibrosIntegrationTests(WebApplicationFactory<Program> factory)
        {
            Client = factory.CreateClient();
        }

        [Fact]
        public async Task ObtenerTodas_DebeResponderCorrectamente()
        {
            HttpResponseMessage respuesta = await Client.GetAsync("/api/libro");
            respuesta.IsSuccessStatusCode.Should().BeTrue();
        }

        [Fact]
        public async Task ObtenerTodas_DebeDevolverUnaLista()
        {
            List<LibroDTO>? libros = await Client.GetFromJsonAsync<List<LibroDTO>>("/api/libro");
            libros.Should().NotBeNull();
            libros.Should().NotBeEmpty();
        }

        [Fact]
        public async Task ObtenerPorId_Existente_DebeDevolverLibro()
        {
            LibroDTO? libro = await Client.GetFromJsonAsync<LibroDTO>("/api/libro/1");

            libro.Should().NotBeNull();
            libro!.Id.Should().Be(1);
            libro.Titulo.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task ObtenerPorId_NoExistente_DebeDevolver404()
        {
            HttpResponseMessage respuesta = await Client.GetAsync("/api/libro/9999");
            respuesta.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ObtenerPorGenero_DebeDevolverUnaLista()
        {
            List<LibroDTO>? libros = await Client.GetFromJsonAsync<List<LibroDTO>>("/api/libro/genero/Terror");
            libros.Should().NotBeNull();
            libros.Should().NotBeEmpty();
        }

        [Fact]
        public async Task ObtenerPorAutor_DebeDevolverUnaLista()
        {
            List<LibroDTO>? libros = await Client.GetFromJsonAsync<List<LibroDTO>>("/api/libro/autor/1");
            libros.Should().NotBeNull();
            libros.Should().NotBeEmpty();
        }

        [Fact]
        public async Task ObtenerDisponibles_DebeDevolverUnaLista()
        {
            List<LibroDTO>? libros = await Client.GetFromJsonAsync<List<LibroDTO>>("/api/libro/disponibles");
            libros.Should().NotBeNull();
            libros.Should().NotBeEmpty();
        }

        [Fact]
        public async Task ObtenerNoDisponibles_DebeDevolverUnaLista()
        {
            List<LibroDTO>? libros = await Client.GetFromJsonAsync<List<LibroDTO>>("/api/libro/no-disponibles");
            libros.Should().NotBeNull();
            libros.Should().NotBeEmpty();
        }

        [Fact]
        public async Task ObtenerBaratos_DebeDevolverUnaLista()
        {
            List<LibroDTO>? libros = await Client.GetFromJsonAsync<List<LibroDTO>>("/api/libro/baratos");
            libros.Should().NotBeNull();
            libros.Should().NotBeEmpty();
        }

        [Fact]
        public async Task ObtenerCaros_DebeDevolverUnaLista()
        {
            List<LibroDTO>? libros = await Client.GetFromJsonAsync<List<LibroDTO>>("/api/libro/caros");
            libros.Should().NotBeNull();
            libros.Should().NotBeEmpty();
        }

        [Fact]
        public async Task ObtenerPorBusqueda_DebeDevolverUnaLista()
        {
            List<LibroDTO>? libros = await Client.GetFromJsonAsync<List<LibroDTO>>("/api/libro/buscar/Harry Potter");
            libros.Should().NotBeNull();
            libros.Should().NotBeEmpty();
            libros!.Any(d => d.Titulo.Contains("Harry Potter")).Should().BeTrue();

        }

        [Fact]
        public async Task ObtenerEstadisticas_DebeDevolverUnaLista()
        {
            EstadisticasDTO? estadisticas = await Client.GetFromJsonAsync<EstadisticasDTO>("/api/libro/estadisticas");
            estadisticas.Should().NotBeNull();
        }

        [Fact]
        public async Task Crear_DebeCrearNuevoLibro()
        {
            LibroCreateDTO nuevoLibro = new()
            {
                Titulo = "Silent Hill 2 : El retorn de la colina",
                Genero = "Terror",
                NumeroPaginas = 502,
                Precio = new decimal(19.60),
                Disponible = true,
                FechaPublicacion = new DateTime(1997-10-31),
                AutorId = 2
            };

            HttpResponseMessage respuesta = await Client.PostAsJsonAsync("/api/libro", nuevoLibro);
            respuesta.IsSuccessStatusCode.Should().BeTrue();
        }

        [Fact]
        public async Task Crear_DebeAparecerEnLaLista()
        {
            LibroCreateDTO nuevoLibro = new()
            {
                Titulo = "Silent Hill 2 : El retorn de la colina",
                Genero = "Terror",
                NumeroPaginas = 502,
                Precio = new decimal(19.60),
                Disponible = true,
                FechaPublicacion = new DateTime(1997 - 10 - 31),
                AutorId = 2
            };

            await Client.PostAsJsonAsync("/api/libro", nuevoLibro);
            List<LibroDTO>? libros = await Client.GetFromJsonAsync<List<LibroDTO>>("/api/libro");
            libros!.Any(d => d.Titulo == "Silent Hill 2 : El retorn de la colina").Should().BeTrue();
        }

        [Fact]
        public async Task Actualizar_DebeModificarLosDatos()
        {
            LibroUpdateDTO actualizar = new()
            {
                Titulo = "Harry Potter y el sueño que da la franquicia",
                Genero = "Terror",
                NumeroPaginas = 1204,
                Precio = new decimal(59.99),
                Disponible = true,
                FechaPublicacion = new DateTime(2001 - 11 - 21),
                AutorId = 1
            };

            HttpResponseMessage respuesta = await Client.PutAsJsonAsync("/api/libro/1", actualizar);
            respuesta.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Actualizar_DebeGuardarLosCambios()
        {
            LibroDTO actualizar = new()
            {
                Titulo = "Harry Potter y el sueño que da la franquicia",
                Genero = "Terror",
                NumeroPaginas = 1204,
                Precio = new decimal(59.99),
                Disponible = true,
                FechaPublicacion = new DateTime(2001 - 11 - 21),
                AutorId = 1
            };

            await Client.PutAsJsonAsync("/api/libro/1", actualizar);
            LibroDTO? Libro = await Client.GetFromJsonAsync<LibroDTO>("/api/libro/1");
            Libro!.Titulo.Should().Be("Harry Potter y el sueño que da la franquicia");
        }

        [Fact]
        public async Task Actualizar_NoExistente_DebeDevolver404()
        {
            LibroUpdateDTO actualizar = new()
            {
                Titulo = "Harry Potter y el sueño que da la franquicia",
                Genero = "Terror",
                NumeroPaginas = 1204,
                Precio = new decimal(59.99),
                Disponible = true,
                FechaPublicacion = new DateTime(2001 - 11 - 21),
                AutorId = 1
            };

            HttpResponseMessage respuesta = await Client.PutAsJsonAsync("/api/libro/9999", actualizar);
            respuesta.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Eliminar_DebeEliminarLibro()
        {
            LibroCreateDTO nuevoLibro = new()
            {
                Titulo = "Silent Hill 2 : El retorn de la colina",
                Genero = "Terror",
                NumeroPaginas = 502,
                Precio = new decimal(19.60),
                Disponible = true,
                FechaPublicacion = new DateTime(1997 - 10 - 31),
                AutorId = 2
            };

            await Client.PostAsJsonAsync("/api/libro", nuevoLibro);
            List<LibroDTO>? lista = await Client.GetFromJsonAsync<List<LibroDTO>>("/api/libro");
            int id = lista!.Last().Id;
            HttpResponseMessage respuesta = await Client.DeleteAsync($"/api/libro/{id}");
            respuesta.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Eliminar_DebeDesaparecerDeLaLista()
        {
            LibroCreateDTO nuevoLibro = new()
            {
                Titulo = "Silent Hill 2 : El retorn de la colina",
                Genero = "Terror",
                NumeroPaginas = 502,
                Precio = new decimal(19.60),
                Disponible = true,
                FechaPublicacion = new DateTime(1997 - 10 - 31),
                AutorId = 2
            };

            await Client.PostAsJsonAsync("/api/libro", nuevoLibro);
            List<LibroDTO>? lista = await Client.GetFromJsonAsync<List<LibroDTO>>("/api/libro");
            int id = lista!.Last().Id;
            await Client.DeleteAsync($"/api/libro/{id}");
            HttpResponseMessage respuesta = await Client.GetAsync($"/api/libro/{id}");
            respuesta.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PrestarLibro_DebeDevolverSuccess()
        {
            var peticion = new HttpRequestMessage(HttpMethod.Patch, "/api/libro/1/prestar");
            HttpResponseMessage respuesta = await Client.SendAsync(peticion);
            respuesta.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DevolverLibro_DebeDevolverSuccess()
        {
            var peticion = new HttpRequestMessage(HttpMethod.Patch, "/api/libro/3/devolver");
            HttpResponseMessage respuesta = await Client.SendAsync(peticion);
            respuesta.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

    }
}
