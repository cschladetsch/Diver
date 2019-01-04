﻿namespace Diver.Language
{
    /// <summary>
    /// Parser for the in-fix Rho language that uses tabs for block definitions like Python.
    /// </summary>
    public partial class RhoParser
        : ParserCommon<RhoLexer, RhoAstNode, RhoToken, ERhoToken, ERhoAst, RhoAstFactory>
        , IParser
    {
        public RhoAstNode Result => _stack.Peek();

        public RhoParser(RhoLexer lexer, IRegistry reg, EStructure st)
            : base(lexer, reg)
        {
            _current = 0;
            _indent = 0;
            _structure = st;
        }

        public bool Process()
        {
            if (_lexer.Failed)
                return Fail(_lexer.Error);

            RemoveWhitespace();

            return Parse(_structure);
        }

        private bool Parse(EStructure st)
        {
            _stack.Push(NewNode(ERhoAst.Program));

            var result = false;
            switch (st)
            {
                case EStructure.Program:
                    result = Program();
                    break;

                case EStructure.Statement:
                    result = Statement();
                    break;

                case EStructure.Expression:
                    result = Expression();
                    break;
            }

            if (Failed || !result)
                return false;

            ConsumeNewLines();

            if (!Try(ERhoToken.None))
                return FailWith("Unexpected extra stuff found");

            return _stack.Count == 1 || Fail("Stack not empty after parsing");
        }

        private bool Program()
        {
            while (!Failed && !Try(ERhoToken.None))
                if (!Statement())
                    return false;

            return true;
        }

        private bool Block()
        {
            ConsumeNewLines();

            var indent = 0;
            while (TryConsume(ERhoToken.Tab))
                ++indent;

            if (indent == 0)
                return false;

            Push(NewNode(ERhoAst.Block));
            while (!Failed)
            {
                if (Try(ERhoToken.None))
                    return true;

                if (!Statement())
                    return FailWith("Statement expected");

                ConsumeNewLines();

                var level = 0;
                while (TryConsume(ERhoToken.Tab))
                    ++level;

                if (level < indent)
                {
                    // return to start so top block can continue
                    _current -= indent;
                    return true;
                }

                if (level != indent)
                    return FailWith("Mismatch block indent");
            }

            return false;
        }

        private bool TryConsume(ERhoToken token)
        {
            ConsumeNewLines();
            if (!Try(token))
                return false;
            Consume();
            return true;
        }

        private void RemoveWhitespace()
        {
            var prevNl = true;
            foreach (var tok in _lexer.Tokens)
            {
                // remove useless consecutive newlines
                var nl = tok.Type == ERhoToken.NewLine;
                if (prevNl && nl)
                    continue;

                prevNl = nl;

                switch (tok.Type)
                {
                    // keep tabs!
                    case ERhoToken.Space:
                    case ERhoToken.Comment:
                        continue;
                }

                _tokens.Add(tok);
            }
        }

        private void ConsumeNewLines()
        {
            while (Try(ERhoToken.NewLine))
                Consume();
        }

        private readonly EStructure _structure;
    }
}
