using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Infra.Migrations
{
    /// <inheritdoc />
    public partial class EstruturaInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(80)", unicode: false, maxLength: 80, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", unicode: false, maxLength: 255, nullable: false),
                    Senha = table.Column<string>(type: "character varying(128)", unicode: false, maxLength: 128, nullable: false),
                    Cpf = table.Column<string>(type: "character varying(11)", unicode: false, maxLength: 11, nullable: false),
                    Nascimento = table.Column<DateTime>(type: "date", nullable: false),
                    Telefone = table.Column<string>(type: "character varying(15)", unicode: false, maxLength: 15, nullable: false),
                    IsAdministrador = table.Column<bool>(type: "boolean", nullable: false),
                    IsAtivo = table.Column<bool>(type: "boolean", nullable: false),
                    IsRedefinirSenha = table.Column<bool>(type: "boolean", nullable: false),
                    TokenRecuperacaoSenha = table.Column<Guid>(type: "uuid", nullable: true),
                    ValidadeRecuperacaoSenha = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ClienteId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

          
            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "Id", "Cpf", "Email", "IsAdministrador", "IsAtivo", "IsRedefinirSenha", "Nascimento", "Nome", "Senha", "Telefone", "TokenRecuperacaoSenha", "ValidadeRecuperacaoSenha" },
                values: new object[] { new Guid("89186c8a-3892-47e6-885e-c376d53d15b1"), "00000000000", "teste.teste@gmail.com", true, true, false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Usuário Teste", "ba3253876aed6bc22d4a6ff53d8406c6ad864195ed144ab5c87621b6c233b548baeae6956df346ec8c17f5ea10f35ee3cbc514797ed7ddd3145464e2a0bab413", "00000000", null, null });

     
            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Email",
                table: "Usuario",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_IsAtivo",
                table: "Usuario",
                column: "IsAtivo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
