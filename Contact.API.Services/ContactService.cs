using Contact.API.Infrastructure;
using Contact.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Contact.API.Services
{
    public class ContactService : IContactService
    {
        public static List<ContactModel> AllContacts { get; set; } = new List<ContactModel>
        {
            new ContactModel
            {
                Id=new Random().Next(1,10000),
                Firstname="Aysel",
                Lastname="Elizade"
            },
            new ContactModel
            {
                Id=new Random().Next(1,10000),
                Firstname="Tural",
                Lastname="Mammadov"
            },
            new ContactModel
            {
                Id=new Random().Next(1,10000),
                Firstname="Eli",
                Lastname="Eliyev"
            }
        };

        public ContactModel Add(ContactModel model)
        {
            model.Id = new Random().Next(1, 10000);
            AllContacts.Add(model);
            return model;
        }

        public bool Delete(int id)
        {
            var item=AllContacts.FirstOrDefault(x => x.Id == id);
            if (item!=null)
            {
                return AllContacts.Remove(item);
            }
            return false;
        }

        public List<ContactModel> GetAll()
        {
            return AllContacts;
        }

        public ContactModel? GetContactById(int id)
        {
            return AllContacts.FirstOrDefault(c => c.Id == id);
        }

        public ContactModel Update(int id, ContactModel model)
        {
            var contact = GetContactById(id);
            if(contact!=null)
            {
                contact.Firstname=model.Firstname;
                contact.Lastname=model.Lastname;
                return contact;
            }
            return null!;
        }
    }
}
