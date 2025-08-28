namespace Flashcards.mosnyan.Application.DTOs;

public record FlashcardStackDto(Guid Id, string Subject, IReadOnlyList<CardReadDto> Cards);