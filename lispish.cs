// Samuel Fisher (fishe108)
// CSE 465
// Project 2
// Dr. Femiani
// Due: April 30th, extended to May 3rd, 2022
// Implementing a parser for a simple LISP-like language

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class LispishParser {

    public class Parser {
        List<Node> tokens = new List<Node>();
        int cur = 0;

        public Parser(Node[] tokens) {
            this.tokens = new List<Node>(tokens);
            this.tokens.Add(new Node(Symbols.INVALID, ""));
        }

        public Node ParseProgram() {
            var children = new List<Node>();
            while (tokens[cur].Symbol != Symbols.INVALID) {
                children.Add(ParseSExpr());
            }
            return new Node(Symbols.Program, children.ToArray());
        }

        public Node ParseSExpr() {
            if (tokens[cur].Text == "(") {
                return new Node(Symbols.SExpr, ParseList());
            } else {
                return new Node(Symbols.SExpr, ParseAtom());
            }
        }
        
	public Node ParseLiteral(string literal) {
            if (tokens[cur].Text == literal) {
                return tokens[cur++];
            } else {
                throw new Exception("Threw an exception on invalid input.");
            }
        }

        public Node ParseList() {
            Node node = tokens[cur];
            cur++;
            if (tokens[cur].Text == ")") {
                return new Node(Symbols.List, node, tokens[cur]);
            } else {
                Node seq = ParseSeq();
                if (tokens[cur].Text == ")") {
                    return new Node(Symbols.List, node, seq, tokens[cur++]);
                } else {
                    throw new Exception ("Threw an exception on invalid input.");
                }
            }
        }

        public Node ParseSeq() {
            Node sExp = (ParseSExpr());
            if (tokens[cur].Text == ")") {
                return new Node(Symbols.Seq, sExp);
            } else {
                return new Node(Symbols.Seq, sExp, ParseSeq());
            }
        }

        public Node ParseAtom() {
            if (tokens[cur].Symbol == Symbols.ID) {
                return new Node(Symbols.Atom, tokens[cur++]);
            } else if (tokens[cur].Symbol == Symbols.INT) {
                return new Node(Symbols.Atom, tokens[cur++]);
            } else if (tokens[cur].Symbol == Symbols.REAL) {
                return new Node(Symbols.Atom, tokens[cur++]);
            } else if (tokens[cur].Symbol == Symbols.STRING) {
                return new Node(Symbols.Atom, tokens[cur++]);
            } else {
                throw new Exception("Threw an exception on invalid input.");
            }
        }
    }

    // Holds all terminal and non-terminal symbols for the intended grammar
    public enum Symbols {
	Atom,
        List,
        Seq,
        SExpr,
        Program,
	INVALID,
	WS,
	ID,
	INT,
	STRING,
	LITERAL,
	REAL
    }

    public class Node {
        public Symbols Symbol;
        public string Text = "";
        List<Node> children = new List<Node>();

        public Node(Symbols symbol, string text) {
            this.Symbol = symbol;
            this.Text = text;
        }

        public Node(Symbols symbol, params Node[] children) {
            this.Symbol = symbol;
            this.Text = "";
            this.children = new List<Node>(children);
        }

	// Help with printing parse tree, written in Dr. Femiani's lecture
        public void Print(string prefix = "") {
            Console.WriteLine($"{prefix}{Symbol.ToString().PadRight(42 - prefix.Length)} {Text}");
            foreach (var child in children) {
                child.Print(prefix + "  ");
            }
        }
    }

    static public List<Node> Tokenize(String src) {
        int pos = 0;
        var result = new List<Node>();
        Match mat;

	// holding regular expression values for symbols
        var WS = new Regex(@"\G\s");
        var INT = new Regex(@"\G[+-]?[0-9]+");
	var REAL = new Regex(@"\G\s*([+-]?[0-9]*\.[0-9]+)");
	var STRING = new Regex(@"\G\s*""(\\.|[^""])*[^\\]""");
	var ID = new Regex(@"\G\s*[^\s\""\\(\\)]+");
	var LIT = new Regex(@"\G\s*([\\(\\)])");

        while (pos < src.Length) {
	    // Check if there is a match between input and
	    // regular expression values written above
	    // Correct order: WS, REAL, INT, STRING, ID, LIT
            if ((mat = WS.Match(src, pos)).Success) {
                pos += mat.Length;
            } else if ((mat = REAL.Match(src, pos)).Success) {
                result.Add(new Node(Symbols.REAL, mat.Value));
                pos += mat.Length;
            } else if ((mat = INT.Match(src, pos)).Success) {
                result.Add(new Node(Symbols.INT, mat.Value));
                pos += mat.Length;
            } else if ((mat = STRING.Match(src, pos)).Success) {
                result.Add(new Node(Symbols.STRING, mat.Value));
                pos += mat.Length;
            } else if ((mat = ID.Match(src, pos)).Success) {
                result.Add(new Node(Symbols.ID, mat.Value));
                pos += mat.Length;
            } else if ((mat = LIT.Match(src, pos)).Success) {
                result.Add(new Node(Symbols.LITERAL, mat.Value));
                pos += mat.Length;
            } else {
                throw new Exception("Threw an exception on invalid input.");
            }
        }
        return result;
    }

    static public Node Parse(Node[] tokens) {
        var p = new Parser(tokens);
        var tree = p.ParseProgram();
        return tree;
    }

    static private void CheckString(string lispcode) {
        try {
            Console.WriteLine(new String('=', 50));
            Console.Write("Input: ");
            Console.WriteLine(lispcode);
            Console.WriteLine(new String('-', 50));

            Node[] tokens = Tokenize(lispcode).ToArray();

            Console.WriteLine("Tokens");
            Console.WriteLine(new String('-', 50));
            foreach (Node node in tokens) {
                Console.WriteLine($"{node.Symbol,-21}\t: {node.Text}");
            }
            Console.WriteLine(new String('-', 50));

            Node parseTree = Parse(tokens);

            Console.WriteLine("Parse Tree");
            Console.WriteLine(new String('-', 50));
            parseTree.Print();
            Console.WriteLine(new String('-', 50));
        }
        catch (Exception e) {
            Console.WriteLine(e.Message);
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
