using System;
using System.Collections.Generic;

public class LispishParser
{
    public class Node
    {
        public void Print(string prefix = "")
        {
        }
    }

    static public List<Node> Tokenize(String src)
    {
        return null;
    }

    static public Node Parse(Node[] tokens)
    {
        return null;
    }

    static private void CheckString(string lispcode)
    {
        try
        {
            Console.WriteLine(new String('=', 50));
            Console.Write("Input: ");
            Console.WriteLine(lispcode);
            Console.WriteLine(new String('-', 50));

            Node[] tokens = Tokenize(lispcode).ToArray();

            Console.WriteLine("Tokens");
            Console.WriteLine(new String('-', 50));
            foreach (Node node in tokens)
            {
                // TODO - print the node symbol and text
            }
            Console.WriteLine(new String('-', 50));

            Node parseTree = Parse(tokens);

            Console.WriteLine("Parse Tree");
            Console.WriteLine(new String('-', 50));
            parseTree.Print();
            Console.WriteLine(new String('-', 50));
        }
        catch (Exception)
        {
            Console.WriteLine("Threw an exception on invalid input.");
        }
    }


    public static void Main(string[] args)
    {
        //Here are some strings to test on in 
        //your debugger. You should comment 
        //them out before submitting!

        // CheckString(@"(define foo 3)");
        // CheckString(@"(define foo ""bananas"")");
        // CheckString(@"(define foo ""Say \\""Chease!\\"" "")");
        // CheckString(@"(define foo ""Say \\""Chease!\\)");
        // CheckString(@"(+ 3 4)");      
        // CheckString(@"(+ 3.14 (* 4 7))");
        // CheckString(@"(+ 3.14 (* 4 7)");

        CheckString(Console.In.ReadToEnd());
    }
}

