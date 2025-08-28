using Flashcards.mosnyan.Domain.Models;

namespace Flashcards.mosnyan.Application.Abstractions.Repositories;

public interface ISessionRepository
{
    public bool CreateNewSession(Session session);
    public IEnumerable<Session> ReadAllSessions();
}