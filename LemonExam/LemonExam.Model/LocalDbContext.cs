using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LemonExam.Model.Master;

namespace LemonExam.Model
{
    public class LocalDbContext : DbContext
    {
        #region Constructor

        /// <summary>
        ///  Add a constructor that receive a parameter of type DbContextOptions<SeatingOverlayDBContext>
        /// </summary>
        /// <param name="options"></param>
        public LocalDbContext(DbContextOptions<LocalDbContext> options) : base(options) { }

        #endregion

        #region Public Properties

        public UserAccount CurrentUser { get; set; }
        public UserSession CurrentSession { get; set; }

        public virtual DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<UserSession> Sessions { get; set; }
        public virtual DbSet<CategoryEntry> Categories { get; set; }
        public virtual DbSet<ProductEntry> Products { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(GetConnection.strDbContext, b => b.UseRowNumberForPaging());
        }
    }
}
