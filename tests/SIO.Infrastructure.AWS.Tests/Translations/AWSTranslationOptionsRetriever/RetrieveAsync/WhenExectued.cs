using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Internal;
using SIO.Infrastructure.AWS.Tests.Translations.AWSTranslationOptionsRetriever;
using SIO.Infrastructure.Translations;
using SIO.Testing.Attributes;
using System.Linq;
using Amazon.Polly;

namespace SIO.Infrastructure.AWS.Tests.Translations.AWSTranslationOptionsRetriever.RetrieveAsync
{
    public class WhenExectued : AWSTranslationOptionsRetrieverSpecification
    {
        protected override async Task<IEnumerable<TranslationOption>> Given()
        {
            return await TranslationOptionsRetriever.RetrieveAsync();
        }

        protected override Task When()
        {
            return Task.CompletedTask;
        }

        [Then]
        public void ResultShouldNotBeEmpty()
        {
            Result.Should().NotBeEmpty();
        }

        [Then]
        public void ResultShouldOnlyContainAWSTranslationOptions()
        {
            Result.All(to => to.Type == TranslationType.AWS).Should().BeTrue();
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForAditi()
        {
            Result.Count(to => to.Subject == VoiceId.Aditi && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForLotte()
        {
            Result.Count(to => to.Subject == VoiceId.Lotte && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForLucia()
        {
            Result.Count(to => to.Subject == VoiceId.Lucia && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForLupe()
        {
            Result.Count(to => to.Subject == VoiceId.Lupe && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForMads()
        {
            Result.Count(to => to.Subject == VoiceId.Mads && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForMaja()
        {
            Result.Count(to => to.Subject == VoiceId.Maja && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForMarlene()
        {
            Result.Count(to => to.Subject == VoiceId.Marlene && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForMathieu()
        {
            Result.Count(to => to.Subject == VoiceId.Mathieu && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForMatthew()
        {
            Result.Count(to => to.Subject == VoiceId.Matthew && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForMaxim()
        {
            Result.Count(to => to.Subject == VoiceId.Maxim && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForMia()
        {
            Result.Count(to => to.Subject == VoiceId.Mia && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForMiguel()
        {
            Result.Count(to => to.Subject == VoiceId.Miguel && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForMizuki()
        {
            Result.Count(to => to.Subject == VoiceId.Mizuki && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForNaja()
        {
            Result.Count(to => to.Subject == VoiceId.Naja && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForNicole()
        {
            Result.Count(to => to.Subject == VoiceId.Nicole && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForPenelope()
        {
            Result.Count(to => to.Subject == VoiceId.Penelope && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForRaveena()
        {
            Result.Count(to => to.Subject == VoiceId.Raveena && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForRicardo()
        {
            Result.Count(to => to.Subject == VoiceId.Ricardo && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForRuben()
        {
            Result.Count(to => to.Subject == VoiceId.Ruben && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForRussell()
        {
            Result.Count(to => to.Subject == VoiceId.Russell && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForSalli()
        {
            Result.Count(to => to.Subject == VoiceId.Salli && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForSeoyeon()
        {
            Result.Count(to => to.Subject == VoiceId.Seoyeon && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForTakumi()
        {
            Result.Count(to => to.Subject == VoiceId.Takumi && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForTatyana()
        {
            Result.Count(to => to.Subject == VoiceId.Tatyana && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForVicki()
        {
            Result.Count(to => to.Subject == VoiceId.Vicki && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForVitoria()
        {
            Result.Count(to => to.Subject == VoiceId.Vitoria && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForZeina()
        {
            Result.Count(to => to.Subject == VoiceId.Zeina && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForZhiyu()
        {
            Result.Count(to => to.Subject == VoiceId.Zhiyu && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForLiv()
        {
            Result.Count(to => to.Subject == VoiceId.Liv && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForKimberly()
        {
            Result.Count(to => to.Subject == VoiceId.Kimberly && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForLea()
        {
            Result.Count(to => to.Subject == VoiceId.Lea && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForEnrique()
        {
            Result.Count(to => to.Subject == VoiceId.Enrique && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForAstrid()
        {
            Result.Count(to => to.Subject == VoiceId.Astrid && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForBianca()
        {
            Result.Count(to => to.Subject == VoiceId.Bianca && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForBrian()
        {
            Result.Count(to => to.Subject == VoiceId.Brian && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForCamila()
        {
            Result.Count(to => to.Subject == VoiceId.Camila && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForCarla()
        {
            Result.Count(to => to.Subject == VoiceId.Carla && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForCarmen()
        {
            Result.Count(to => to.Subject == VoiceId.Carmen && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForCeline()
        {
            Result.Count(to => to.Subject == VoiceId.Celine && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForChantal()
        {
            Result.Count(to => to.Subject == VoiceId.Chantal && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForConchita()
        {
            Result.Count(to => to.Subject == VoiceId.Conchita && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForCristiano()
        {
            Result.Count(to => to.Subject == VoiceId.Cristiano && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForDora()
        {
            Result.Count(to => to.Subject == VoiceId.Dora && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForEmma()
        {
            Result.Count(to => to.Subject == VoiceId.Emma && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForKendra()
        {
            Result.Count(to => to.Subject == VoiceId.Kendra && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForEwa()
        {
            Result.Count(to => to.Subject == VoiceId.Ewa && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForFiliz()
        {
            Result.Count(to => to.Subject == VoiceId.Filiz && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForGeraint()
        {
            Result.Count(to => to.Subject == VoiceId.Geraint && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForGiorgio()
        {
            Result.Count(to => to.Subject == VoiceId.Giorgio && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForGwyneth()
        {
            Result.Count(to => to.Subject == VoiceId.Gwyneth && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForHans()
        {
            Result.Count(to => to.Subject == VoiceId.Hans && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForInes()
        {
            Result.Count(to => to.Subject == VoiceId.Ines && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForIvy()
        {
            Result.Count(to => to.Subject == VoiceId.Ivy && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForJacek()
        {
            Result.Count(to => to.Subject == VoiceId.Jacek && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForJan()
        {
            Result.Count(to => to.Subject == VoiceId.Jan && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForJoanna()
        {
            Result.Count(to => to.Subject == VoiceId.Joanna && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForJoey()
        {
            Result.Count(to => to.Subject == VoiceId.Joey && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForJustin()
        {
            Result.Count(to => to.Subject == VoiceId.Justin && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForKarl()
        {
            Result.Count(to => to.Subject == VoiceId.Karl && to.Type == TranslationType.AWS).Should().Be(1);
        }

        [Then]
        public void ResultShouldContainASingleTranslationOptionForAmy()
        {
            Result.Count(to => to.Subject == VoiceId.Amy && to.Type == TranslationType.AWS).Should().Be(1);
        }
    }
}
