using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ApiEscapeRank.Modelos
{
    public partial class MySQLDbcontext : DbContext
    {

        public MySQLDbcontext()
        {
        }

        public MySQLDbcontext(DbContextOptions<MySQLDbcontext> options) : base(options)
        {
        }

        public virtual DbSet<Categoria> Categorias { get; set; }
        public virtual DbSet<Ciudad> Ciudades { get; set; }
        public virtual DbSet<Companyia> Companyias { get; set; }
        public virtual DbSet<Dificultad> Dificultades { get; set; }
        public virtual DbSet<Equipo> Equipos { get; set; }
        public virtual DbSet<EquiposUsuarios> EquiposUsuarios { get; set; }
        public virtual DbSet<Noticia> Noticias { get; set; }
        public virtual DbSet<Partida> Partidas { get; set; }
        public virtual DbSet<Perfil> Perfiles { get; set; }
        public virtual DbSet<Provincia> Provincias { get; set; }
        public virtual DbSet<Publico> Publico { get; set; }
        public virtual DbSet<Sala> Salas { get; set; }
        public virtual DbSet<SalasCategorias> SalasCategorias { get; set; }
        public virtual DbSet<SalasPublico> SalasPublico { get; set; }
        public virtual DbSet<SalasTematicas> SalasTematicas { get; set; }
        public virtual DbSet<Tematica> Tematicas { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<UsuariosAmigos> UsuariosAmigos { get; set; }
        public virtual DbSet<Valoracion> Valoraciones { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=localhost;user id=admin;password=1234;database=escaperank", x => x.ServerVersion("5.7.26-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.ToTable("categorias");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Tipo)
                    .HasColumnName("tipo")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Ciudad>(entity =>
            {
                entity.ToTable("ciudades");

                entity.HasIndex(e => e.ProvinciaId)
                    .HasName("fk_ciudades_provincias");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CiudadOrigen)
                    .HasColumnName("ciudad_origen")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Latitud)
                    .HasColumnName("latitud")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Longitud)
                    .HasColumnName("longitud")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ProvinciaId)
                    .HasColumnName("provincia_id")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.Provincia)
                    .WithMany(p => p.Ciudades)
                    .HasForeignKey(d => d.ProvinciaId)
                    .HasConstraintName("fk_Cities_Provinces1");
            });

            modelBuilder.Entity<Companyia>(entity =>
            {
                entity.ToTable("companyias");

                entity.HasIndex(e => e.CiudadId)
                    .HasName("fk_companyias_ciudades");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CiudadId)
                    .IsRequired()
                    .HasColumnName("ciudad_id")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CodigoPostal)
                    .HasColumnName("codigo_postal")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Direccion)
                    .HasColumnName("direccion")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Facebook)
                    .HasColumnName("facebook")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.GoogleMaps)
                    .HasColumnName("google_maps")
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Instagram)
                    .HasColumnName("instagram")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Latitud)
                    .HasColumnName("latitud")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Longitud)
                    .HasColumnName("longitud")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.NumeroLocal)
                    .HasColumnName("numero_local")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.NumeroOpiniones)
                    .HasColumnName("numero_opiniones")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Puntuacion)
                    .HasColumnName("puntuacion")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Rango)
                    .HasColumnName("rango")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Telefono)
                    .HasColumnName("telefono")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.TripAdvisor)
                    .HasColumnName("trip_advisor")
                    .HasColumnType("varchar(300)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Web)
                    .HasColumnName("web")
                    .HasColumnType("varchar(300)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.Ciudad)
                    .WithMany(p => p.Companyias)
                    .HasForeignKey(d => d.CiudadId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Companies_Cities1");
            });

            modelBuilder.Entity<Dificultad>(entity =>
            {
                entity.ToTable("dificultades");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Tipo)
                    .HasColumnName("tipo")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Equipo>(entity =>
            {
                entity.ToTable("equipos");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Avatar)
                    .HasColumnName("avatar")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<EquiposUsuarios>(entity =>
            {
                entity.HasKey(e => new { e.EquipoId, e.UsuarioId })
                    .HasName("PRIMARY");

                entity.ToTable("equipos_usuarios");

                entity.HasIndex(e => e.EquipoId)
                    .HasName("fk_equiposusuarios_usuarios");

                entity.HasIndex(e => e.UsuarioId)
                    .HasName("fk_equiposusuarios_equipos");

                entity.Property(e => e.EquipoId)
                    .HasColumnName("equipo_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UsuarioId)
                    .HasColumnName("usuario_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Equipo)
                    .WithMany(p => p.EquiposUsuarios)
                    .HasForeignKey(d => d.EquipoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_teams_has_usuarios_teams1");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.EquiposUsuarios)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_teams_has_usuarios_usuarios1");
            });

            modelBuilder.Entity<Noticia>(entity =>
            {
                entity.ToTable("noticias");

                entity.HasIndex(e => e.CompanyiaId)
                    .HasName("fk_noticias_companyias");

                entity.HasIndex(e => e.UsuarioId)
                    .HasName("fk_noticias_usuarios");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CompanyiaId)
                    .HasColumnName("companyia_id")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Imagen)
                    .HasColumnName("imagen")
                    .HasColumnType("varchar(2000)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Link)
                    .HasColumnName("link")
                    .HasColumnType("varchar(400)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.NumeroComentarios)
                    .HasColumnName("numero_comentarios")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NumeroFavoritos)
                    .HasColumnName("numero_favoritos")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Promocionada)
                    .HasColumnName("promocionada")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.TextoCorto)
                    .HasColumnName("texto_corto")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.TextoLargo)
                    .HasColumnName("texto_largo")
                    .HasColumnType("varchar(2000)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Titular)
                    .HasColumnName("titular")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.UsuarioId)
                    .HasColumnName("usuario_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Companyia)
                    .WithMany(p => p.Noticias)
                    .HasForeignKey(d => d.CompanyiaId)
                    .HasConstraintName("fk_news_companies1");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Noticias)
                    .HasForeignKey(d => d.UsuarioId)
                    .HasConstraintName("fk_news_usuarios1");
            });

            modelBuilder.Entity<Partida>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.SalaId, e.EquipoId })
                    .HasName("PRIMARY");

                entity.ToTable("partidas");

                entity.HasIndex(e => e.EquipoId)
                    .HasName("fk_partidas_equipos");

                entity.HasIndex(e => e.SalaId)
                    .HasName("fk_partidas_salas");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.SalaId)
                    .HasColumnName("sala_id")
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.EquipoId)
                    .HasColumnName("equipo_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Minutos)
                    .HasColumnName("minutos")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Segundos)
                    .HasColumnName("segundos")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.Equipo)
                    .WithMany(p => p.Partidas)
                    .HasForeignKey(d => d.EquipoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_matches_teams1");

                entity.HasOne(d => d.Sala)
                    .WithMany(p => p.Partidas)
                    .HasForeignKey(d => d.SalaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_matches_escape_rooms1");
            });

            modelBuilder.Entity<Perfil>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.UsuarioId })
                    .HasName("PRIMARY");

                entity.ToTable("perfiles");

                entity.HasIndex(e => e.UsuarioId)
                    .HasName("fk_perfiles_usuarios");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.UsuarioId)
                    .HasColumnName("usuario_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Avatar)
                    .HasColumnName("avatar")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.MejorTiempo)
                    .HasColumnName("mejor_tiempo")
                    .HasColumnType("datetime");

                entity.Property(e => e.Nick)
                    .HasColumnName("nick")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.NumeroPartidas)
                    .HasColumnName("numero_partidas")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PartidasGanadas)
                    .HasColumnName("partidas_ganadas")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PartidasPerdidas)
                    .HasColumnName("partidas_perdidas")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Usuario)
                 .WithOne(p => p.Perfil)
                    //    .WithMany(p => p.Perfiles)
                    //   .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_profile_usuarios1");
            });

            modelBuilder.Entity<Provincia>(entity =>
            {
                entity.ToTable("provincias");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Latitud)
                    .HasColumnName("latitud")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Longitud)
                    .HasColumnName("longitud")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Publico>(entity =>
            {
                entity.ToTable("publico");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Tipo)
                    .HasColumnName("tipo")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Sala>(entity =>
            {
                entity.ToTable("salas");

                entity.HasIndex(e => e.CompanyiaId)
                    .HasName("fk_salas_companyias");

                entity.HasIndex(e => e.DificultadId)
                    .HasName("fk_salas_dificultades");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.AdaptadoCiegos)
                    .HasColumnName("adaptado_ciegos")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.AdaptadoEmbarazadas)
                    .HasColumnName("adaptado_embarazadas")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.AdaptadoMinusvalidos)
                    .HasColumnName("adaptado_minusvalidos")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.AdaptadoSordos)
                    .HasColumnName("adaptado_sordos")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ComoReservar)
                    .HasColumnName("como_reservar")
                    .HasColumnType("varchar(2000)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CompanyiaId)
                    .IsRequired()
                    .HasColumnName("companyia_id")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Descripcion)
                    .HasColumnName("descripcion")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.DificultadId)
                    .HasColumnName("dificultad_id")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.DisponibleIngles)
                    .HasColumnName("disponible_ingles")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Duracion)
                    .HasColumnName("duracion")
                    .HasColumnType("varchar(11)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.EdadPublico)
                    .HasColumnName("edad_publico")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.EnOferta)
                    .HasColumnName("en_oferta")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ImagenAncha)
                    .HasColumnName("imagen_ancha")
                    .HasColumnType("varchar(400)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ImagenEstrecha)
                    .HasColumnName("imagen_estrecha")
                    .HasColumnType("varchar(400)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.JugadoresIncluidos)
                    .HasColumnName("jugadores_incluidos")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.MaximoJugadores)
                    .HasColumnName("maximo_jugadores")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MinimoJugadores)
                    .HasColumnName("minimo_jugadores")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ModoCombate)
                    .HasColumnName("modo_combate")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Promocionada)
                    .HasColumnName("promocionada")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NoClaustrofobicos)
                    .HasColumnName("no_claustrofobicos")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.NumeroResenyas)
                    .HasColumnName("numero_resenyas")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.OtrosDatos)
                    .HasColumnName("otros_datos")
                    .HasColumnType("varchar(2000)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.PrecioJugadorAdicional)
                    .HasColumnName("precio_jugador_adicional")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.PrecioMaximo)
                    .HasColumnName("precio_maximo")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.PrecioMinimo)
                    .HasColumnName("precio_minimo")
                    .HasColumnType("varchar(11)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Proximamente)
                    .HasColumnName("proximamente")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.RegaloBonus)
                    .HasColumnName("regalo_bonus")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.SalaIgual)
                    .HasColumnName("sala_igual")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.TerminosReserva)
                    .HasColumnName("terminos_reserva")
                    .HasColumnType("varchar(2000)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.TextoCombate)
                    .HasColumnName("texto_combate")
                    .HasColumnType("varchar(2000)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.TextoOferta)
                    .HasColumnName("texto_oferta")
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.UrlReserva)
                    .HasColumnName("url_reserva")
                    .HasColumnType("varchar(300)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Validez)
                    .HasColumnName("validez")
                    .HasColumnType("varchar(2000)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Visto)
                    .HasColumnName("visto")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.Companyia)
                    .WithMany(p => p.Salas)
                    .HasForeignKey(d => d.CompanyiaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Escape_Rooms_Companies1");

                entity.HasOne(d => d.Dificultad)
                    .WithMany(p => p.Salas)
                    .HasForeignKey(d => d.DificultadId)
                    .HasConstraintName("fk_Escape_Rooms_Difficulties1");
            });

            modelBuilder.Entity<SalasCategorias>(entity =>
            {
                entity.HasKey(e => new { e.SalaId, e.CategoriaId })
                    .HasName("PRIMARY");

                entity.ToTable("salas_categorias");

                entity.HasIndex(e => e.CategoriaId)
                    .HasName("fk_salascategorias_categorias");

                entity.HasIndex(e => e.SalaId)
                    .HasName("fk_salascategorias_salas");

                entity.Property(e => e.SalaId)
                    .HasColumnName("sala_id")
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CategoriaId)
                    .HasColumnName("categoria_id")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.Categoria)
                    .WithMany(p => p.SalasCategorias)
                    .HasForeignKey(d => d.CategoriaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Escape_Rooms_has_Categories_Categories1");

                entity.HasOne(d => d.Sala)
                    .WithMany(p => p.SalasCategorias)
                    .HasForeignKey(d => d.SalaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Escape_Rooms_has_Categories_Escape_Rooms1");
            });

            modelBuilder.Entity<SalasPublico>(entity =>
            {
                entity.HasKey(e => new { e.SalaId, e.PublicoId })
                    .HasName("PRIMARY");

                entity.ToTable("salas_publico");

                entity.HasIndex(e => e.PublicoId)
                    .HasName("fk_salaspublico_publico");

                entity.HasIndex(e => e.SalaId)
                    .HasName("fk_salaspublico_salas");

                entity.Property(e => e.SalaId)
                    .HasColumnName("sala_id")
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.PublicoId)
                    .HasColumnName("publico_id")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.Publico)
                    .WithMany(p => p.SalasPublico)
                    .HasForeignKey(d => d.PublicoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Escape_Rooms_has_Audience_Audience1");

                entity.HasOne(d => d.Sala)
                    .WithMany(p => p.SalasPublico)
                    .HasForeignKey(d => d.SalaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Escape_Rooms_has_Audience_Escape_Rooms1");
            });

            modelBuilder.Entity<SalasTematicas>(entity =>
            {
                entity.HasKey(e => new { e.SalaId, e.TematicaId })
                    .HasName("PRIMARY");

                entity.ToTable("salas_tematicas");

                entity.HasIndex(e => e.SalaId)
                    .HasName("fk_salastematicas_salas");

                entity.HasIndex(e => e.TematicaId)
                    .HasName("fk_salastematicas_tematicas");

                entity.Property(e => e.SalaId)
                    .HasColumnName("sala_id")
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.TematicaId)
                    .HasColumnName("tematica_id")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.Sala)
                    .WithMany(p => p.SalasTematicas)
                    .HasForeignKey(d => d.SalaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Escape_Rooms_has_Themes_Escape_Rooms1");

                entity.HasOne(d => d.Tematica)
                    .WithMany(p => p.SalasTematicas)
                    .HasForeignKey(d => d.TematicaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Escape_Rooms_has_Themes_Themes1");
            });

            modelBuilder.Entity<Tematica>(entity =>
            {
                entity.ToTable("tematicas");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Tipo)
                    .HasColumnName("tipo")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuarios");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Activado)
                    .HasColumnName("activado")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Avatar)
                    .HasColumnName("avatar")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CodigoActivado)
                    .HasColumnName("codigo_activado")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Contrasenya)
                    .IsRequired()
                    .HasColumnName("contrasenya")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
                

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Nacido)
                    .HasColumnName("nacido")
                    .HasColumnType("date");

                entity.Property(e => e.Telefono)
                    .HasColumnName("telefono")
                    .HasColumnType("varchar(15)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("usuario")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<UsuariosAmigos>(entity =>
            {
                entity.HasKey(e => new { e.UsuarioId, e.AmigoId })
                    .HasName("PRIMARY");

                entity.ToTable("usuarios_amigos");

                entity.HasIndex(e => e.AmigoId)
                    .HasName("fk_usuariosamigos_amigos");

                entity.HasIndex(e => e.UsuarioId)
                    .HasName("fk_usuariosamigos_usuarios");

                entity.Property(e => e.UsuarioId)
                    .HasColumnName("usuario_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AmigoId)
                    .HasColumnName("amigo_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Amigo)
                    .WithMany(p => p.UsuariosAmigosAmigo)
                    .HasForeignKey(d => d.AmigoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Usuarios_has_Usuarios_Usuarios1");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.UsuariosAmigosUsuario)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Usuarios_has_Usuarios_Usuarios");
            });

            modelBuilder.Entity<Valoracion>(entity =>
            {
                entity.ToTable("valoraciones");

                entity.HasIndex(e => e.SalaId)
                    .HasName("fk_valoraciones_salas");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Comentario)
                    .HasColumnName("comentario")
                    .HasColumnType("varchar(300)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Estrellas)
                    .HasColumnName("estrellas")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SalaId)
                    .IsRequired()
                    .HasColumnName("sala_id")
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.Sala)
                    .WithMany(p => p.Valoraciones)
                    .HasForeignKey(d => d.SalaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_valorations_escape_rooms1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
