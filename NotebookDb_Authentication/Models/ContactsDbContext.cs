using Microsoft.EntityFrameworkCore;

namespace _21_NotebookDb.Models
{
    public class ContactsDbContext : DbContext //IdentityDbContext<User>
    {
        public DbSet<Contact> Contacts { get; protected set; }
        public ContactsDbContext(DbContextOptions<ContactsDbContext> options) : base(options)
        {
            //устанавливаем с-во контекста и отключаем отслеживание изменений
            //(чтобы ef не ругался на использование  записей record Contact),
            //т.к. если отслеживание оставить, то он ругается в HomeModel в строке Update(контакта),
            //что он отслеживал данную запись, а теперь ему подсовывают такую же с другим id
            
            //можно делать не через record, а через класс, все работает и с отслеживанием,
            //но тогда нужно не забывать прописывать конструктор без параметров (иначе ругается)
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>().HasData(
                new Contact(
                    Guid.NewGuid(),
                    "Петров",
                    "Петр", 
                    "Петрович", 
                    "+79504112233", 
                    "Москва"),
                new Contact(
                    Guid.NewGuid(), 
                    "Сидоров", 
                    "Сидор", 
                    "Сидорович",
                    "+79504112244", 
                    "Урюпинск"));
        }
    }
}
