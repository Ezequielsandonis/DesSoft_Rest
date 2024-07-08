using Xunit;
using Moq;
using System.Collections.Generic;
using System.Data;
using AppDbContext;

namespace TareasRepositorio.Tests
{
    public class TareaRepositorioTests
    {
        private readonly Mock<Contexto> _mockContexto;

        public TareaRepositorioTests()
        {
            // Configurar el mock de Contexto
            _mockContexto = new Mock<Contexto>("server=Pruebas; database=Pruebas; user id=Pruebas;pwd=dasdasd; TrustServerCertificate=true;");
        }

        [Fact]
        public void ListarTareas_DeberiaRetornarListaDeTareas()
        {
            // Arrange
            var mockDataReader = new Mock<IDataReader>();
            mockDataReader.SetupSequence(r => r.Read())
                .Returns(true)
                .Returns(false); // Simulamos una sola tarea en el reader

            mockDataReader.Setup(r => r["TareaId"]).Returns(1);
            mockDataReader.Setup(r => r["Titulo"]).Returns("Tarea 1");
            mockDataReader.Setup(r => r["FechaLimite"]).Returns(DateTime.Now);
            mockDataReader.Setup(r => r["Estado"]).Returns(true);

            var mockCommand = new Mock<IDbCommand>();
            mockCommand.Setup(c => c.ExecuteReader()).Returns(mockDataReader.Object);

            // Configurar el repositorio con el mock de Contexto
            var tareaRepositorio = new TareaRepositorio(_mockContexto.Object);

            // Act
            var tareas = tareaRepositorio.ListarTareas();

            // Assert
            Assert.NotNull(tareas);
            Assert.Single(tareas);
            Assert.Equal("Tarea 1", tareas[0].Titulo);
        }
    }
}
