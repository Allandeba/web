using getQuote.Models;

namespace getQuote
{
    public class PersonBusiness
    {
        private readonly PersonRepository _repository;

        public PersonBusiness(PersonRepository personRepository)
        {
            _repository = personRepository;
        }

        public async Task<IEnumerable<PersonModel>> GetPeople()
        {
            IEnumerable<PersonModel> people = await _repository.GetAllAsync();
            return people.OrderByDescending(p => p.PersonId);
        }

        public async Task AddAsync(PersonModel person)
        {
            await _repository.AddAsync(person);
        }

        public async Task<PersonModel> GetByIdAsync(int personId)
        {
            return await _repository.GetByIdAsync(personId);
        }

        public async Task UpdateAsync(PersonModel person)
        {
            PersonModel? existentPerson = await _repository.GetByIdAsync(person.PersonId);

            if (existentPerson == null)
            {
                return;
            }

            existentPerson.Contact = person.Contact;
            existentPerson.Document = person.Document;
            existentPerson.FirstName = person.FirstName;
            existentPerson.LastName = person.LastName;

            await _repository.UpdateAsync(existentPerson);
        }

        public async Task RemoveAsync(int personId)
        {
            PersonModel? person = await _repository.GetByIdAsync(personId);
            if (person == null)
            {
                return;
            }
            ;

            await _repository.RemoveAsync(person);
        }
    }
}
