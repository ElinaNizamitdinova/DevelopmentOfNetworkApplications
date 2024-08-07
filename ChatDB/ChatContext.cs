using ChatCommon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;


namespace ChatDB
{
    public class ChatContext: DbContext
    {
        public  ChatContext()
        {

        }

        public  ChatContext(DbContextOptions<ChatContext> dbc) : base(dbc)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=db; DataBase = BD; Integrated Security=False; TrustServerCertificate=True;Trusted_Connection=True ").UseLazyLoadingProxies();
        }
       protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>(entity =>
            {
                
                entity.HasKey(x => x.id).HasName("users_pkey");
                entity.ToTable("users");

                entity.HasIndex(x => x.FullName).IsUnique();

                entity.Property(e => e.FullName).HasColumnName("fullName").HasMaxLength(255).IsRequired();
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(x => x.MessageId).HasName("message_pk");
                entity.ToTable("message");

                entity.Property(e => e.Text)
                .HasColumnName("message_text");
                entity.Property(e => e.DateTime)
                .HasColumnName("data_time");
                entity.Property(e => e.IsSend)
                .HasColumnName("Is_send");
                entity.Property(e => e.MessageId)
                .HasColumnName("id");
                entity.HasOne(x => x.UserTo)
                .WithMany(x => x.MessagesTo).HasForeignKey(x=>x.UserToId).
                HasConstraintName("message_to_user_fk");
                entity.HasOne(x => x.UserFrom)
                .WithMany(x => x.MessagesFrom).HasForeignKey(x=>x.UserFromId)
                .HasConstraintName("message_from_user_fk");

            });
            base.OnModelCreating(modelBuilder);
        }
        
    }
}
