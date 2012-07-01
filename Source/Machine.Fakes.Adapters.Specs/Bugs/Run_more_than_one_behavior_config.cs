using Machine.Fakes.Adapters.Rhinomocks;
using Machine.Specifications;

namespace Machine.Fakes.Adapters.Specs.Bugs
{
    [Tags("Bug")]
    public class When_requesting_alex_from_contact_service : WithSubject<ContactService, RhinoFakeEngine>
    {
        static Contact _contact;
        static IContact _actualContact;

        Establish context = () =>
        {
            With<Alex>();
            With<AlexRepository>();
        };

        Because of = () => { _actualContact = Subject.GetContact(1); };

        It should_return_the_contact = () => _actualContact.ShouldEqual(The<IContact>());
    }

    public class AlexRepository
    {
        OnEstablish context = fakeAccessor =>
            fakeAccessor.The<IContactRepository>()
                .WhenToldTo(r => r.Get(1))
                .Return(fakeAccessor.The<IContact>);
    }

    public class Alex
    {
        OnEstablish context = fakeAccessor =>
        {
            fakeAccessor.The<IContact>().Id = 1;
            fakeAccessor.The<IContact>().Name = "Alex";
        };
    }

    public interface IContact
    {
        string Name { get; set; }
        int Id { get; set; }
    }

    public class Contact : IContact
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

    public class ContactService
    {
        readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public IContact GetContact(int id)
        {
            return _contactRepository.Get(id);
        }
    }

    public interface IContactRepository
    {
        IContact Get(int id);
    }
}