using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CONTROLECT.Models
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Atleta> Atleta { get; set; }
        public virtual DbSet<AtletaModalidade> AtletaModalidade { get; set; }
        public virtual DbSet<Despesa> Despesa { get; set; }

        public virtual DbSet<ExameFaixa> ExameFaixa{ get; set; }

        public virtual DbSet<ExameFaixaDetalhe> ExameFaixaDetalhe { get; set; }

        public virtual DbSet<Faixa> Faixa { get; set; }

        public virtual DbSet<FormaPagamento> Formapagamento { get; set; }
        public virtual DbSet<Item> Item { get; set; }

        public virtual DbSet<ItemDespesa> ItemDespesa { get; set; }
        public virtual DbSet<ItemVenda> ItemVenda { get; set; }

        public virtual DbSet<Mensalidade> Mensalidade { get; set; }
        public virtual DbSet<Mes> Mes { get; set; }
        public virtual DbSet<Modalidade> Modalidade { get; set; }
        public virtual DbSet<Presenca> Presenca { get; set; }
        public virtual DbSet<Professor> Professor { get; set; }
        public virtual DbSet<ProfessorModalidade> ProfessorModalidade { get; set; }
        public virtual DbSet<Turno> Turno { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //optionsBuilder.UseSqlServer("Data Source=DESKTOP-0SIG7CL\\SQLEXPRESS;Initial Catalog=BANCODADOSCT;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Atleta>(entity =>
            {
                entity.HasKey(e => e.IdAtleta)
                    .HasName("PK__tmp_ms_x__392F4303F5D9F47A");

                entity.ToTable("ATLETA");

                entity.Property(e => e.Bairro)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Celular)
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.Cep)
                    .HasColumnName("CEP")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Cpf)
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.Ddd)
                    .HasColumnName("DDD")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Endereco)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Identidade)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NomeCompleto)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.NomeResponsavel)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.PrimeiroNome)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Telefone)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UltimoNome)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

            });

            modelBuilder.Entity<AtletaModalidade>(entity =>
            {
                entity.HasKey(e => e.IdAtletaModalidade)
                    .HasName("PK__tmp_ms_x__1AD04B13D7FF28EA");

                entity.ToTable("ATLETA_MODALIDADE");

                entity.Property(e => e.IdTurno).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.IdAtletaNavigation)
                    .WithMany(p => p.AtletaModalidade)
                    .HasForeignKey(d => d.IdAtleta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ATLETA_MODALIDADE_ToAtleta");

                entity.HasOne(d => d.IdModalidadeNavigation)
                    .WithMany(p => p.AtletaModalidade)
                    .HasForeignKey(d => d.IdModalidade)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ATLETA_MODALIDADE_ToModalidade");

                entity.HasOne(d => d.IdTurnoNavigation)
                    .WithMany(p => p.AtletaModalidade)
                    .HasForeignKey(d => d.IdTurno)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ATLETA_MODALIDADE_ToTurno");

            });

            modelBuilder.Entity<Despesa>(entity =>
            {
                entity.HasKey(e => e.IdDespesa)
                    .HasName("PK__tmp_ms_x__9D84BE1B29E6C094");

                entity.ToTable("DESPESA");

                entity.Property(e => e.NomeDespesa)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);
            });


            modelBuilder.Entity<ExameFaixa>(entity =>
            {
                entity.HasKey(e => e.IdExameFaixa)
                    .HasName("PK__tmp_ms_x__9D84BE1B29E6C094");

                entity.ToTable("EXAMEFAIXA");

                entity.Property(e => e.NomeExameFaixa)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdModalidadeNavigation)
                    .WithMany(p => p.ExameFaixa)
                    .HasForeignKey(d => d.IdModalidade)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EXAMEFAIXA_ToModalidade");

            });


            modelBuilder.Entity<ExameFaixaDetalhe>(entity =>
            {
                entity.HasKey(e => e.IdExameFaixaDetalhe)
                    .HasName("PK__EXAMEFAIXADETALHE__50FB6F5D88DB9488");



                entity.ToTable("EXAMEFAIXADETALHE");


                entity.Property(e => e.Valor).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DataPagamento).HasColumnType("date");

                entity.HasOne(d => d.IdExameNavigation)
                    .WithMany(p => p.ExameFaixaDetalhe)
                    .HasForeignKey(d => d.IdExameFaixa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DETALHE_EXAME_ToExame");


                entity.HasOne(d => d.IdAtletaNavigation)
                    .WithMany(p => p.ExameFaixaDetalhe)
                    .HasForeignKey(d => d.IdAtleta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DETALHE_EXAME_ToAtleta");


                entity.HasOne(d => d.IdFormaPagamentoNavigation)
                    .WithMany(p => p.ExameFaixaDetalhe)
                    .HasForeignKey(d => d.IdFormaPagamento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DETALHE_EXAME_ToFormaPagamento");

            });

            //modelBuilder.Entity<Despesa>(entity =>
            //{
            //    entity.HasKey(e => e.IdDespesa)
            //        .HasName("PK__tmp_ms_x__9D84BE1B29E6C094");

            //    entity.ToTable("DESPESA");

            //    entity.Property(e => e.NomeDespesa)
            //        .IsRequired()
            //        .HasMaxLength(80)
            //        .IsUnicode(false);
            //});

            modelBuilder.Entity<Faixa>(entity =>
            {
                entity.HasKey(e => e.IdFaixa)
                    .HasName("PK__FAIXA__848425F80ADD2FC2");

                entity.ToTable("FAIXA");

                entity.Property(e => e.IdFaixa).ValueGeneratedNever();

                entity.Property(e => e.NomeFaixa)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.IdItem);

                entity.ToTable("ITEM");

                entity.Property(e => e.IdItem).ValueGeneratedNever();

                entity.Property(e => e.Valor).HasColumnType("decimal(18, 2)");


                entity.Property(e => e.NomeItem)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ItemVenda>(entity =>
            {
                entity.HasKey(e => e.IdItemVenda)
                    .HasName("PK__ITEMVENDA__50FB6F5D88DB9488");



                entity.ToTable("ITEMVENDA");


                entity.Property(e => e.ValorUnitario).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ValorTotal).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DataVenda).HasColumnType("date");
                entity.Property(e => e.DataHoraVenda).HasColumnType("datetime");

                entity.HasOne(d => d.IdItemNavigation)
                    .WithMany(p => p.ItemVenda)
                    .HasForeignKey(d => d.IdItem)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ITEM_VENDA_ToItem");


                entity.HasOne(d => d.IdFormaPagamentoNavigation)
                    .WithMany(p => p.ItemVenda)
                    .HasForeignKey(d => d.IdFormaPagamento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ITEM_VENDA_ToFormaPgto");


            });


            modelBuilder.Entity<ItemDespesa>(entity =>
            {
                entity.HasKey(e => e.IdItemDespesa)
                    .HasName("PK__ITEMDESPESA__50FB6F5D88DB9488");



                entity.ToTable("ITEMDESPESA");


                entity.Property(e => e.ValorUnitario).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ValorTotal).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DataPagamento).HasColumnType("date");

                entity.HasOne(d => d.IdDespesaNavigation)
                    .WithMany(p => p.ItemDespesa)
                    .HasForeignKey(d => d.IdDespesa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ITEM_DESPESA_ToDespesa");


            });


            modelBuilder.Entity<FormaPagamento>(entity =>
            {
                entity.HasKey(e => e.IdFormaPagamento)
                    .HasName("PK__FORMAPAG__848425F80ADD2FC2");

                entity.ToTable("FORMAPAGAMENTO");

                entity.Property(e => e.IdFormaPagamento).ValueGeneratedNever();

                entity.Property(e => e.NomeFormaPagamento)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });


            modelBuilder.Entity<Mensalidade>(entity =>
            {
                entity.HasKey(e => e.IdMensalidade)
                    .HasName("PK__tmp_ms_x__420F6358528FBFE5");

                entity.ToTable("MENSALIDADE");

                entity.Property(e => e.DataPagamento).HasColumnType("date");

                entity.Property(e => e.Observacao)
                    .HasMaxLength(500)
                    .IsUnicode(false);


                entity.Property(e => e.QuitadoProfessor);


                entity.Property(e => e.Valor).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.ValorRepasseProfessor).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.IdAtletaNavigation)
                    .WithMany(p => p.Mensalidade)
                    .HasForeignKey(d => d.IdAtleta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MENSALIDADES_ToAtleta");

                entity.HasOne(d => d.IdModalidadeNavigation)
                    .WithMany(p => p.Mensalidade)
                    .HasForeignKey(d => d.IdModalidade)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MENSALIDADES_ToModalidade");

                //entity.HasOne(d => d.IdMesNavigation)
                //    .WithMany(p => p.Mensalidade)
                //    .HasForeignKey(d => d.IdMes)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_MENSALIDADES_ToMes");

                entity.HasOne(d => d.IdFormaPagamentoNavigation)
                    .WithMany(p => p.Mensalidade)
                    .HasForeignKey(d => d.IdFormaPagamento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MENSALIDADES_ToFormaPgto");

                entity.HasOne(d => d.IdProfessorNavigation)
                    .WithMany(p => p.Mensalidade)
                    .HasForeignKey(d => d.IdProfessor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MENSALIDADES_ToProfessor");

                entity.HasOne(d => d.IdMesNavigation)
                    .WithMany(p => p.Mensalidade)
                    .HasForeignKey(d => d.IdMes)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MENSALIDADES_ToMes");

            });

            modelBuilder.Entity<Mes>(entity =>
            {
                entity.HasKey(e => e.IdMes);

                entity.ToTable("MES");

                entity.Property(e => e.IdMes).ValueGeneratedNever();

                entity.Property(e => e.NomeMes)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });


            modelBuilder.Entity<Modalidade>(entity =>
            {
                entity.HasKey(e => e.IdModalidade)
                    .HasName("PK__tmp_ms_x__E103F01E2A48DB9B");

                entity.ToTable("MODALIDADE");

                entity.Property(e => e.NomeModalidade)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Professor>(entity =>
            {
                entity.HasKey(e => e.IdProfessor)
                    .HasName("PK__tmp_ms_x__9D84BE1B29E6C094");

                entity.ToTable("PROFESSOR");

                entity.Property(e => e.NomeProfessor)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProfessorModalidade>(entity =>
            {
                entity.HasKey(e => e.IdProfessorModalidade)
                    .HasName("PK__tmp_ms_x__DE23AA927711D120");

                entity.ToTable("PROFESSOR_MODALIDADE");

                entity.Property(e => e.DataOperacao).HasColumnType("date");

                entity.HasOne(d => d.IdModalidadeNavigation)
                    .WithMany(p => p.ProfessorModalidade)
                    .HasForeignKey(d => d.IdModalidade)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROFESSOR_MODALIDADE_Modalidade");

                entity.HasOne(d => d.IdProfessorNavigation)
                    .WithMany(p => p.ProfessorModalidade)
                    .HasForeignKey(d => d.IdProfessor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROFESSOR_MODALIDADE_Professor");
            });

            modelBuilder.Entity<Presenca>(entity =>
            {
                entity.HasKey(e => e.IdPresenca)
                    .HasName("PK__PRESENCA__50FB6F5D88DB9488");

                entity.ToTable("PRESENCA");

                entity.Property(e => e.DataPresenca).HasColumnType("date");
                entity.Property(e => e.IdTurno).HasDefaultValueSql("((1))");
                entity.Property(e => e.DataPresenca).HasColumnType("date");

                entity.HasOne(d => d.IdAtletaNavigation)
                    .WithMany(p => p.Presenca)
                    .HasForeignKey(d => d.IdAtleta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PRESENCA_ToAtleta");

                entity.HasOne(d => d.IdModalidadeNavigation)
                    .WithMany(p => p.Presenca)
                    .HasForeignKey(d => d.IdModalidade)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PRESENCA_ToModalidade");

                entity.HasOne(d => d.IdProfessorNavigation)
                    .WithMany(p => p.Presenca)
                    .HasForeignKey(d => d.IdProfessor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PRESENCA_ToProfessor");

                entity.HasOne(d => d.IdTurnoNavigation)
                    .WithMany(p => p.Presenca)
                    .HasForeignKey(d => d.IdTurno)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ATLETA_MODALIDADE_ToTurno");


            });

            modelBuilder.Entity<Turno>(entity =>
            {
                entity.HasKey(e => e.IdTurno)
                    .HasName("PK__TURNO__C1ECF79A75482C2A");

                entity.ToTable("TURNO");

                entity.Property(e => e.IdTurno).ValueGeneratedNever();

                entity.Property(e => e.NomeTurno)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });


            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PK__tmp_ms_x__5B65BF97D22B1BBA");

                entity.ToTable("USUARIO");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Senha)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
