using System;
using System.Collections.Generic;
using System.Linq;

namespace SIO.Domain.Extensions
{
    internal static class StringExtensions
    {
        public static IEnumerable<string> ChunkWithSentance(this string source, int maxChar)
        {
            var sourceSpan = source.AsSpan();
            var currentText = new char[maxChar];
            var items = new List<char[]>();

            var pointer = 0;
            var lazyPointer = 0;
            var currentIndex = 0;

            for (int i = 0; pointer <= sourceSpan.Length; i++, pointer++, currentIndex++)
            {
                if (currentIndex == maxChar - 1 || pointer == sourceSpan.Length)
                {
                    for (var j = maxChar - 1; j >= 0; j--)
                    {
                        var tempChar = currentText.AsSpan()[j];

                        if (tempChar == '.')
                        {
                            items.Add((char[])currentText.Clone());
                            Array.Clear(currentText, 0, currentText.Length);
                            pointer = j + 1 + lazyPointer;
                            lazyPointer = pointer;
                            currentIndex = -1;
                            break;
                        }

                        currentText[j] = ' ';
                    }
                }
                else
                    currentText[currentIndex] = sourceSpan[pointer];
            }

            return items.Select(i => new string(i).Trim());
        }
    }
}
