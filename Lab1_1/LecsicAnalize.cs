using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Compiler
{
    class LecsicAnalize
    {
        private List<Word> analizedText = new List<Word>();
        List<string> logicSymbol = new List<string>() { "same", "less", "greater", "greaterequal", "lessequal", "notsame" };

        public bool LecsicError() 
        {
            bool error = false;
            foreach (var word in analizedText)
            {
                if (word.wordType == "indefinite_word")
                {
                    Console.WriteLine("Line: " + word.lineNumb + ", " + word.wordText + " unidentified element");
                    error = true;
                }
            }
            return error;
        }
        private string DetectType(string word)
        {
            if (word == "var")
            {
                return "var";
            }
            if (word == ";")
            {
                return "semicolon";
            }
            if (word == ",")
            {
                return "comma";
            }
            if (word == "begin")
            {
                return "begin";
            }
            if (word == "end")
            {
                return "end";
            }

            if (word == "if")
            {
                return "if";
            }
            if (word == "else")
            {
                return "else";
            }
            if (word == "while")
            {
                return "while";
            }


            if (word == "=")
            {
                return "equals";
            }
            if (word == "+" || word == "*" || word == "/")
            {
                return "binary";
            }
            if (word == "-")
            {
                if (analizedText[analizedText.Count - 1].wordType == "left_bracket" || analizedText[analizedText.Count - 1].wordType == "equals" || logicSymbol.Contains(analizedText[analizedText.Count - 1].wordType))
                    return "unary";
                else
                    return "binary";
            }
            if (word == "(")
            {
                return "left_bracket";
            }
            if (word == ")")
            {
                return "right_bracket";
            }
            if (word == ":integer")
            {
                return "type_declaration";
            }



            if (word == "<")
            {
                return "less";
            }
            if (word == ">")
            {
                return "greater";
            }
            if (word == "<=")
            {
                return "lessequal";
            }
            if (word == ">=")
            {
                return "greaterequal";
            }
            if (word == "==")
            {
                return "same";
            }
            if (word == "<>")
            {
                return "notsame";
            }

            if (word == "{")
            {
                return "expression_begin";
            }
            if (word == "}")
            {
                return "expression_end";
            }

            if (Regex.IsMatch(word, @"^[a-z]+$"))
            {
                return "some_letters";
            }
            if (Regex.IsMatch(word, @"^[0-9]+$"))
            {
                return "number";
            }
            return "indefinite_word";
        }
        public void LecsicAnaliz()
        {
            try
            {
                StreamReader sr = new StreamReader("Text.txt");
                string line = sr.ReadLine();
                int lineNum = 0;
                while (line != null)
                {
                    line = line.Replace(",", " , ");
                    line = line.Replace("+", " + ");
                    line = line.Replace("-", " - ");
                    line = line.Replace("*", " * ");
                    line = line.Replace("/", " / ");
                    line = line.Replace("(", " ( ");
                    line = line.Replace(")", " ) ");
                    line = line.Replace(";", " ; ");
                    line = line.Replace(":", " :");
                    line = line.Replace("{", " { ");
                    line = line.Replace("}", " } ");
                    string[] sline = line.ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    foreach (string word in sline)
                    {
                        analizedText.Add(new Word(lineNum, word, DetectType(word)));                                          
                    }
                    lineNum++;
                    line = sr.ReadLine();
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message, "file read error");
            }
        }

        public List<Word> GetAnalizedText()
        {
            return analizedText;
        }

        public void PrintLecsicList() 
        {
            foreach (var word in analizedText)
            {
                Console.WriteLine("Line: " + word.lineNumb + " Word: " + word.wordText + " Type: " + word.wordType);
            }
        }
    }


}




