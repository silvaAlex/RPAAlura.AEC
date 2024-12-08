namespace RPAAlura.AEC.Domain.Entities
{
    public class Curso
    {
        public string Titulo { get; set; } = string.Empty;
        public string CargaHoraria { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public List<Professor> Professores { get; set; } = new List<Professor>();
    }
}
