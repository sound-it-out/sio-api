namespace SIO.Domain.Documents.Aggregates
{
    public enum TranslationProgress
    {
        NotStarted,
        Queued,
        Started,
        InProgress,
        Completed,
        Failed
    }
}