namespace SIO.Domain.Documents
{
    public enum DocumentCondition
    {
        Uploaded,
        TranslationQueued,
        TranslationStarted,
        TranslationSucceded,
        TranslationFailed,
        Deleted
    }
}
