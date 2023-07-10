using System;
using System.Collections.Generic;

namespace Compiler
{
    class SyntaxAnaliz
    {
        private List<Word> analizedText = new List<Word>();
        List<string> variable = new List<string>();
        List<string> logicSymbol = new List<string>() { "same", "less", "greater", "greaterequal", "lessequal", "notsame" };
        int index = 0;


        private void ErrorMessage(string expected)
        {
            Console.WriteLine("Line " + Convert.ToString(analizedText[index].lineNumb) + " expectation:  \"" + expected + "\" was expected,  \"" + analizedText[index].wordText + "\" was received");
        }

        public bool Analiz(List<Word> text)
        {
            analizedText = text;
            if (analizedText[index].wordType == "var")
            {
                index++;
                if (!VariablesList())
                {
                    return false;
                }
                index++;
                if (analizedText[index].wordType == "semicolon")
                {
                    index++;
                    if (analizedText[index].wordType == "begin")
                    {
                        index++;
                        while (analizedText[index].wordType != "end")
                        {
                            if (!Operator())
                            {
                                return false;
                            }
                            index++;
                        }
                        Console.WriteLine("syntax analiz complete");
                        return true;
                    }
                    else
                    {
                        ErrorMessage("begin");
                        return false;
                    }
                }
                else
                {
                    ErrorMessage(";");
                    return false;
                }
            }
            else
            {
                ErrorMessage("var");
                return false;
            }
        }
        private bool VariablesList()
        {
            if (analizedText[index].wordType == "some_letters")
            {
                variable.Add(analizedText[index].wordText);
                index++;
                if (analizedText[index].wordType == "type_declaration")
                {
                    return true;
                }
                if (analizedText[index].wordType == "comma")
                {
                    index++;
                    if (VariablesList())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                ErrorMessage(":integer or ,");
                return false;
            }
            else
            {
                ErrorMessage("variable");
                return false;
            }
        }

        private bool Assignment()
        {
            if (variable.Contains(analizedText[index].wordText))
            {
                index++;
                if (analizedText[index].wordType == "equals")
                {
                    if (!Expression())
                    {
                        return false;
                    }
                    if (analizedText[index].wordType == "semicolon")
                    {
                        return true;
                    }
                    else
                    {
                        ErrorMessage(";");
                        return false;
                    }
                }
                else
                {
                    ErrorMessage("=");
                    return false;
                }
            }
            else
            {
                ErrorMessage("variable");
                return false;
            }
        }

        private bool Expression()
        {
            index++;
            if (analizedText[index].wordType == "unary")
            {
                index++;
            }
            if (!Subexpression())
            {
                return false;
            }
            return true;
        }

        private bool Subexpression()
        {
            if (analizedText[index].wordType == "left_bracket")
            {
                if (!Expression())
                {
                    return false;
                }
                if (analizedText[index].wordType == "right_bracket")
                {
                    index++;
                    if (analizedText[index].wordType == "binary")
                    {
                        index++;
                        if (!Subexpression())
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    ErrorMessage(")");
                    return false;
                }
            }
            else
            {
                if (analizedText[index].wordType == "number" || variable.Contains(analizedText[index].wordText))
                {
                    index++;
                    if (analizedText[index].wordType == "binary")
                    {
                        index++;
                        if (!Subexpression())
                        {
                            return false;
                        }

                    }
                    return true;
                }
                else
                {
                    ErrorMessage("( , number or variable");
                    return false;
                }
            }
        }


        private bool Operator()
        {
            if (analizedText[index].wordType == "if")
            {
                if (!Branching())
                {
                    return false;
                }
            }
            else
            {
                if (analizedText[index].wordType == "while")
                {                   
                    if (!WhileLoop())
                    {
                        return false;
                    }
                }
                else
                {
                    if (!Assignment())
                    {
                        return false;
                    }
                }
            }  
            return true;
        }

        private bool WhileLoop()
        {
            index++;
            if (BolleanExpressin())
            {
                index++;
                if (analizedText[index].wordType == "expression_begin")
                {
                    index++;
                    while (analizedText[index].wordType != "expression_end")
                    {
                        if (!Operator())
                        {
                            return false;
                        }
                        index++;
                    }
                }
                else
                {
                    ErrorMessage("{");
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        private bool BolleanExpressin()
        {
            if (analizedText[index].wordType == "left_bracket")
            {
                index++;
                if (variable.Contains(analizedText[index].wordText))
                {
                   
                        index++;
                        if (logicSymbol.Contains(analizedText[index].wordType))
                        {

                            if (!Expression())
                            {
                                return false;
                            }

                            if (analizedText[index].wordType == "semicolon")
                            {
                                index++;
                                if (analizedText[index].wordType == "right_bracket")
                                {
                                    return true;
                                }
                                else
                                {
                                    ErrorMessage(")");
                                    return false;
                                }
                            }
                            else
                            {
                                ErrorMessage(";");
                                return false;
                            }

                            //    index++;
                            //    if (analizedText[index].wordType == "unary")
                            //    {
                            //        index++;
                            //    }
                            //    if (analizedText[index].wordType == "number")
                            //    {
                            //        index++;
                            //        if (analizedText[index].wordType == "right_bracket")
                            //        {
                            //            return true;
                            //        }
                            //        else
                            //        {
                            //            ErrorMessage(")");
                            //            return false;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        ErrorMessage("number");
                            //        return false;
                            //    }
                            //}
                            //else
                            //{
                            //    if (analizedText[index].wordText == "-")
                            //    {
                            //        index++;
                            //    }
                            //    if (analizedText[index].wordType == "number")
                            //    {
                            //        index++;
                            //        if (analizedText[index].wordType == "right_bracket")
                            //        {
                            //            return true;
                            //        }
                            //        else
                            //        {
                            //            ErrorMessage(")");
                            //            return false;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        ErrorMessage("number");
                            //        return false;
                            //    }
                            //}
                        }
                        else
                        {
                        ErrorMessage("Logic Symbol");
                        return false;
                        }
                }
                else
                {
                    ErrorMessage("variable");
                    return false;
                }
            }
            else
            {
                ErrorMessage("(");
                return false;
            }
        }

        private bool Branching()
        {
            index++;
            if (BolleanExpressin())
            {
                index++;
                if (analizedText[index].wordType == "expression_begin")
                {
                    index++;
                    while (analizedText[index].wordType != "expression_end")
                    {
                        if (!Operator())
                        {
                            return false;
                        }
                        index++;
                    }
                    if (analizedText[index+1].wordType == "else")
                    {
                        index++;
                        index++;
                        if (analizedText[index].wordType == "expression_begin")
                        {
                            index++;
                            while (analizedText[index].wordType != "expression_end")
                            {
                                if (!Operator())
                                {
                                    return false;
                                }
                                index++;
                            }                           
                        }
                        else
                        {
                            ErrorMessage("{");
                            return false;
                        }
                    }
                }
                else
                {
                    ErrorMessage("{");
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        public List<Word> GetAnalizedText()
        {
            return analizedText;
        }

        public List<string> GetVariable()
        {
            return variable;
        }
    }
}




