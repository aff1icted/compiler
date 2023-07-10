using System;
using System.Collections.Generic;
using System.IO;

namespace Compiler
{
    class CodeGenerator
    {

        List<string> code = new List<string>();
        int LineCount = 0;
        List<Mark> MarkList = new List<Mark>();
        List<string> JumpList = new List<string>() { "JMP", "JEQ", "JLT", "JLE", "JGT", "JGE","JNE" };

        struct Mark
        {
            public int line;
            public string number;

            public Mark(int line, string number)
            {
                this.line = line;
                this.number = number;
            }
        }    

        public void CodeGen(List<Word> text, List<string> variableList)
        {
            string variable = "";
            int index = 0;
            foreach (var word in text)
            {

                if (word.wordType == "some_letters")
                {
                    if (text[index + 1].wordText == "=")
                    {
                        variable = GetVariableInd(word.wordText, variableList);
                    }
                    else
                    {
                        code.Add("LOAD " + GetVariableInd(word.wordText, variableList));
                        LineCount++;
                    }
                    
                }


                if (word.wordType == "unary")
                {
                    code.Add("NOT");
                    LineCount++;
                }

                if (word.wordType == "binary" && word.wordText == "-")
                {
                    code.Add("SUB");
                    LineCount++;
                }

                if (word.wordType == "number")
                {
                    code.Add("LIT " + word.wordText);
                    LineCount++;
                }

                if (word.wordText == "+")
                {
                    code.Add("ADD");
                    LineCount++;
                }
                if (word.wordText == "*")
                {
                    code.Add("MUL");
                    LineCount++;
                }
                if (word.wordText == "/")
                {
                    code.Add("DIV");
                    LineCount++;
                }
                if (word.wordText == ";")
                {
                    code.Add("STO " + variable);
                    LineCount++;
                }

                if (word.wordType == "mark")
                {
                    code.Add("NOP");
                    MarkList.Add(new Mark
                    {
                        line = LineCount,
                        number = word.wordText.Substring(1)
                    });
                    LineCount++;
                }

                if (JumpList.Contains(word.wordType))
                {
                    code.Add(word.wordType+" "+word.wordText);                    
                    LineCount++;
                }

                index++;
            }
            LinkReplace();
            SeeCode();
            WriteCode();
        }


        private void LinkReplace()
        {
            for (int i = 0; i < code.Count; i++)
            {
                if (code[i].Contains("@"))
                {
                    bool notfound = true;
                    int j = 0;
                    while (notfound)
                    {
                        if (MarkList[j].number == code[i].Substring(5))
                        {
                            code[i] = code[i].Substring(0, 4) + MarkList[j].line;
                            notfound = false;
                        }
                        j++;
                    }
                }
            }
                
            
        }

        private string GetVariableInd(string var, List<string> variableList)
        {
            return variableList.IndexOf(var).ToString();
        }

        private void SeeCode()
        {
            Console.WriteLine();
            foreach (var item in code)
            {
                Console.WriteLine(item);
            }
        }

        private void WriteCode()
        {
            StreamWriter sw = new StreamWriter("code.cod");
            foreach (var item in code)
            {
                sw.WriteLine(item);
            }
            sw.Close();
        }
    }


}




