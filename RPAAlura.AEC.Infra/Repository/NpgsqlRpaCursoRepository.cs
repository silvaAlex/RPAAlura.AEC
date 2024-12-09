using Dapper;
using RPAAlura.AEC.Application.Interfaces;
using RPAAlura.AEC.Domain.Entities;
using System.Data;

namespace RPAAlura.AEC.Infra.Repository
{
    public class NpgsqlRpaCursoRepository : IRpaRepository<Curso>
    {
        private readonly IDbConnection _dbConnection;

        public NpgsqlRpaCursoRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void Save(Curso curso)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.Open();
            }

            using var transaction = _dbConnection.BeginTransaction();
            try
            {

                var courseId = _dbConnection.ExecuteScalar<int>(
                    "INSERT INTO \"Cursos\" (\"Titulo\", \"CargaHoraria\", \"Descricao\") VALUES (@Titulo, @CargaHoraria, @Descricao) RETURNING \"Id\"",
                    new { curso.Titulo, curso.CargaHoraria, curso.Descricao },
                    transaction: transaction);

                // Insert the professors
                foreach (var professor in curso.Professores)
                {
                    _dbConnection.Execute(
                        "INSERT INTO \"Professores\" (\"CursoId\", \"Nome\") VALUES (@CursoId, @Nome)",
                        new { CursoId = courseId, professor.Nome },
                        transaction: transaction);
                }

                // Commit the transaction
                transaction.Commit();
            }
            catch (Exception ex)
            {
                // Rollback the transaction if any error occurs
                transaction.Rollback();
                throw new Exception("An error occurred while saving the course and professors.", ex);
            }
        }
    }
}
