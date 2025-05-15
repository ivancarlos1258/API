using API.Domain.TableModels;
using API.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infra.Configuration
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> entity)
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(x => x.Email);
            entity.HasIndex(x => x.IsAtivo);

            entity.Property(e => e.Cpf).IsRequired().HasMaxLength(11).IsUnicode(false);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255).IsUnicode(false);
            entity.Property(e => e.Nascimento).HasColumnType("date");
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(80).IsUnicode(false);
            entity.Property(e => e.Senha).IsRequired().HasMaxLength(128).IsUnicode(false);
            entity.Property(e => e.IsAdministrador).IsRequired();
            entity.Property(e => e.IsAtivo).IsRequired();
            entity.Property(e => e.IsRedefinirSenha).IsRequired();
            entity.Property(e => e.Telefone).HasMaxLength(15).IsUnicode(false).IsRequired();


            //usuário inicial do sistema
            entity.HasData(new Usuario()
            {
                Id = Guid.Parse("89186c8a-3892-47e6-885e-c376d53d15b1"),
                Cpf = "00000000000",
                Email = "teste.teste@rumosolucoes.com",
                IsAdministrador = true,
                IsAtivo = true,
                IsRedefinirSenha = false,
                Nascimento = DateTime.Parse("2024-01-01"),
                Nome = "Usuário Teste",
                Senha = Criptografia.StringParaSha512("123456"),
                Telefone = "00000000"
            });
        }
    }
}
