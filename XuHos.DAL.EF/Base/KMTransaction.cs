using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DAL.EF
{
    public class KMTransaction : IDisposable
    {
        [ThreadStatic]
        private static KMTransaction current;
        public static KMTransaction Current
        {
            get { return current; }
        }

        private DBEntities db;
        private DbContextTransaction tran;
        

        internal KMTransaction(DBEntities db, DbContextTransaction tran)
        {
            this.db = db;
            this.tran = tran;
            current = this;
        }

        public void Commit()
        {
            if(this.tran != null)
            {
                this.tran.Commit();
                if (this.db != null) this.db.transaction = null;
                this.tran = null;
                this.db = null;
            }
            current = null;
        }

        public void Rollback()
        {
            if (this.tran != null)
            {
                this.tran.Rollback();
                if (this.db != null) this.db.transaction = null;
                this.tran = null;
                this.db = null;
            }
            current = null;
        }

        public void Dispose()
        {
            this.Rollback();
        }
    }
}
