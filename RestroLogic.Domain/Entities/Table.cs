using RestroLogic.Domain.Enums;
using RestroLogic.Domain.Exceptions;

namespace RestroLogic.Domain.Entities
{
    public sealed class Table
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Code { get; private set; } // e.g., "M12"
        public int? Capacity { get; private set; }
        public TableStatus Status { get; private set; } = TableStatus.Available;

        private Table() { }

        public Table(string code, int? capacity = null)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Table code is required.");
            Code = code.Trim().ToUpperInvariant();
            Capacity = capacity;
        }

        public bool IsAvailable => Status == TableStatus.Available;

        public void MarkAvailable() => Status = TableStatus.Available;

        public void MarkOccupied()
        {
            if (Status == TableStatus.Blocked)
                throw new DomainException("Blocked table cannot be occupied.");
            Status = TableStatus.Occupied;
        }

        public void Block() => Status = TableStatus.Blocked;

        public void MarkCleaning() => Status = TableStatus.Cleaning;
    }
}
