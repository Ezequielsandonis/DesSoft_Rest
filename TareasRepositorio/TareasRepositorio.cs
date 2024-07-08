using System.Data;
using System;
using AppDbContext;
using Microsoft.Data.SqlClient;
using Modelos;
using System.Globalization;

namespace TareasRepositorio
{
    public class TareaRepositorio
    {
        //Llamar a la conexion 
        private readonly Contexto _contexto;


        //constructor para iniciliazar la clase con la conexion 
        public TareaRepositorio(Contexto contexto)
        {
            _contexto = contexto;
      
        }

        //implementacion del repositorio


        public List<TareaDto> ListarTareas()
        {
            //se crea una nueva lista del modelo 
            var tareas = new List<TareaDto>();

            //abrir conexion
            using (SqlConnection con = new(_contexto.Conexion))
            {
                //llamar procedimiento almacenado
                using (SqlCommand cmd = new SqlCommand("ListarTareas", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //abrir conexion y ejecutar
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        var tarea = new TareaDto
                        {
                            //castear
                            TareaId = (int)rdr["TareaId"],
                            Titulo = rdr["Titulo"].ToString(),
                            FechaLimite = Convert.ToDateTime(rdr["FechaLimite"]),
                            Estado = (Boolean)rdr["Estado"]
                        };

                        //agregar a la lista
                        tareas.Add(tarea);
                    }
                }
            }

            return tareas;
        }


        //ListarPorId 
        public TareaDto ListarTareaPorId(int id)
        {
            //iniciar instancia 
            TareaDto tarea = new();
            using (SqlConnection con = new(_contexto.Conexion))
            {
                using (SqlCommand cmd = new("ListarTareaPorId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // parametros
                    cmd.Parameters.AddWithValue("@TareaId", id);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        tarea = new TareaDto
                        {
                            //castear
                            TareaId = id,
                            Titulo = rdr["Titulo"].ToString(),
                            FechaLimite = Convert.ToDateTime(rdr["FechaLimite"]),
                            Estado = (Boolean)rdr["Estado"]
                        };
                    }

                }
            }

            return (tarea);
        }

        // insertar 
        public void InsertarTarea(TareaDto tarea)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_contexto.Conexion))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("InsertarTarea", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Titulo", tarea.Titulo);
                        cmd.Parameters.AddWithValue("@FechaLimite", tarea.FechaLimite);
                        cmd.Parameters.AddWithValue("@Estado", tarea.Estado);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Lanza una excepción para manejo en el controlador
                throw new ApplicationException("Error inesperado al insertar la tarea.", ex);
            }
        }


        //actualizar
        public void ActualizarTarea(TareaDto tarea)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_contexto.Conexion))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("ActualizarTarea", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TareaId", tarea.TareaId);
                        cmd.Parameters.AddWithValue("@Titulo", tarea.Titulo);
                        cmd.Parameters.AddWithValue("@FechaLimite", tarea.FechaLimite);
                        cmd.Parameters.AddWithValue("@Estado", tarea.Estado);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Lanza una excepción para manejo en el controlador
                throw new ApplicationException("Error inesperado al actualizar la tarea.", ex);
            }
        }

        //eliminar
        public void EliminarTarea(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_contexto.Conexion))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("EliminarTarea", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TareaId", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Lanza una excepción para manejo en el controlador
                throw new ApplicationException("Error inesperado al Eliminar la tarea.", ex);
            }
        }


    }
}

