using RestroLogic.Domain.ValueObjects;

namespace RestroLogic.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Email Email { get; private set; }
        public string Phone {  get; private set; }

        protected Customer() { }

        public Customer(string firstName, string lastName, Email email, string phone)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
        }
    }
}
