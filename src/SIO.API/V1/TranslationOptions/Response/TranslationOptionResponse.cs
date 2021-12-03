﻿using SIO.Domain.Documents.Events;

namespace SIO.Api.V1.TranslationOptions.Response
{
    public record TranslationOptionResponse(string Id, string Subject, TranslationType TranslationType);
}
