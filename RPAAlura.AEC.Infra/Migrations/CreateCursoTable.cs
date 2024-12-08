using FluentMigrator;

namespace RPAAlura.AEC.Infra.Migrations
{
    [Migration(2024120701)]
    public class CreateCursoTable : Migration
    {
        public override void Down()
        {
            Delete.Table("Professores");
            Delete.Table("Cursos");
        }

        public override void Up()
        {
            Create.Table("Cursos")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Titulo").AsString().NotNullable()
                .WithColumn("CargaHoraria").AsInt32().NotNullable()
                .WithColumn("Descricao").AsString().NotNullable();

            Create.Table("Professores")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("CursoId").AsInt32().NotNullable().ForeignKey("Cursos", "Id")
                .WithColumn("Nome").AsString().NotNullable();
        }
    }
}
