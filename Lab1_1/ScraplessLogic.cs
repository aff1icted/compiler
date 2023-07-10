using System;
using System.Collections.Generic;
using System.IO;

namespace Compiler
{
    class ScraplessLogic
    {
        int index = 0;
        private List<Word> Text = new List<Word>();
        private List<Word> analizedText = new List<Word>();
        private int markcount = 0;
        private Stack<Word> MarkandJump= new Stack<Word>();

        List<Word> expList = new List<Word>();

        public void Analize(List<Word> text) 
        {
            Text = text;
            
            while (Text[index].wordType!="begin")
            {
                index++;                
            }

            while (Text[index].wordType != "end")
            {
                index++;
                BodyAnalize();
            }            
            StreamWriter sw = new StreamWriter("test.txt");
            foreach (var item in analizedText)
            {
                sw.WriteLine(item.wordText);
            }
            sw.Close();
        }

        private void BodyAnalize()
        {
            while (Text[index].wordType != "end" && Text[index].wordType != "expression_end") 
            {
                if (Text[index].wordType == "if")
                {
                    ScraplessIf();
                }
                else
                {
                    if (Text[index].wordType == "while")
                    {
                        ScraplessWhile();
                    }
                    else
                    {
                        if (Text[index].wordType == "equals")
                        {
                            analizedText.Add(Text[index]);
                            index++;
                            while (Text[index].wordText !=";")
                            {
                                expList.Add(Text[index]);
                                index++;
                            }
                            
                            List<Word> PolishnotList = PolishNot(expList);
                            foreach (var pword in PolishnotList)
                            {
                                analizedText.Add(pword);
                            }
                            expList.Clear();
                            analizedText.Add(Text[index]);
                            index++;
                        }
                        else
                        {
                            analizedText.Add(Text[index]);
                            index++;
                        } 
                    }
                }
            }
                
        }

        private void ScraplessWhile()
        {
            index++;
            analizedText.Add(MarkGenerator());
            index++;
            var variable=Text[index];
            index++;
            var buf = Text[index];
            index++;


            while (Text[index].wordText != ";")
            {
                expList.Add(Text[index]);
                index++;
            }

            List<Word> PolishnotList = PolishNot(expList);
            foreach (var pword in PolishnotList)
            {
                analizedText.Add(pword);
            }
            expList.Clear();



            analizedText.Add(variable);
            analizedText.Add(JumpGenerator(buf.wordType,markcount));
            MarkandJump.Push(MarkGenerator());
            MarkandJump.Push(JumpGenerator("absolute",markcount-2));
            index++;
            index++;
            index++;
            BodyAnalize();
            analizedText.Add(MarkandJump.Pop());
            analizedText.Add(MarkandJump.Pop());
        }

        private void ScraplessIf()
        {
            index++;
            index++;
            var variable = Text[index];
            index++;
            var buf = Text[index];
            index++;

            while (Text[index].wordText != ";")
            {
                expList.Add(Text[index]);
                index++;
            }

            List<Word> PolishnotList = PolishNot(expList);
            foreach (var pword in PolishnotList)
            {
                analizedText.Add(pword);
            }
            expList.Clear();


            analizedText.Add(variable);

            analizedText.Add(JumpGenerator(buf.wordType, markcount+1));
            MarkandJump.Push(MarkGenerator());
            MarkandJump.Push(MarkGenerator());
            MarkandJump.Push(JumpGenerator("absolute", markcount - 2));
            index++;
            index++;
            index++;
            BodyAnalize();
            index++;
            if (Text[index].wordType == "else")
            {
                analizedText.Add(MarkandJump.Pop());
                analizedText.Add(MarkandJump.Pop());
                index++;
                index++;
                BodyAnalize();
                analizedText.Add(MarkandJump.Pop());
            }
            else
            {
                MarkandJump.Pop();
                analizedText.Add(MarkandJump.Pop());                
                MarkandJump.Pop();
                BodyAnalize();
            }
            
        }

        Word MarkGenerator() 
        {
            Word mark = new Word();
            mark.wordText = "#" + markcount;
            mark.wordType = "mark";
            markcount++;
            return mark;
            
        }

        Word JumpGenerator(string Type,int Target)
        {
            Word jump = new Word();
            if (Type == "same")
            {
                jump.wordType = "JNE";               
            }
            if (Type == "less")
            {           
                jump.wordType = "JGE";
            }
            if (Type == "greater")
            {               
                jump.wordType = "JLE";
            }
            if (Type == "greaterequal")
            {               
                jump.wordType = "JLT";
            }
            if (Type == "lessequal")
            {
                jump.wordType = "JGT";
            }
            if (Type == "notsame")
            {
                jump.wordType = "JEQ";
            }
            if (Type == "absolute")
            {
                jump.wordType = "JMP";
            }
            jump.wordText = "@" + Target;
            return jump;
        }

        private List<Word> PolishNot(List<Word> expression)
        {
            List<Word> newExpression = new List<Word>();
            Stack<Word> temp = new Stack<Word>();
            foreach (var word in expression)
            {
                if (word.wordType == "unary" || word.wordType == "binary" || word.wordType == "left_bracket" || word.wordType == "right_bracket")
                {
                    if (temp.Count == 0)
                    {
                        temp.Push(word);
                    }
                    else
                    {
                        bool check = true;
                        if (word.wordType == "right_bracket")
                        {
                            while (check)
                            {
                                if (temp.Peek().wordType == "left_bracket")
                                {
                                    temp.Pop();
                                    check = false;
                                }
                                else
                                {
                                    newExpression.Add(temp.Pop());
                                }
                            }
                        }
                        else
                        {
                            while (check)
                            {
                                if (word.wordText == "(")
                                {
                                    temp.Push(word);
                                    check = false;
                                }
                                else
                                {
                                    if (GetWeight(word) > GetWeight(temp.Peek()))
                                    {
                                        temp.Push(word);
                                        check = false;
                                    }
                                    else
                                    {
                                        newExpression.Add(temp.Pop());
                                        if (temp.Count == 0)
                                        {
                                            temp.Push(word);
                                            check = false;
                                        }
                                    }
                                }

                            }
                        }

                    }
                }
                else
                {
                    newExpression.Add(word);
                }
            }
            while (temp.Count != 0)
            {
                newExpression.Add(temp.Pop());
            }
            return newExpression;
        }

        private int GetWeight(Word word)
        {
            if (word.wordType == "unary")
            {
                return 3;
            }

            if (word.wordText == "+" || word.wordText == "-")
            {
                return 1;
            }

            if (word.wordText == "*" || word.wordText == "/")
            {
                return 2;
            }

            if (word.wordText == "(")
            {
                return 0;
            }

            return 0;
        }

        public List<Word> GetScraplessText()
        {
            return analizedText;
        }
    }


}




