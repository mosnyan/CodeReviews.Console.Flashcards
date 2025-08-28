using Flashcards.mosnyan.Domain.Models;

namespace Flashcards.mosnyan.Application.Abstractions.Repositories;

public interface IStackRepository
{
    public bool CreateNewStack(FlashcardStack flashcardStack);
    public IEnumerable<FlashcardStack> ReadAllStacks();
    public FlashcardStack? ReadStackById(Guid id);
    public bool UpdateStack(FlashcardStack flashcardStack);
    public bool DeleteStack(FlashcardStack flashcardStack);
    public bool DeleteStackById(Guid id);
}