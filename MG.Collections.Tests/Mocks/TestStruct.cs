using System;

namespace MG.Collections.Mocks
{
    public interface IStruct
    {
        int Id { get; }
        Guid UniqueId { get; }
        string Value { get; }
    }

    internal struct TestStruct : IStruct
    {
        public int Id { get; }
        public Guid UniqueId { get; }
        public string Value { get; }
    }
}
