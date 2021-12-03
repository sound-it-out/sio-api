using SIO.Domain.Documents.Events;
using SIO.Domain.TranslationOptions.Events;
using SIO.Domain.Translations.Events;
using SIO.Domain.Users.Events;
using System;

namespace SIO.Domain
{
    public static class EventHelper
    {
        public static Type[] AllEvents = new Type[]
        {
            typeof(DocumentDeleted),
            typeof(DocumentUploaded),
            typeof(TranslationCharactersProcessed),
            typeof(TranslationFailed),
            typeof(TranslationQueued),
            typeof(TranslationStarted),
            typeof(TranslationSucceded),
            typeof(UserEmailChanged),
            typeof(UserLoggedIn),
            typeof(UserLoggedOut),
            typeof(UserPasswordTokenGenerated),
            typeof(UserPurchasedCharacterTokens),
            typeof(UserRegistered),
            typeof(UserVerified),
            typeof(TranslationOptionImported)
        };
    }
}
