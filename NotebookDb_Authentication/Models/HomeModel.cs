using Microsoft.EntityFrameworkCore;

namespace _21_NotebookDb.Models
{
    public record HomeModel(ContactsDbContext Context)
    {
        public IReadOnlyCollection<Contact> Contacts = new List<Contact> { };

        public async Task UpdateContactsAsync() => 
            Contacts = await Context.Contacts.ToListAsync();

        public async Task<Contact> GetContactByIdAsync(Guid id) =>
            await Context.Contacts.FirstAsync(contact => contact.Id == id);

        public async Task AddAsync(Contact contact)
        {
            Context.Contacts.Add(contact);
            await Context.SaveChangesAsync();
        }

        public async Task ChangeAsync(Contact newDataofChangingContact)
        {
            Context.Contacts.Update(newDataofChangingContact);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var deletingContact = Context.Contacts.FirstOrDefault((contact) => contact.Id == id);
            if (deletingContact is null) return;
            Context.Contacts.Remove(deletingContact);
            await Context.SaveChangesAsync();
        }

    }
}
