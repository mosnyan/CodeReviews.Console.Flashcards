using Flashcards.mosnyan.Domain.Models;

namespace Flashcards.mosnyan.Application.Abstractions.Repositories;

public interface IStackRepository
{
    public bool CreateNewStack(Stack stack);
    public IEnumerable<Stack> ReadAllStacks();
    public Stack? ReadStackById(Guid id);
    public bool UpdateStack(Stack stack);
    public bool DeleteStack(Stack stack);
    public bool DeleteStackById(Guid id);
}