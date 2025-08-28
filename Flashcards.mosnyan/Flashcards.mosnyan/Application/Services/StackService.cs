using System.Collections;
using Flashcards.mosnyan.Application.Abstractions.Repositories;
using Flashcards.mosnyan.Application.DTOs;
using Flashcards.mosnyan.Domain.Models;

namespace Flashcards.mosnyan.Application.Services;

public class StackService(IStackRepository repo)
{
    public bool CreateStack(FlashcardStackCreationDto dto)
    {
        var stack = new FlashcardStack(dto.Subject);
        return repo.CreateNewStack(stack);
    }

    public FlashcardStackDto? ReadStackById(Guid id)
    {
        FlashcardStack? stack = repo.ReadStackById(id);

        if (stack is null)
        {
            return null;
        }

        return new FlashcardStackDto(
            stack.Id,
            stack.Subject,
            stack.GetCards()
                .Select(card => new CardReadDto(card.Prompt, card.Answer))
                .ToList()
            );
    }

    public IEnumerable<FlashcardStackDto> ReadAllStacks()
    {
        return repo.ReadAllStacks()
            .Select(stack => new FlashcardStackDto(
                    stack.Id,
                    stack.Subject,
                    stack.GetCards()
                        .Select(card => new CardReadDto(card.Prompt, card.Answer))
                        .ToList()
                )
            );
    }

    public bool UpdateStack(FlashcardStackDto dto)
    {
        return repo.UpdateStack(new FlashcardStack(dto.Id, dto.Subject, dto.Cards
                .Select(card => new Flashcard(card.Prompt, card.Answer, dto.Id))
                .ToList()
            )
        );
    }

    public bool DeleteStack(FlashcardStackDto dto)
    {
        return repo.DeleteStackById(dto.Id);
    }

    public bool DeleteStackById(Guid id)
    {
        return repo.DeleteStackById(id);
    }
}