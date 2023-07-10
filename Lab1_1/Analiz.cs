using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Compiler
{
    public struct Word
    {
        public int lineNumb;
        public string wordText;
        public string wordType;

        public Word(int line, string word, string type)
        {
            this.lineNumb = line;
            this.wordText = word;
            this.wordType = type;
        }
    }
    class Analiz
    {
        public void Analize()
        {
            LecsicAnalize lecsic = new LecsicAnalize();
            Console.WriteLine("lexis analiz...");
            lecsic.LecsicAnaliz();
            if (lecsic.LecsicError())
            {
                return;
            }
            lecsic.PrintLecsicList();
            Console.WriteLine("lexis analiz complete");
            Console.WriteLine("syntax analiz...");
            SyntaxAnaliz syntax = new SyntaxAnaliz();
            if (!syntax.Analiz(lecsic.GetAnalizedText()))
            {
                return;
            }
            Console.WriteLine("scrapless generation...");
            ScraplessLogic scraplessLogic = new ScraplessLogic();
            scraplessLogic.Analize(syntax.GetAnalizedText());
            Console.WriteLine("scrapless generation complete");
            CodeGenerator codeGenerator = new CodeGenerator();
            codeGenerator.CodeGen(scraplessLogic.GetScraplessText(),syntax.GetVariable());


        }   

    }


}




